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
		if (FocusHelper.DefaultFocus is null){return;}

		Window.GetWindow(FocusHelper.DefaultFocus).Activate();
		FocusHelper.FocusWindowContent(FocusHelper.DefaultFocus);
	}
}
