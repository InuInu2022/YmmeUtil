using System.Diagnostics;

using Epoxy;
using YmmeUtil.Ymm4;

namespace YmmeUtil.Sandbox;

[ViewModel]
public class MainViewModel
{
	public Command? Ready { get; set; }
	public Command? TaskbarUtilCommand { get; set; }
	public Command? WindowUtilCommand { get; set; }
	public MainViewModel()
	{
		Ready = Command.Factory.Create(() =>
		{

			return default;
		});

		TaskbarUtilCommand = Command.Factory.Create(async () =>
		{
			var taskbar = TaskbarUtil.GetMainTaskbarInfo();
			TaskbarUtil.StartIndeterminate(taskbar);
			await Task.Delay(3000);
			TaskbarUtil.PauseIndeterminate(taskbar);
			await Task.Delay(3000);
			TaskbarUtil.FinishIndeterminate(taskbar);
			await Task.Delay(3000);
			TaskbarUtil.ShowNormal(taskbar);
			await Task.Delay(3000);
			TaskbarUtil.ShowError(taskbar);
			await Task.Delay(3000);
			TaskbarUtil.ShowProgress(0.3);
			await Task.Delay(1000);
			TaskbarUtil.ShowProgress(0.6);
			await Task.Delay(1000);
			TaskbarUtil.ShowProgress(0.9);
			await Task.Delay(1000);
			TaskbarUtil.ShowNormal(taskbar);
			await Task.Delay(1000);
			TaskbarUtil.FinishProgress(taskbar);
		});
		WindowUtilCommand = Command.Factory.Create(() =>
		{
			try
			{
				var main = WindowUtil.GetYmmMainWindow();
				Debug.Assert(main is not null, "YMM4のメインウィンドウが取得できませんでした。");

				var tool = WindowUtil.GetToolWindow("YMM4のツールウィンドウ");
				Debug.Assert(tool is not null, "YMM4のツールウィンドウが取得できませんでした。");
			}
			catch (System.Exception e)
			{
				Debug.WriteLine($"{e.Message} {e.StackTrace}");
			}
			return default;
		});
	}
}