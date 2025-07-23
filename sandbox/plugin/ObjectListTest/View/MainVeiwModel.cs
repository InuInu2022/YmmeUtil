using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Data;
using System.Windows.Media.Animation;
using Epoxy;
using YmmeUtil.Bridge;
using YmmeUtil.Bridge.Wrap;
using YmmeUtil.Ymm4;

namespace YmmeUtil.Sandbox.ObjectListTest.View;

[ViewModel]
public class MainViewModel
{
	public Command? Ready { get; set; }
	public Command? ReloadCommand { get; set; }

	public string SearchText { get; set; } = string.Empty;

	public ObservableCollection<ObjectListItem> Items { get; private set; } = [];

	public ICollectionView? FilteredItems { get; private set; }

	public MainViewModel()
	{
		Ready = Command.Factory.Create(() =>
		{
			var hasTL = TimelineUtil.TryGetTimeline(out var timeLine);

			if (!hasTL || timeLine is null)
				return default;

			var raw = timeLine.RawTimeline;

			if (raw is INotifyPropertyChanged target)
			{
				//監視する
				target.PropertyChanged += OnTimelineChanged;
				// ここでタイムラインの変更を反映させる
				UpdateItems(timeLine);
			}

			return default;
		});
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
		Items = new ObservableCollection<ObjectListItem>(
			timeLine.Items.Select(item => new ObjectListItem(ItemFactory.Create(item)))
		);
		OnItemsChanged();
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
