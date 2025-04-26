namespace YmmeUtil.Ymm4.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.ImageItem
/// </summary>
public record WrapImageItem : WrapVisualItemBase
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.ImageItem";
    public WrapImageItem(dynamic item)
		: base((object)item) { }
}
