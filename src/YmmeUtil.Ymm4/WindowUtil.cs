using System.Windows;

using YukkuriMovieMaker.Commons;

namespace YmmeUtil.Ymm4;

public static class WindowUtil
{
	/// <summary>
	/// 外部のアプリにフォーカスが移った後に呼ぶとYMM4にフォーカスを戻すことができる
	/// </summary>
	public static void FocusBack()
	{
		if (FocusHelper.DefaultFocus is null) { return; }

		Window.GetWindow(FocusHelper.DefaultFocus).Activate();
		FocusHelper.FocusWindowContent(FocusHelper.DefaultFocus);
	}

	/// <summary>
	/// YMM4のメインウィンドウを取得します
	/// </summary>
	/// <returns></returns>
	public static Window GetYmmMainWindow()
	{
		return GetWindows()
			.First(w =>
				string.Equals(
					w.GetType().FullName,
					"YukkuriMovieMaker.Views.MainView",
					StringComparison.Ordinal
				)
			);
	}

	public static Window? GetToolWindow(string windowName)
	{
		return GetWindows()
			.FirstOrDefault(w =>
				w.Title is not null
				&& (w.Title?.Length == 0
					|| w.Title!.Contains(windowName, StringComparison.Ordinal))
			);
	}

	static IEnumerable<Window> GetWindows()
	{
		List<dynamic> windows = [.. Application.Current.Windows];
		return windows
			.OfType<Window>();
	}


}
