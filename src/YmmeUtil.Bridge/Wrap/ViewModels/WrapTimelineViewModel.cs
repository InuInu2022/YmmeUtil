using System.Collections.Immutable;
using System.ComponentModel;
using Reactive.Bindings;
using YmmeUtil.Bridge.Internal;
using YmmeUtil.Bridge.Wrap.Items;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Plugin;

namespace YmmeUtil.Bridge.Wrap.ViewModels;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.ViewModel.TimelineViewModel
/// </summary>
public partial class WrapTimelineViewModel
	: Bindable,
		IDisposable
{
	bool _disposedValue;
	private ReactiveProperty<WrapSceneTitleViewModel> _selectedScene;

	public WrapTimelineViewModel(
		dynamic timelineAreaViewModel
	)
	{
		RawTimelineVm = timelineAreaViewModel;
	}

	/// <summary>
	/// アイテムの動的オブジェクト。
	/// <para>YMM4本体の実装に依存するため、基本的には直接使用しないでください。</para>
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public dynamic RawTimelineVm { get; private set; }

	public IReadOnlyReactiveProperty<double> CanvasWidth =>
		RawTimelineVm.CanvasWidth;

	public IReadOnlyReactiveProperty<double> CanvasHeight =>
		RawTimelineVm.CanvasHeight;

	/// <seealso cref="WrapTimeLine.Items"/>
	public ImmutableList<IWrapBaseItem> Items
	{
		get
		{
			return Reflect.GetImmutableListProp<IWrapBaseItem>(
				RawTimelineVm,
				nameof(Items),
				factory: (Func<dynamic, IWrapBaseItem>)(
					item => ItemFactory.Create(item)
				)
			);
		}
		set
		{
			Reflect.SetImmutableListProp<IWrapBaseItem>(
				RawTimelineVm,
				nameof(Items),
				value
			);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public ReactiveProperty<dynamic> RawSelectedScene =>
		RawTimelineVm.SelectedScene;

	public ReactiveProperty<WrapSceneTitleViewModel> SelectedScene
	{
		get
		{
			_selectedScene =
				new ReactiveProperty<WrapSceneTitleViewModel>
				{
					Value = new WrapSceneTitleViewModel(
						RawTimelineVm.SelectedScene
					),
				};
			return _selectedScene;
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				RawTimelineVm?.Dispose();
				_selectedScene?.Dispose();
			}

			// TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
			// TODO: 大きなフィールドを null に設定します
			_disposedValue = true;
		}
	}

	// 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
	// ~WrapTimelineViewModel()
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
