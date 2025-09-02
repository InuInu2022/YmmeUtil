using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dynamitey;
using YmmeUtil.Bridge.Internal;
using YmmeUtil.Bridge.Wrap.Items;
using YukkuriMovieMaker.Plugin.Effects;
using YukkuriMovieMaker.Project;

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
	[EditorBrowsable(EditorBrowsableState.Advanced)]
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

	/* TODO
	public LayerSettings LayerSettings => _timeline.LayerSettings;
	public LayerSelection LayerSelection => _timeline.LayerSelection;
	*/

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

	public IWrapBaseItem SelectedItem
	{
		get { return ItemFactory.Create(_timeline.SelectedItem); }
		set { _timeline.SelectedItem = value; }
	}

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
		set
		{
			Reflect.SetImmutableListProp<IWrapBaseItem>(_timeline, nameof(SelectedItems), value);
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

	public TimelineVerticalLine VerticalLine => _timeline.VerticalLine;
	public VideoInfo VideoInfo => _timeline.VideoInfo;

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

	public void SelectItems(IEnumerable<IWrapBaseItem> items)
	{
		var rawItems = items
			.Select(i => i.RawItem)
			.Where(item => item is not null)
			.Select(item => item is WrapBaseItem wrapItem ? wrapItem.RawItem : item)
			.OfType<dynamic>()
			.ToArray();
		Dynamic.InvokeMemberAction(_timeline, "SelectItems", rawItems);
	}

	public void DeleteItems(IEnumerable<WrapTimeLine> items)
	{
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
