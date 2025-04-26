using System.Diagnostics;
using YmmeUtil.Ymm4.Wrap;
using YmmeUtil.Ymm4.Wrap.Items;
using YukkuriMovieMaker.ViewModels;

namespace YmmeUtil.Ymm4;

public static class TimelineUtil
{
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
			// リフレクションでAddItemsメソッドを探す
			var timelineType = timeline.RawTimeline.GetType();
			var addItemsMethod = timelineType.GetMethod(
				"AddItems",
				System.Reflection.BindingFlags.Instance
					| System.Reflection.BindingFlags.Public
					| System.Reflection.BindingFlags.NonPublic
			);

			if (addItemsMethod != null)
			{
				var paramType = addItemsMethod.GetParameters()[0].ParameterType;
				var elementType = paramType.GenericTypeArguments[0]; // IEnumerable<T>のT型を取得

				// 型付きリストを動的に作成
				var listType = typeof(List<>).MakeGenericType(elementType);
				var typedList = Activator.CreateInstance(listType);

				// Add メソッドを取得
				var addMethod = listType.GetMethod("Add");

				// 各アイテムをリストに追加
				foreach (var item in items)
				{
					if (item?.RawItem != null)
					{
						addMethod.Invoke(typedList, new[] { item.RawItem });
					}
				}

				// メソッドを呼び出す
				addItemsMethod.Invoke(timeline.RawTimeline, new[] { typedList });
				return true;
			}
			Debug.WriteLine("AddItemsメソッドが見つかりませんでした。");
			return false;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"アイテム追加中にエラーが発生しました: {ex.Message}");
			return false;
		}
	}
}
