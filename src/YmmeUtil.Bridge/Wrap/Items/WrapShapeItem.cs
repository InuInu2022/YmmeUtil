namespace YmmeUtil.Bridge.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.ShapeItem
/// </summary>
public record WrapShapeItem : WrapVisualItemBase
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.ShapeItem";

	public WrapShapeItem(dynamic item)
		: base((object)item) { }
}
