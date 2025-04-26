using System.Collections.Concurrent;
using System.Diagnostics;

using Dynamitey;

using YmmeUtil.Ymm4.Wrap;
using YmmeUtil.Ymm4.Wrap.Items;
using YukkuriMovieMaker.ViewModels;

namespace YmmeUtil.Ymm4;

public static class TimelineUtil
{
	// 型情報のキャッシュ用ディクショナリ
	static readonly ConcurrentDictionary<string, Type> _typeCache = new(StringComparer.Ordinal);

	// 最初に取得したIItem型をキャッシュ
	static Type? _cachedItemType;

	/// <summary>
	/// YMM4のメインウィンドウの`Timeline`を取得を試す
	/// </summary>
	/// <param name="timeLine"></param>
	/// <returns></returns>
	public static bool TryGetTimeline(out WrapTimeLine? timeLine)
	{
		timeLine = default;
		var mainWinVM = GetMainViewModel();
		if (mainWinVM is null)
		{
			return false;
		}

		var timelineAreaVM = Internal.Reflect.GetProp(mainWinVM, "TimelineAreaViewModel");
		if (timelineAreaVM is null)
		{
			return false;
		}

		var vm = Internal.Reflect.GetProp(timelineAreaVM, "ViewModel");
		if (vm is null)
			return false;
		var vmValue = Internal.Reflect.GetProp(vm, "Value");
		if (vmValue is null)
			return false;

		var tl = Internal.Reflect.GetField(vmValue, "timeline", true);
		if (tl is null)
			return false;
		timeLine = new WrapTimeLine(tl);
		return true;
	}

	public static IMainViewModel? GetMainViewModel()
	{
		var mainWindow = WindowUtil.GetYmmMainWindow();
		if (mainWindow is null)
			return default;
		dynamic viewModel = mainWindow.DataContext;
		return viewModel as IMainViewModel;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011")]
	public static bool TryAddItems(
		this WrapTimeLine timeline,
		IEnumerable<IWrapBaseItem> items
	)
	{
		if (timeline is null || items?.Any() is not true)
		{
			return false;
		}

		try
		{
			// キャッシュされたIItem型を使用するか、新たに取得してキャッシュ
			Type itemType = GetCachedItemType(timeline.RawTimeline);
			dynamic itemsList = Dynamic.InvokeConstructor(typeof(List<>).MakeGenericType(itemType));

			// RawItemをリストに追加
			foreach (var item in items)
			{
				if (item?.RawItem != null)
				{
					Dynamic.InvokeMemberAction(itemsList, "Add", item.RawItem);
				}
			}

			// AddItemsメソッドを動的に呼び出す
			Dynamic.InvokeMemberAction(timeline.RawTimeline, "AddItems", itemsList);
			return true;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"アイテム追加中にエラーが発生しました: {ex.Message}");
			return false;
		}
	}
	// IItem型を取得してキャッシュするメソッド
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011")]
	static Type GetCachedItemType(dynamic timeline)
	{
		// キャッシュがあればそれを返す
		if (_cachedItemType != null)
			return _cachedItemType;

		// キャッシュがなければ動的に取得
		var timelineType = timeline.GetType();
		var addItemsMethod = timelineType.GetMethod(
			"AddItems",
			System.Reflection.BindingFlags.Instance
				| System.Reflection.BindingFlags.Public
				| System.Reflection.BindingFlags.NonPublic
		)
			?? throw new InvalidOperationException("AddItemsメソッドが見つかりません");
		var paramType = addItemsMethod.GetParameters()[0].ParameterType;
		var elementType = paramType.GetGenericArguments()[0];

		// 型をキャッシュして返す
		_cachedItemType = elementType;
		return elementType;
	}

	// 型名から型を取得してキャッシュするメソッド
	static Type GetOrCreateType(string typeName, dynamic referenceObject)
	{
		return _typeCache.GetOrAdd(
			typeName,
			name =>
			{
				// 参照オブジェクトと同じアセンブリから型を取得
				var assembly = referenceObject.GetType().Assembly;
				var type = assembly.GetType(name);

				if (type == null)
					throw new TypeLoadException($"型 '{name}' が見つかりません");

				return type;
			}
		);
	}
}
