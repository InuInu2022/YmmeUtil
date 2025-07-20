using System.Collections.Immutable;
using YmmeUtil.Bridge.Internal;
using YukkuriMovieMaker.Project;
using YukkuriMovieMaker.UndoRedo;

namespace YmmeUtil.Bridge.Wrap;

public partial class WrapScenes : UndoRedoable
{
	public dynamic RawScenes { get; init; }

	public WrapScenes(dynamic scenes)
	{
		RawScenes = scenes;
	}

	public UndoRedoManager UndoRedoManager => RawScenes.UndoRedoManager;

	public IEnumerable<WrapScene> AllScenes =>
		Reflect.GetImmutableListProp<WrapScene>(
			RawScenes,
			nameof(AllScenes),
			factory: (Func<dynamic, WrapScene>)(item => new WrapScene(item))
		);

	public ImmutableList<WrapTimeLine> Timelines
	{
		get
		{
			return Reflect.GetImmutableListProp<WrapTimeLine>(
				RawScenes,
				nameof(Timelines),
				factory: (Func<dynamic, WrapTimeLine>)(item => new WrapTimeLine(item))
			);
		}
		set { Reflect.SetImmutableListProp<WrapTimeLine>(RawScenes, nameof(Timelines), value); }
	}

	public void AddScene(WrapTimeLine wrapTimeline)
	{
		RawScenes.AddScene(wrapTimeline.RawTimeline);
	}

	public void CreateScene(int index, VideoInfo info, TimelineVerticalLine verticalLine)
	{
		RawScenes.CreateScene(index, info, verticalLine);
	}

	public void CreateScene(
		string name,
		int index,
		VideoInfo info,
		TimelineVerticalLine verticalLine
	)
	{
		RawScenes.CreateScene(name, index, info, verticalLine);
	}

	public void ClearScenes()
	{
		RawScenes.ClearScenes();
	}

	public void DeleteScene(int index)
	{
		RawScenes.DeleteScene(index);
	}
}
