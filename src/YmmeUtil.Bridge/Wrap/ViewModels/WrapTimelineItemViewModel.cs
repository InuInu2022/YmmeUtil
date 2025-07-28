using System.Drawing;
using System.Windows.Input;
using YmmeUtil.Bridge.Wrap.Items;

namespace YmmeUtil.Bridge.Wrap.ViewModels;

public partial class WrapTimelineItemViewModel
{
	public WrapTimelineItemViewModel(dynamic timelineItemViewModel)
	{
		RawTimelineItemViewModel = timelineItemViewModel;
	}

	public dynamic RawTimelineItemViewModel { get; }
	public bool IsSelected
	{
		get => Internal.Reflect.GetProp<bool>(RawTimelineItemViewModel, nameof(IsSelected));
		set => Internal.Reflect.SetProp(RawTimelineItemViewModel, nameof(IsSelected), value, true);
	}

	public bool IsLocked
	{
		get => Internal.Reflect.GetProp<bool>(RawTimelineItemViewModel, nameof(IsLocked));
		set => RawTimelineItemViewModel.IsLocked = value;
	}

	public bool IsVisible
	{
		get => Internal.Reflect.GetProp<bool>(RawTimelineItemViewModel, nameof(IsVisible));
		set => RawTimelineItemViewModel.IsVisible = value;
	}

	public Color Color
	{
		get => Internal.Reflect.GetProp<Color>(RawTimelineItemViewModel, nameof(Color));
		set => RawTimelineItemViewModel.Color = value;
	}

	public string Description
	{
		get => Internal.Reflect.GetProp<string>(RawTimelineItemViewModel, nameof(Description));
		set => RawTimelineItemViewModel.Description = value;
	}

	public string Label
	{
		get => Internal.Reflect.GetProp<string>(RawTimelineItemViewModel, nameof(Label));
		set => RawTimelineItemViewModel.Label = value;
	}

	public int Layer
	{
		get => Internal.Reflect.GetProp<int>(RawTimelineItemViewModel, nameof(Layer));
	}

	public ICommand SelectCommand =>
		Internal.Reflect.GetProp<ICommand>(RawTimelineItemViewModel, nameof(SelectCommand));

	public IWrapBaseItem Item
	{
		get
		{
			var raw = Internal.Reflect.GetProp<dynamic>(RawTimelineItemViewModel, nameof(Item));
			return new WrapBaseItem(raw);
		}
	}

	public dynamic RawItem
	{
		get => Item.RawItem;
	}
}
