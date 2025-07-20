namespace YmmeUtil.Bridge.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.TachieItem
/// </summary>
public record WrapTachieItem : WrapVisualItemBase
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.TachieItem";

	public WrapTachieItem(dynamic item)
		: base((object)item) { }
}
