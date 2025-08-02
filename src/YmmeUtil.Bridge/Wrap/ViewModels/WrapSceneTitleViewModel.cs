using System.ComponentModel;
using Reactive.Bindings;
using YmmeUtil.Bridge.Wrap;
using YukkuriMovieMaker.Commons;

namespace YmmeUtil.Bridge.Wrap.ViewModels;

public partial class WrapSceneTitleViewModel
	: Bindable,
		IDisposable
{
	bool _disposedValue;

	public WrapSceneTitleViewModel(
		dynamic sceneTitleViewModel
	)
	{
		RawSceneTitleViewModel = sceneTitleViewModel;
	}

	/// <summary>
	/// アイテムの動的オブジェクト。
	/// <para>YMM4本体の実装に依存するため、基本的には直接使用しないでください。</para>
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public dynamic RawSceneTitleViewModel
	{
		get;
		private set;
	}

	public ReactiveProperty<string> Title =>
		RawSceneTitleViewModel.Title;
	public int Index
	{
		get => RawSceneTitleViewModel.Index;
		set => RawSceneTitleViewModel.Index = value;
	}

	public bool IsSelected
	{
		get => RawSceneTitleViewModel.IsSelected;
		set => RawSceneTitleViewModel.IsSelected = value;
	}

	public WrapTimeLine Timeline =>
		new(RawSceneTitleViewModel.Timeline);

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				RawSceneTitleViewModel?.Dispose();
			}

			// TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
			// TODO: 大きなフィールドを null に設定します
			_disposedValue = true;
		}
	}

	public void Dispose()
	{
		// このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
