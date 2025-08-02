using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;

using YmmeUtil.Bridge.Wrap.Items;

using YmmeUtil.Bridge.Internal;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Plugin;

namespace YmmeUtil.Bridge.Wrap.ViewModels;

public partial class WrapTimelineItemViewModel
	: Bindable,
		IDisposable
{
	private bool _disposedValue;

	public WrapTimelineItemViewModel(dynamic timelineItemViewModel)
	{
		RawTimelineItemViewModel = timelineItemViewModel;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public dynamic RawTimelineItemViewModel { get; }
	public bool IsSelected
	{
		get =>
			Reflect.GetProp<bool>(
				RawTimelineItemViewModel,
				nameof(IsSelected)
			);
		set =>
			Reflect.SetProp(
				RawTimelineItemViewModel,
				nameof(IsSelected),
				value,
				true
			);
	}

	public bool IsLocked
	{
		get =>
			Reflect.GetProp<bool>(
				RawTimelineItemViewModel,
				nameof(IsLocked)
			);
		set => RawTimelineItemViewModel.IsLocked = value;
	}

	public bool IsVisible
	{
		get =>
			Reflect.GetProp<bool>(
				RawTimelineItemViewModel,
				nameof(IsVisible)
			);
		set => RawTimelineItemViewModel.IsVisible = value;
	}

	public Color Color
	{
		get =>
			Reflect.GetProp<Color>(
				RawTimelineItemViewModel,
				nameof(Color)
			);
		set => RawTimelineItemViewModel.Color = value;
	}

	public string Description
	{
		get =>
			Reflect.GetProp<string>(
				RawTimelineItemViewModel,
				nameof(Description)
			);
		set => RawTimelineItemViewModel.Description = value;
	}

	public string Label
	{
		get =>
			Reflect.GetProp<string>(
				RawTimelineItemViewModel,
				nameof(Label)
			);
		set => RawTimelineItemViewModel.Label = value;
	}

	public int Layer
	{
		get =>
			Reflect.GetProp<int>(
				RawTimelineItemViewModel,
				nameof(Layer)
			);
	}

	public ICommand SelectCommand =>
		Reflect.GetProp<ICommand>(
			RawTimelineItemViewModel,
			nameof(SelectCommand)
		);

	public IWrapBaseItem Item
	{
		get
		{
			var raw = Reflect.GetProp<dynamic>(
				RawTimelineItemViewModel,
				nameof(Item)
			);
			return new WrapBaseItem(raw);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public dynamic RawItem
	{
		get => Item.RawItem;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				RawTimelineItemViewModel?.Dispose();
			}

			// TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
			// TODO: 大きなフィールドを null に設定します
			_disposedValue = true;
		}
	}

	// // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
	// ~WrapTimelineItemViewModel()
	// {
	//     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
	//     Dispose(disposing: false);
	// }

	public void Dispose()
	{
		// このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
