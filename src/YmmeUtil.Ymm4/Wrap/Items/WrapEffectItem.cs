namespace YmmeUtil.Ymm4.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.EffectItem
/// </summary>
public record WrapEffectItem : WrapVisualItemBase
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.EffectItem";
    public WrapEffectItem(dynamic item)
		: base((object)item) { }
}
