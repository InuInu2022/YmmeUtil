using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Threading;

using YmmeUtil.Bridge;
using YmmeUtil.Sandbox.ObjectListTest.View;

using YukkuriMovieMaker.Plugin;

namespace YmmeUtil.Sandbox.ObjectListTest;

[PluginDetails(AuthorName = "InuInu", ContentId = "")]
public class ObjectListTest : IToolPlugin
{
	public string Name { get; } = "Sandbox.ObjectListTest";
	public PluginDetailsAttribute Details =>
		GetType().GetCustomAttribute<PluginDetailsAttribute>() ?? new();

	public Type ViewModelType { get; } = typeof(MainViewModel);
	public Type ViewType { get; } = typeof(MainView);

	private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

	public ObjectListTest()
	{

		var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };

		void TickEvent(object? s, EventArgs e)
		{
			foreach (Window win in Application.Current.Windows)
			{
				// スプラッシュではなく、実ウィンドウかを判定
				if (IsRealUiWindow(win) && win.IsLoaded)
				{
					timer.Stop();
					OnUiReady(win);
					timer.Tick -= TickEvent;
					return;
				}
			}
		}
		timer.Tick += TickEvent;

		timer.Start();
	}

	private static bool IsRealUiWindow(Window window)
	{
		return !string.IsNullOrWhiteSpace(window.Title) && window.Title != "Splash";
	}

	private static void OnUiReady(Window mainWindow)
	{
		// 本体UIにアクセス可能
		//test
		var hasTL = TimelineUtil.TryGetTimeline(out var timeLine);
		if (!hasTL || timeLine is null)
			return;
		Debug.WriteLine(JsonSerializer.Serialize(timeLine.VideoInfo, _jsonOptions));
	}
}
