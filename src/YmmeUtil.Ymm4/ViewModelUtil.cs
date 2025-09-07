using System.Windows;
using System.Windows.Media;

using YukkuriMovieMaker.ViewModels;

namespace YmmeUtil.Ymm4;

public static class ViewModelUtil
{
	/// <summary>
	/// YMM4のメインウィンドウのViewModelを取得します
	/// </summary>
	public static IMainViewModel? GetParentViewModel(
		DependencyObject child
	)
	{
		var parent = child;
		while (parent is not null)
		{
			if (
				parent is FrameworkElement fe
				&& fe.DataContext is IMainViewModel vm
			)
			{
				return vm;
			}
			parent = VisualTreeHelper.GetParent(parent);
		}
		return null;
	}
}