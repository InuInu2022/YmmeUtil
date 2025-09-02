using System.Diagnostics;
using Epoxy;
using YmmeUtil.Bridge;
using YmmeUtil.Bridge.Wrap.Items;
using YmmeUtil.Ymm4;
using System.Reactive;
using System.Reactive.Linq;

namespace YmmeUtil.Sandbox;

[ViewModel]
public class MainViewModel
{
	public Command? Ready { get; set; }
	public Command? TaskbarUtilCommand { get; set; }
	public Command? WindowUtilCommand { get; set; }
	public Command? TimelineUtilCommand { get; set; }
	public Command? ItemEditorUtilCommand { get; set; }

	public MainViewModel()
	{
		Ready = Command.Factory.Create(() =>
		{
			return default;
		});
		TaskbarUtilCommand = TestTaskbarUtils();
		WindowUtilCommand = TestWindowUtils();
		TimelineUtilCommand = TestTimelineUtils();
		ItemEditorUtilCommand = TestItemEditorUtil();
	}

	static Command TestTaskbarUtils() =>
		Command.Factory.Create(async () =>
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

	static Command TestWindowUtils() =>
		Command.Factory.Create(() =>
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

	static Command TestTimelineUtils() =>
		Command.Factory.Create(() =>
		{
			try
			{
				var hasTL = Bridge.TimelineUtil.TryGetTimeline(out var timeLine);
				Debug.Assert(hasTL, "YMM4のタイムラインが取得できませんでした。");
				if (!hasTL) return default;

				Debug.Assert(timeLine is not null, "YMM4のタイムラインが取得できませんでした。");
				if (timeLine is null) return default;

				var hasItemVm = TimelineUtil.TryGetItemViewModels(out var itemViewModels);
				Debug.Assert(
					hasItemVm,
					"YMM4のアイテムビューが取得できませんでした。"
				);
				Debug.Assert(
					itemViewModels is not null,
					"YMM4のアイテムビューが取得できませんでした。"
				);
				if (itemViewModels is null) return default;

				//get
				Debug.WriteLine($"Timeline ID: {timeLine.Id}");
				Debug.WriteLine($"Timeline Name: {timeLine.Name}");
				Debug.WriteLine($"Timeline CurrentFrame: {timeLine.CurrentFrame}");
				Debug.WriteLine($"Timeline Length: {timeLine.Length}");
				Debug.WriteLine($"Timeline MaxLayer: {timeLine.MaxLayer}");
				timeLine.Items.ToList()
					.ForEach(item => Debug.WriteLine($"Timeline Item:\n\t {item}"));
				//Debug.WriteLine($"Timeline : \n\t {timeLine}");

				//set
				timeLine.Name = "Test";
				Debug.WriteLine($"Timeline Name: {timeLine.Name}");

				timeLine.CurrentFrame = 100;
				Debug.WriteLine($"Timeline CurrentFrame: {timeLine.CurrentFrame}");

				//timeLine.Items = [];
				timeLine.Items.ToList()
					.ForEach(item => Debug.WriteLine($"Timeline Item:\n\t {item}"));
				var t = new WrapTextItem()
				{
					Text = "Test",
					Length = 100,
					FontColor = new System.Windows.Media.Color()
					{
						A = 255,
						R = 255,
						G = 0,
						B = 0
					},
					X = new(100),
					Y = new(100),
				};

				//追加
				timeLine.TryAddItems([t]);

			}
			catch (System.Exception e)
			{
				Debug.WriteLine($"{e.Message} {e.StackTrace}");
			}
			return default;
		});

	static Command TestItemEditorUtil() =>
		Command.Factory.Create(() =>
		{
			try
			{
				var hasIE = ItemEditorUtil.TryGetItemEditor(out var itemEditor);
				Debug.Assert(hasIE, "YMM4のアイテムエディタが取得できませんでした。");
				if (!hasIE) return default;

				Debug.Assert(itemEditor is not null, "YMM4のアイテムエディタが取得できませんでした。");
				if (itemEditor is null) return default;

				//get
				Debug.WriteLine($"""
					ItemEditor EditorInfo: {itemEditor.EditorInfo}
						- EditorType: {itemEditor.EditorInfo.EditorType}
						- Layer: {itemEditor.EditorInfo.Layer}
					ItemEditor Items count: {itemEditor.Items.Value.Count}
						{string.Join("\n\t- ", itemEditor.Items.Value.Select(i => i.RawItemTypeName))}
					""");

				//set


				//追加


			}
			catch (System.Exception e)
			{
				Debug.WriteLine($"{e.Message} {e.StackTrace}");
			}
			return default;
		});

}
