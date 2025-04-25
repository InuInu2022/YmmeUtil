using System.Collections.Immutable;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using ImpromptuInterface;

namespace YmmeUtil.Ymm4.Internal;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011")]
internal static class Reflect
{
	/// <summary>
	/// GetProp from ViewModel class
	/// </summary>
	/// <param name="vm">ViewModel class</param>
	/// <param name="propName">Property name of ViewModel class</param>
	/// <param name="isPrivate">Indicates if the property is private</param>
	/// <returns></returns>
	/// <seealso cref="GetProp{T}(dynamic, string, bool)"/>
	internal static dynamic? GetProp(dynamic vm, string propName, bool isPrivate = false)
	{
		Type vmType = vm.GetType();

		var propertyInfo = vmType.GetProperty(
			propName,
			bindingAttr: isPrivate
				? BindingFlags.NonPublic | BindingFlags.Instance
				: BindingFlags.Public | BindingFlags.Instance
		);

		return propertyInfo?.GetValue(vm);
	}

	/// <inheritdoc cref="GetProp(dynamic, string, bool)"/>
	internal static T GetProp<T>(dynamic vm, string propName, bool isPrivate = false)
	{
		return (T)GetProp(vm, propName, isPrivate);
	}

	/// <summary>
	/// GetField from ViewModel class
	/// </summary>
	/// <param name="vm">ViewModel class</param>
	/// <param name="fieldName">Field name of ViewModel class</param>
	/// <param name="isPrivate">Indicates if the field is private</param>
	/// <returns></returns>
	/// <seealso cref="GetField{T}(dynamic, string, bool)"/>
	internal static dynamic? GetField(dynamic vm, string fieldName, bool isPrivate = false)
	{
		Type vmType = vm.GetType();
		var fieldInfo = vmType.GetField(
			fieldName,
			bindingAttr: isPrivate
				? BindingFlags.NonPublic | BindingFlags.Instance
				: BindingFlags.Public | BindingFlags.Instance
		);

		return fieldInfo?.GetValue(vm);
	}

	/// <inheritdoc cref="GetField(dynamic, string, bool)"/>
	internal static T GetField<T>(dynamic vm, string fieldName, bool isPrivate = false)
	{
		return (T)GetField(vm, fieldName, isPrivate);
	}

	/// <summary>
	/// ImmutableListのプロパティから値を取得する
	/// </summary>
	/// <typeparam name="TWrapper">変換先のラッパー型</typeparam>
	/// <param name="sourceObject">ソースオブジェクト</param>
	/// <param name="propertyName">プロパティ名</param>
	/// <param name="factory">TWrapper型のインスタンスを生成するファクトリ関数</param>
	/// <returns>変換されたラッパーコレクション</returns>
	/// <exception cref="InvalidOperationException">プロパティが見つからないか、ImmutableList型でない場合</exception>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "SMA0040:Missing Using Statement", Justification = "<保留中>")]
	public static ImmutableList<TWrapper> GetImmutableListProp<TWrapper>(
		object sourceObject,
		string propertyName,
		Func<dynamic, TWrapper> factory
	)
		where TWrapper : class
	{
		ArgumentNullException.ThrowIfNull(sourceObject);
		ArgumentNullException.ThrowIfNull(factory);

		var sourceType = sourceObject.GetType();

		var propInfo = sourceType.GetProperty(
			propertyName,
			BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
		) ?? throw new InvalidOperationException(
			$"プロパティ '{propertyName}' が見つかりません。"
		);

		var propType = propInfo.PropertyType;

		if (!propType.IsGenericType || propType.GetGenericTypeDefinition() != typeof(ImmutableList<>))
		{
			throw new InvalidOperationException(
				$"プロパティ '{propertyName}' は ImmutableList<T> ではありません。"
			);
		}

		var value = propInfo.GetValue(sourceObject);
		if (value == null)
		{
			return ImmutableList<TWrapper>.Empty;
		}

		if (value is not System.Collections.IEnumerable enumerableValue)
		{
			throw new InvalidOperationException(
				$"プロパティ '{propertyName}' の値をIEnumerableに変換できません。"
			);
		}

		// リフレクションでIEnumerable<T>をループ処理
		var result = new List<TWrapper>();
		var enumerator = enumerableValue.GetEnumerator();

		while (enumerator.MoveNext())
		{
			if (enumerator.Current != null)
			{
				var wrapper = factory(enumerator.Current);
				if (wrapper != null)
				{
					result.Add(wrapper);
				}
			}
		}

		// IDisposableの場合は破棄
		(enumerator as IDisposable)?.Dispose();

		// ListをImmutableListに変換して返す
		return [.. result];
	}

	/// <summary>
	/// ImmutableListのプロパティを割り当てる
	/// </summary>
	/// <typeparam name="TWrapper"></typeparam>
	/// <param name="targetObject"></param>
	/// <param name="propertyName"></param>
	/// <param name="rawItems"></param>
	/// <exception cref="InvalidOperationException"></exception>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051")]
	public static void SetImmutableListProp<TWrapper>(
		object targetObject,
		string propertyName,
		IEnumerable<TWrapper> rawItems
	)
		where TWrapper : class
	{
		ArgumentNullException.ThrowIfNull(targetObject);
		rawItems ??= [];

		var targetType = targetObject.GetType();

		var propInfo = targetType.GetProperty(
			propertyName,
			BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
		) ?? throw new InvalidOperationException(
			$"プロパティ '{propertyName}' が見つかりません。"
		);

		var propType = propInfo.PropertyType;

		if (!propType.IsGenericType || propType.GetGenericTypeDefinition() != typeof(ImmutableList<>))
		{
			throw new InvalidOperationException(
				$"プロパティ '{propertyName}' は ImmutableList<T> ではありません。"
			);
		}

		var interfaceType = propType.GetGenericArguments()[0];

		var listType = typeof(List<>).MakeGenericType(interfaceType);
		var list = Activator.CreateInstance(listType);
		var addMethod = listType.GetMethod("Add");

		foreach (var item in rawItems)
		{
			if (item is null) continue;

			var expando = new ExpandoObject() as IDictionary<string, object>;

			foreach (var prop in typeof(TWrapper).GetProperties(
				BindingFlags.Public | BindingFlags.Instance))
			{
				try
				{
					var val = prop.GetValue(item);
					if (val is null) continue;
					expando[prop.Name] = val;
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error setting property '{prop.Name}': {e.Message}");
				}
			}

			// インターフェースに変換して追加
			var actedItem = expando.ActLike(interfaceType);
			addMethod?.Invoke(list, [ actedItem ]);
		}

		var immutableType = typeof(ImmutableList);

		var createRangeMethod = immutableType
			.GetMethods(BindingFlags.Public | BindingFlags.Static)
			.FirstOrDefault(m =>
				string.Equals(m.Name, "CreateRange", StringComparison.Ordinal)
				&& m.IsGenericMethod
				&& m.GetParameters().Length == 1)
				?? throw new InvalidOperationException("ImmutableList.CreateRangeメソッドが見つかりません。");

		// ジェネリックメソッドをインスタンス化
		var genericCreateRange = createRangeMethod.MakeGenericMethod(interfaceType);

		// ImmutableListを作成
		var immutableList = genericCreateRange.Invoke(null, [list]);

		// プロパティに設定
		propInfo.SetValue(targetObject, immutableList);
	}
}
