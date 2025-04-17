using YmmeUtil.Ymm4.Wrap;

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
}
