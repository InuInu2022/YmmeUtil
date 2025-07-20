using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using Dynamitey;
using ImpromptuInterface;

namespace YmmeUtil.Bridge.Internal;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011")]
internal static class Reflect
{
	// 型情報のキャッシュ用ディクショナリ
	static readonly ConcurrentDictionary<string, Type> _typeCache = new(StringComparer.Ordinal);

	// 最初に取得したIItem型をキャッシュ
	static Type? _cachedItemType;

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
	[System.Diagnostics.CodeAnalysis.SuppressMessage(
		"Usage",
		"SMA0040:Missing Using Statement",
		Justification = "<保留中>"
	)]
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

		var propInfo =
			sourceType.GetProperty(
				propertyName,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
			)
			?? throw new InvalidOperationException(
				$"プロパティ '{propertyName}' が見つかりません。"
			);

		var propType = propInfo.PropertyType;

		if (
			!propType.IsGenericType
			|| propType.GetGenericTypeDefinition() != typeof(ImmutableList<>)
		)
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
	/// <exception cref="InvalidOperationException"></exception>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051")]
	public static void SetImmutableListProp<TWrapper>(
		object targetObject,
		string propertyName,
		IEnumerable<TWrapper> wrapperItems
	)
	{
		ArgumentNullException.ThrowIfNull(targetObject);
		wrapperItems ??= [];

		var targetType = targetObject.GetType();

		var propInfo =
			targetType.GetProperty(
				propertyName,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
			)
			?? throw new InvalidOperationException(
				$"プロパティ '{propertyName}' が見つかりません。"
			);

		var propType = propInfo.PropertyType;

		if (
			!propType.IsGenericType
			|| propType.GetGenericTypeDefinition() != typeof(ImmutableList<>)
		)
		{
			throw new InvalidOperationException(
				$"プロパティ '{propertyName}' は ImmutableList<T> ではありません。"
			);
		}

		var interfaceType = propType.GetGenericArguments()[0];

		var listType = typeof(List<>).MakeGenericType(interfaceType);
		var list = Activator.CreateInstance(listType);
		var addMethod = listType.GetMethod("Add");

		// wrapperItemsからRawItemを抽出するためのプロパティ情報取得
		var rawItemProp = typeof(TWrapper).GetProperty("RawItem");

		if (rawItemProp is null)
		{
			throw new InvalidOperationException(
				$"型 '{typeof(TWrapper).Name}' には 'RawItem' プロパティがありません。"
			);
		}

		foreach (var wrapperItem in wrapperItems)
		{
			if (wrapperItem is null)
				continue;

			try
			{
				// RawItemを取得
				var rawItem = rawItemProp.GetValue(wrapperItem);

				if (rawItem != null)
				{
					// 直接RawItemを追加（変換なし）
					addMethod?.Invoke(list, [rawItem]);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"アイテムの追加中にエラーが発生しました: {e.Message}");
			}
		}

		var immutableType = typeof(ImmutableList);

		var createRangeMethod =
			immutableType
				.GetMethods(BindingFlags.Public | BindingFlags.Static)
				.FirstOrDefault(m =>
					string.Equals(m.Name, "CreateRange", StringComparison.Ordinal)
					&& m.IsGenericMethod
					&& m.GetParameters().Length == 1
				)
			?? throw new InvalidOperationException(
				"ImmutableList.CreateRangeメソッドが見つかりません。"
			);

		// ジェネリックメソッドをインスタンス化
		var genericCreateRange = createRangeMethod.MakeGenericMethod(interfaceType);

		// ImmutableListを作成
		var immutableList = genericCreateRange.Invoke(null, [list]);

		// プロパティに設定
		propInfo.SetValue(targetObject, immutableList);
	}

	static Type GetCachedItemType(dynamic timeline)
	{
		// キャッシュがあればそれを返す
		if (_cachedItemType != null)
			return _cachedItemType;

		// キャッシュがなければ動的に取得
		var timelineType = timeline.GetType();
		var addItemsMethod =
			timelineType.GetMethod(
				"AddItems",
				System.Reflection.BindingFlags.Instance
					| System.Reflection.BindingFlags.Public
					| System.Reflection.BindingFlags.NonPublic
			) ?? throw new InvalidOperationException("AddItemsメソッドが見つかりません");
		var paramType = addItemsMethod.GetParameters()[0].ParameterType;
		var elementType = paramType.GetGenericArguments()[0];

		// 型をキャッシュして返す
		_cachedItemType = elementType;
		return elementType;
	}
}
