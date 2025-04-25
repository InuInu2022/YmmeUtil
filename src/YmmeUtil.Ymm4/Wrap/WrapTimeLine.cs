using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

using YmmeUtil.Ymm4.Internal;

namespace YmmeUtil.Ymm4.Wrap;

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
	public ImmutableList<WrapBaseItem> Items
	{
		get
		{
			return Reflect.GetImmutableListProp<WrapBaseItem>(
				_timeline,
				nameof(Items),
				factory: (Func<dynamic, WrapBaseItem>)(item => new WrapBaseItem(item))
			);
		}

		set
		{
			Reflect.SetImmutableListProp<WrapBaseItem>(_timeline, nameof(Items), value);
		}
	}
	public WrapBaseItem SelectedItem => new(_timeline.SelectedItem);
	public IEnumerable<WrapBaseItem> SelectedItems
	{
		get
		{
			var items = (IEnumerable<dynamic>)_timeline.SelectedItems;
			return items.Select(i => new WrapBaseItem(i));
		}
	}

	public IEnumerable<WrapBaseItem> SelectedAndGroupedItems
	{
		get
		{
			var items = (IEnumerable<WrapBaseItem>)_timeline.SelectedAndGroupedItems;
			return items.Select<dynamic, WrapBaseItem>(i => new WrapBaseItem(i));
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
}
