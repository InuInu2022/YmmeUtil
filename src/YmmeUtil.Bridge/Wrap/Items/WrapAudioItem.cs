namespace YmmeUtil.Bridge.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.AudioItem
/// </summary>
public record WrapAudioItem : WrapBaseItem
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.AudioItem";

	public WrapAudioItem(dynamic item)
		: base((object)item) { }
}
