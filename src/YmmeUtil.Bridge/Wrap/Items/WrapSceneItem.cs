namespace YmmeUtil.Bridge.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.SceneItem
/// </summary>
public record WrapSceneItem : WrapVisualItemBase
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.SceneItem";

	public WrapSceneItem(dynamic item)
		: base((object)item) { }
}
