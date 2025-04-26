using System.Collections.Immutable;

using YmmeUtil.Ymm4.Internal;

using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Plugin.Effects;
using YukkuriMovieMaker.Project;

namespace YmmeUtil.Ymm4.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.VisualItem
/// </summary>
public abstract partial record WrapVisualItemBase : WrapBaseItem
{
	public override string RawItemTypeName
		=> "YukkuriMovieMaker.Project.Items.VisualItem";
	protected WrapVisualItemBase(dynamic item)
		: base((object)item) { }

	protected WrapVisualItemBase()
		: base()
	{
		// Constructor body
	}

	public Animation X { get; set; } = new(0.0, -99999.0, 99999.0);
	public Animation Y { get; set; } = new(0.0, -99999.0, 99999.0);
	public Animation Z { get; set; } = new(0.0, -99999.0, 99999.0);

	public Animation Opacity { get; set; } = new(100.0, 0.0, 100.0);
	public Animation Zoom { get; set; } = new(100.0, 0.0, 5000.0);
	public Animation Rotation { get; set; } = new(0.0, -36000.0, 36000.0, 360.0);

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "SMA0020:Unchecked Cast to Enum Type", Justification = "<保留中>")]
	public Blend Blend
	{
		get
		{
			int blendValue = Convert.ToInt32(Item.Blend);
			if (EnumUtil.IsBlendValueDefined(blendValue))
			{
				return (Blend)blendValue;
			}
			return Blend.Normal; // デフォルト値を返す
		}
		set => Item.Blend = value;
	}

	public bool IsInverted
	{
		get => Item.IsInverted;
		set => Item.IsInverted = value;
	}

	public bool IsAlwaysOnTop
	{
		get => Item.IsAlwaysOnTop;
		set => Item.IsAlwaysOnTop = value;
	}

	public bool IsZOrderEnabled
	{
		get => Item.IsZOrderEnabled;
		set => Item.IsZOrderEnabled = value;
	}

	public bool IsClippingWithObjectAbove
	{
		get => Item.IsClippingWithObjectAbove;
		set => Item.IsClippingWithObjectAbove = value;
	}

	public double FadeIn
	{
		get => Item.FadeIn;
		set => Item.FadeIn = value;
	}

	public double FadeOut
	{
		get => Item.FadeOut;
		set => Item.FadeOut = value;
	}

	public ImmutableList<IVideoEffect> VideoEffects
	{
		get => Reflect.GetImmutableListProp<IVideoEffect>(
			Item,
			nameof(VideoEffects),
			factory: (Func<dynamic, IVideoEffect>)(f => (IVideoEffect)f)
		);
		set
		{
			Reflect.SetImmutableListProp<IVideoEffect>(
				Item,
				nameof(VideoEffects),
				value
			);
		}
	}
}
