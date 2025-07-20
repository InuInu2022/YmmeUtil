namespace YmmeUtil.Bridge.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.VoiceItem
/// </summary>
public record WrapVoiceItem : WrapBaseItem
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.VoiceItem";

	public WrapVoiceItem(dynamic item)
		: base((object)item) { }
}
