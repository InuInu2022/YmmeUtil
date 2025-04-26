namespace YmmeUtil.Ymm4.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.TransitionItem
/// </summary>
public record WrapTransitionItem : WrapBaseItem
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.TransitionItem";
    public WrapTransitionItem(dynamic item)
		: base((object)item) { }
}
