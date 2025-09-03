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
#if FOCUSHELPER_DEFAULTFOCUS_REMOVED
		// v4.45.1～？: DefaultFocusが削除された場合の処理
		var mainWindow = GetYmmMainWindow();
		mainWindow?.Activate();
#else
		// v4.45.0.x 以前
		if (FocusHelper.DefaultFocus is null) { return; }

		Window.GetWindow(FocusHelper.DefaultFocus).Activate();
		FocusHelper.FocusWindowContent(FocusHelper.DefaultFocus);
#endif
	}

	/// <summary>
	/// YMM4のメインウィンドウを一つ、取得します
	/// </summary>
	/// <returns>取得できない場合はnullが返ります</returns>
	/// <seealso cref="GetYmmMainWindows"/>
	public static Window? GetYmmMainWindow()
	{
		return GetYmmMainWindows().FirstOrDefault(w => w.IsVisible);
	}

	/// <summary>
	/// YMM4のメインウィンドウをすべて取得します
	/// </summary>
	/// <returns></returns>
	public static IEnumerable<Window> GetYmmMainWindows()
	{
		return GetWindows()
			.Where(w =>
				string.Equals(
					w.GetType().FullName,
					"YukkuriMovieMaker.Views.MainView",
					StringComparison.Ordinal
				)
			);
	}

	/// <summary>
	/// ツールウィンドウを取得します
	/// </summary>
	/// <param name="windowName">ウィンドウ名に含まれる文字列</param>
	/// <returns></returns>
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
		var ws = Application.Current.Windows;
		if (ws is null or [])
		{
			return [];
		}
		List<dynamic> windows = [.. ws];
		return windows
			.OfType<Window>();
	}


}
