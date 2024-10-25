using System.Windows;
using System.Windows.Shell;

namespace YmmeUtil.Ymm4;

public static class TaskbarUtil
{
	public static TaskbarItemInfo GetMainTaskbarInfo()
	{
		var main = Application.Current.MainWindow;
		main.TaskbarItemInfo ??= new TaskbarItemInfo();
		return main.TaskbarItemInfo;
	}

	public static void StartIndeterminate(TaskbarItemInfo? taskbar = null)
	{
		taskbar ??= GetMainTaskbarInfo();

		taskbar.ProgressState = TaskbarItemProgressState.Indeterminate;
	}

	public static void PauseIndeterminate(TaskbarItemInfo? taskbar = null)
	{
		taskbar ??= GetMainTaskbarInfo();

		taskbar.ProgressState = TaskbarItemProgressState.Paused;
	}

	public static void FinishIndeterminate(TaskbarItemInfo? taskbar = null)
	{
		taskbar ??= GetMainTaskbarInfo();

		taskbar.ProgressState = TaskbarItemProgressState.None;
	}
}
