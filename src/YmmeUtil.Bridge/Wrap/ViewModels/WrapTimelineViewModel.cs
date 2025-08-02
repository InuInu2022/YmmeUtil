using System.Collections.Immutable;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using Reactive.Bindings;
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
	ReactiveProperty<WrapSceneTitleViewModel?>? _selectedScene;
	private IDisposable? _selectedSceneSubscription;

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

	[System.Diagnostics.CodeAnalysis.SuppressMessage(
		"Usage",
		"SMA0040:Missing Using Statement",
		Justification = "<保留中>"
	)]
	public ReactiveProperty<WrapSceneTitleViewModel?> SelectedScene
	{
		get
		{
			if (_selectedScene is not null)
			{
				return _selectedScene;
			}

			// 初期値を作成 - nullを許可
			var initialValue = RawSelectedScene.Value
				is not null
				? new WrapSceneTitleViewModel(
					RawSelectedScene.Value
				)
				: null;

			_selectedScene =
				new ReactiveProperty<WrapSceneTitleViewModel?>(
					initialValue
				);

			// Subscriptionを保存して後でDisposeできるように
			_selectedSceneSubscription =
				RawSelectedScene.Subscribe(rawScene =>
				{
					// 既存の値を破棄してから新しい値を設定
					var oldValue = _selectedScene.Value;

					try
					{
						_selectedScene.Value = rawScene
							is not null
							? new WrapSceneTitleViewModel(
								rawScene
							)
							: null;
					}
					finally
					{
						// 古いインスタンスを必ず破棄
						oldValue?.Dispose();
					}
				});

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

				// _selectedScene.Valueも破棄する
				_selectedScene?.Value?.Dispose();
				_selectedScene?.Dispose();

				_selectedSceneSubscription?.Dispose();
				RawSelectedScene?.Dispose();
			}
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
