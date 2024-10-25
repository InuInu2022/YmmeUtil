using System.Windows;
using System.Windows.Shell;

namespace YmmeUtil.Ymm4;

public static class TaskbarUtil
{
	/// <summary>
	/// YMM4のメインウィンドウの`TaskbarInfo`を取得（ない場合は生成）
	/// </summary>
	/// <returns></returns>
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

	/// <summary>
	/// エラーがあったとき
	/// </summary>
	/// <param name="taskbar"></param>
	public static void ShowError(TaskbarItemInfo? taskbar = null)
	{
		taskbar ??= GetMainTaskbarInfo();

		taskbar.ProgressState = TaskbarItemProgressState.Error;
	}

	public static void ShowNormal(TaskbarItemInfo? taskbar = null)
	{
		taskbar ??= GetMainTaskbarInfo();

		taskbar.ProgressState = TaskbarItemProgressState.Normal;
	}

	public static void ShowProgress(
		double percent,
		TaskbarItemInfo? taskbar = null
	)
	{
		taskbar ??= GetMainTaskbarInfo();
		if(taskbar.ProgressState != TaskbarItemProgressState.Normal)
		{
			taskbar.ProgressState = TaskbarItemProgressState.Normal;
		}
		taskbar.ProgressValue = percent;
	}

	public static void FinishProgress(
		TaskbarItemInfo? taskbar = null
	)
	{
		taskbar ??= GetMainTaskbarInfo();
		taskbar.ProgressState = TaskbarItemProgressState.None;
	}
}
