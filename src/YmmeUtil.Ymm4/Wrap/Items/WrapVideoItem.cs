namespace YmmeUtil.Ymm4.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.VideoItem
/// </summary>
public record WrapVideoItem : WrapVisualItemBase
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.VideoItem";
    public WrapVideoItem(dynamic item)
		: base((object)item) { }
}
