using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;

using YmmeUtil.Bridge.Wrap.Items;

using YmmeUtil.Bridge.Internal;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Plugin;
using YmmeUtil.Ymm4;
using System.Windows.Media;
using System.Windows;

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

	/// <summary>
	/// Gets a value indicating whether the item is hidden.
	/// </summary>
	/// <seealso cref="WrapBaseItem.IsHidden"/>
	public bool IsHidden
	{
		get =>
			Reflect.GetProp<bool>(
				RawTimelineItemViewModel,
				nameof(IsHidden)
			);
		private set => RawTimelineItemViewModel.IsHidden = value;
	}

	[Obsolete($"Use {nameof(IsHidden)} instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool IsVisible
	{
		get => IsHidden;
		set => IsHidden = value;
	}

	public System.Windows.Media.Color Color
	{
		get =>
			Reflect.GetProp<System.Windows.Media.Color>(
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

	[Obsolete($"Deprecated", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public int Layer
	{
		get
		{
			return Ymm4Version.Current > new Version(4, 44, 0, 0)
				? -1
				: Reflect.GetProp<int>(
					RawTimelineItemViewModel,
					nameof(Layer)
				);
		}
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

	public bool IsBackgroundImageVisible
	{
		get =>
			Reflect.GetProp<bool>(
				RawTimelineItemViewModel,
				nameof(IsBackgroundImageVisible)
			);
		set => RawTimelineItemViewModel.IsBackgroundImageVisible = value;
	}

	public ImageSource BackgroundImage
	{
		get =>
			Reflect.GetProp<ImageSource>(
				RawTimelineItemViewModel,
				nameof(BackgroundImage)
			);
		private set => RawTimelineItemViewModel.BackgroundImage = value;
	}

	public Thickness BackgroundImageMargin
	{
		get =>
			Reflect.GetProp<Thickness>(
				RawTimelineItemViewModel,
				nameof(BackgroundImageMargin)
			);
	}

	public bool IsClipping
	{
		get =>
			Reflect.GetProp<bool>(
				RawTimelineItemViewModel,
				nameof(IsClipping)
			);
		private set => RawTimelineItemViewModel.IsClipping = value;
	}

	public bool IsMiniItem
	{
		get =>
			Reflect.GetProp<bool>(
				RawTimelineItemViewModel,
				nameof(IsMiniItem)
			);
		private set => RawTimelineItemViewModel.IsMiniItem = value;
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
