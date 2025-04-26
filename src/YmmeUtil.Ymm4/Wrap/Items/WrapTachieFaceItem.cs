namespace YmmeUtil.Ymm4.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.TachieFaceItem
/// </summary>
public record WrapTachieFaceItem : WrapBaseItem
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.TachieFaceItem";
    public WrapTachieFaceItem(dynamic item)
		: base((object)item) { }
}
