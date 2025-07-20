using System.Collections.Immutable;
using ImpromptuInterface;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Audio;
using YukkuriMovieMaker.Plugin.Effects;

namespace YmmeUtil.Bridge.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.VideoItem
/// </summary>
public record WrapVideoItem : WrapVisualItemBase
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.VideoItem";

	public WrapVideoItem(dynamic item)
		: base((object)item) { }

	public ImmutableList<IAudioEffect> AudioEffects
	{
		get => Impromptu.ActLike<ImmutableList<IAudioEffect>>(Item.AudioEffects);
		set => Item.AudioEffects = value;
	}

	public int AudioTrackIndex
	{
		get => Item.AudioTrackIndex;
		set => Item.AudioTrackIndex = value;
	}

	public double EchoAttenuation
	{
		get => Item.EchoAttenuation;
		set => Item.EchoAttenuation = value;
	}

	public double EchoInterval
	{
		get => Item.EchoInterval;
		set => Item.EchoInterval = value;
	}

	public bool EchoIsEnabled
	{
		get => Item.EchoIsEnabled;
		set => Item.EchoIsEnabled = value;
	}

	public string FilePath
	{
		get => Item.FilePath;
		set => Item.FilePath = value;
	}

	public bool IsLooped
	{
		get => Item.IsLooped;
		set => Item.IsLooped = value;
	}

	public bool IsWaveformEnabled
	{
		get => Item.IsWaveformEnabled;
		set => Item.IsWaveformEnabled = value;
	}

	public Animation Pan => Item.Pan;

	public Animation Volume => Item.Volume;

	public AudioStreamBase CreateAudioSource(WrapScene scene)
	{
		return Item.CreateAudioSource(scene);
	}
}
