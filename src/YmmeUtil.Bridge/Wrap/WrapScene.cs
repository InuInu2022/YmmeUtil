using System.Diagnostics.CodeAnalysis;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Audio.Effects;
using YukkuriMovieMaker.Player.Video;

namespace YmmeUtil.Bridge.Wrap;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Scene
/// </summary>
public class WrapScene : ISceneInfo
{
	dynamic RawScene { get; init; }

	public WrapScene(dynamic scene)
	{
		RawScene = scene;
	}

	public Guid ID => RawScene.ID;
	public string Name => RawScene.Name;

	public WrapTimeLine Timeline => new(RawScene.Timeline);

	public Guid[] ParentScenes => RawScene.ParentScenes;

	public WrapScenes Scenes => new(RawScene.Scenes);

	public IEnumerable<WrapScene> AllScenesWithParentScenesInfo =>
		RawScene.AllScenesWithParentScenesInfo.Select(
			(Func<dynamic, WrapScene>)(x => new WrapScene(x))
		);
	public int Width => RawScene.Width;
	public int Height => RawScene.Height;
	public int FPS => RawScene.FPS;
	public int AudioSamplingRate => RawScene.AudioSamplingRate;
	public YukkuriMovieMaker.Player.Video.FrameTime Duration => RawScene.Duration;

	public bool CanCreateSubScene() => RawScene.CanCreateSubScene();

	public WrapScene CreateNest()
	{
		return new(RawScene.CreateNest());
	}

	public bool TryCreateAudioSource(out IAudioStream? source) =>
		RawScene.TryCreateAudioSource(out source);

	public bool TryCreateVideoSource(out ITimelineSourceAndDevices? source) =>
		RawScene.TryCreateVideoSource(out source);

	public bool TryCreateVideoSource(
		IGraphicsDevicesAndContext devices,
		[NotNullWhen(true)] out ITimelineSource? source
	) => RawScene.TryCreateVideoSource(devices, out source);
}
