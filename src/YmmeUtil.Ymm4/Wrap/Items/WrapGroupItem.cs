using System.Collections.Immutable;
using YmmeUtil.Ymm4.Internal;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Plugin.Effects;
using ImpromptuInterface;

namespace YmmeUtil.Ymm4.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.GroupItem
/// </summary>
public record WrapGroupItem : WrapVisualItemBase
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.GroupItem";

	public WrapGroupItem(dynamic item)
		: base((object)item) { }

	public int CompositeCenter
	{
		get => EnumUtil.GetEnumValue(Item.CompositeCenter);
		set
		{
			var enumType = Item.CompositeCenter.GetType();
			Item.CompositeCenter = EnumUtil.SetEnumValue(enumType, value);
		}
	}

	public int GroupRange
	{
		get => Item.GroupRange;
		set => Item.GroupRange = value;
	}

	public IEnumerable<IVideoEffect> GroupEffects
	{
		get => Impromptu
			.ActLike<IEnumerable<IVideoEffect>>(Item.GroupEffects);
		set => Item.GroupEffects = value;
	}

	public bool IsCompressFrame
	{
		get => Item.IsCompressFrame;
		set => Item.IsCompressFrame = value;
	}

	public bool IsGroupOnly
	{
		get => Item.IsGroupOnly;
		set => Item.IsGroupOnly = value;
	}
}
