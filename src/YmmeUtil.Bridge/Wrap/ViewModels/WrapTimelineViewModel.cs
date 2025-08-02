using System.Collections.Immutable;
using System.ComponentModel;
using System.Reactive.Linq;

using Reactive.Bindings;

using YmmeUtil.Bridge.Internal;
using YmmeUtil.Bridge.Wrap.Items;

using YukkuriMovieMaker.Commons;

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
		dynamic timelineViewModelValue
	)
	{
		RawTimelineVm = timelineViewModelValue;
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
	public dynamic RawSelectedScene
	{
		get
		{
			var selectedScene = Internal.Reflect.GetProp(
				RawTimelineVm,
				"SelectedScene"
			);
			return selectedScene
				?? throw new InvalidOperationException(
					"SelectedScene property not found"
				);
		}
	}

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
			var rawSelectedSceneValue =
				Internal.Reflect.GetProp(
					RawSelectedScene,
					"Value"
				);
			var initialValue = rawSelectedSceneValue
				is not null
				? new WrapSceneTitleViewModel(
					rawSelectedSceneValue
				)
				: null;

			// ReactivePropertyを初期化
			_selectedScene =
				new ReactiveProperty<WrapSceneTitleViewModel?>(
					initialValue
				);

			// Subscriptionを保存して後でDisposeできるように
			_selectedSceneSubscription = Observable
				.FromEventPattern<
					PropertyChangedEventHandler,
					PropertyChangedEventArgs
				>(
					h =>
						RawSelectedScene.PropertyChanged +=
							h,
					h =>
						RawSelectedScene.PropertyChanged -=
							h
				)
				.Where(x =>
					x.EventArgs.PropertyName
					== nameof(
						ReactiveProperty<object>.Value
					)
				)
				.Subscribe(_ =>
				{
					// 変更処理
					var oldValue = _selectedScene?.Value;
					try
					{
						if (_selectedScene is not null)
						{
							var newRawValue =
								Internal.Reflect.GetProp(
									RawSelectedScene,
									"Value"
								);
							_selectedScene.Value =
								newRawValue is not null
									? new WrapSceneTitleViewModel(
										newRawValue
									)
									: null;
						}
					}
					finally
					{
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
