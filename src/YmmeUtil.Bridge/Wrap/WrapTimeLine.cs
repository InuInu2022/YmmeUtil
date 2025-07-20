using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Dynamitey;
using YmmeUtil.Bridge.Internal;
using YmmeUtil.Bridge.Wrap.Items;
using YukkuriMovieMaker.Plugin.Effects;

namespace YmmeUtil.Bridge.Wrap;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.TimeLine
/// </summary>
public partial record WrapTimeLine
{
	/// <summary>
	/// タイムラインの動的オブジェクト。
	/// <para>YMM4本体の実装に依存するため、基本的には直接使用しないでください。</para>
	/// </summary>
	public dynamic RawTimeline => _timeline;

	// 以下はYMM4本体側の実装が変わってもいいように
	// ラップしたプロパティ

	public Guid Id => _timeline.ID;
	public string Name
	{
		get => _timeline.Name;
		set => _timeline.Name = value;
	}

	[Range(0, int.MaxValue)]
	public int CurrentFrame
	{
		get => _timeline.CurrentFrame;
		set => _timeline.CurrentFrame = value;
	}
	public int Length => _timeline.Length;
	public int MaxLayer => _timeline.MaxLayer;

	#region Item
	//--------------------------------------------------------+
	public ImmutableList<IWrapBaseItem> Items
	{
		get
		{
			return Reflect.GetImmutableListProp<IWrapBaseItem>(
				_timeline,
				nameof(Items),
				factory: (Func<dynamic, IWrapBaseItem>)(item => ItemFactory.Create(item))
			);
		}
		set { Reflect.SetImmutableListProp<IWrapBaseItem>(_timeline, nameof(Items), value); }
	}

	public IWrapBaseItem SelectedItem => ItemFactory.Create(_timeline.SelectedItem);

	public IEnumerable<IWrapBaseItem> SelectedItems
	{
		get
		{
			return Reflect.GetImmutableListProp<IWrapBaseItem>(
				_timeline,
				nameof(SelectedItems),
				factory: (Func<dynamic, IWrapBaseItem>)(item => ItemFactory.Create(item))
			);
		}
	}

	public IEnumerable<IWrapBaseItem> SelectedAndGroupedItems
	{
		get
		{
			var items = (IEnumerable<dynamic>)_timeline.SelectedAndGroupedItems;
			return items.Select<dynamic, IWrapBaseItem>(i => ItemFactory.Create(i));
		}
		set => _timeline.SelectedAndGroupedItems = value;
	}
	//--------------------------------------------------------+
	#endregion

	public YukkuriMovieMaker.Project.VideoInfo VideoInfo => _timeline.VideoInfo;

	readonly dynamic _timeline;

	/// <summary>
	/// タイムラインのラッパーオブジェクトを初期化します。
	/// </summary>
	/// <param name="timeline">タイムラインの動的オブジェクト。<see cref="TimelineUtil.TryGetTimeline(out WrapTimeLine?)"/>を使ってください。</param>
	/// <seealso cref="TimelineUtil.TryGetTimeline(out WrapTimeLine?)"/>
	public WrapTimeLine(dynamic timeline)
	{
		_timeline = timeline;
	}

	public void DeleteItems(IEnumerable<WrapTimeLine> items)
	{
		//_timeline.DeleteItems(
		//	_timeline.ConvertToIItem(items));
		Dynamic.InvokeMemberAction(_timeline, "DeleteItems", _timeline.ConvertToIItem(items));
	}

	public void PasteAudioEffects()
	{
		_timeline.PasteAudioEffects();
	}

	public void PasteAudioEffects(IEnumerable<IAudioEffect> effects)
	{
		_timeline.PasteAudioEffects(effects);
	}
}
