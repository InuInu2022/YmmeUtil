namespace YmmeUtil.Bridge.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.FrameBufferItem
/// </summary>
public record WrapFrameBufferItem : WrapVisualItemBase
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.FrameBufferItem";

	public WrapFrameBufferItem(dynamic item)
		: base((object)item) { }
}
