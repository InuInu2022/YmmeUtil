using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using Epoxy;
using YmmeUtil.Bridge;
using YmmeUtil.Bridge.Wrap;
using YmmeUtil.Bridge.Wrap.Items;
using YmmeUtil.Bridge.Wrap.ViewModels;
using YmmeUtil.Ymm4;

namespace YmmeUtil.Sandbox.ObjectListTest.View;

[ViewModel]
public class MainViewModel
{
	public Command? Ready { get; set; }
	public Command? ReloadCommand { get; set; }
	public Command? SelectionChangedCommand { get; set; }

	public string SearchText { get; set; } = string.Empty;

	public ObservableCollection<ObjectListItem> Items { get; private set; } = [];

	public ICollectionView? FilteredItems { get; private set; }

	public MainViewModel()
	{
		Ready = Command.Factory.Create(() =>
		{
			//set Title
			List<dynamic> windows = [.. Application.Current.Windows];
			var win = windows.OfType<Window>().FirstOrDefault(w => w.DataContext is MainViewModel);
			if (win is not null)
				win.Title = "YMM オブジェクトリスト（テスト）";


			// App loaded event
			var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
			void TickEvent(object? s, EventArgs e)
			{
				foreach (Window win in Application.Current.Windows)
				{
					// スプラッシュではなく、実ウィンドウかを判定
					if (IsRealUiWindow(win) && win.IsLoaded)
					{
						timer.Stop();
						OnHostUiReady(win);
						timer.Tick -= TickEvent;
						return;
					}
				}
			}
			timer.Tick += TickEvent;
			timer.Start();

			return default;
		});

		ReloadCommand = Command.Factory.Create(() =>
		{
			if (TimelineUtil.TryGetTimeline(out var timeLine) && timeLine is not null)
			{
				UpdateItems(timeLine);
			}

			return default;
		});

		SelectionChangedCommand = Command.Factory.Create<SelectionChangedEventArgs>(
			(e) =>
			{
				if (!TimelineUtil.TryGetTimeline(out var timeLine) || timeLine is null)
				{
					return default;
				}

				var items = e.AddedItems;
				if (items is null)
					return default;

				var wItems = items
					.OfType<ObjectListItem>()
					.Select(item => item.ConvertToItemViewModel())
					.Where(item => item is not null)
					.OfType<WrapTimelineItemViewModel>()
					.ToList();

				foreach (var item in wItems)
				{
					try
					{
						var cmd = item.SelectCommand;
						if (cmd?.CanExecute(null) == true)
						{
							cmd.Execute(null);
						}
					}
					catch (System.Exception ex)
					{
						Debug.WriteLine($"Failed to select item: {item}, {ex.Message}");
					}
				}

				return default;
			}
		);
	}

	static bool IsRealUiWindow(Window window)
	{
		return !string.IsNullOrWhiteSpace(window.Title) && window.Title != "Splash";
	}

	void OnHostUiReady(Window mainWindow)
	{
		var hasTL = TimelineUtil.TryGetTimeline(out var timeLine);

		if (!hasTL || timeLine is null)
			return;

		var raw = timeLine.RawTimeline;

		if (raw is INotifyPropertyChanged target)
		{
			//監視する
			target.PropertyChanged += OnTimelineChanged;
			// ここでタイムラインの変更を反映させる
			UpdateItems(timeLine);
		}
	}

	void FilterItems()
	{
		if (FilteredItems is null)
		{
			return;
		}

		FilteredItems.Filter = item =>
		{
			return item is ObjectListItem yourItem
				&& (
					string.IsNullOrEmpty(SearchText)
					|| yourItem.Label.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
				);
		};
	}

	void OnTimelineChanged(object? sender, PropertyChangedEventArgs e)
	{
		//Debug.WriteLine($"Property changed: {e.PropertyName}");

		if (!TimelineUtil.TryGetTimeline(out var timeLine) || timeLine is null)
			return;

		switch (e.PropertyName)
		{
			case "Items":
				UpdateItems(timeLine);
				break;

			default:
				break;
		}
	}

	void UpdateItems(WrapTimeLine timeLine)
	{
		if (!TimelineUtil.TryGetItemViewModels(out var itemViewModels) || itemViewModels is null)
		{
			return;
		}
		// アイテムビューのモデルが取得できた場合は、アイテムを更新する
		Items = new ObservableCollection<ObjectListItem>(
			itemViewModels.Select(item => new ObjectListItem(item))
		);
		OnItemsChanged();
		return;
	}

	[PropertyChanged(nameof(SearchText))]
	[SuppressMessage("", "IDE0051")]
	private ValueTask SearchTextChangedAsync(string value)
	{
		FilterItems();
		return default;
	}

	void OnItemsChanged()
	{
		FilteredItems = CollectionViewSource.GetDefaultView(Items);
		FilterItems();
	}
}
