using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using YukkuriMovieMaker.Commons;

namespace YmmeUtil.Bridge.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.TextItem
/// </summary>
public record WrapTextItem : WrapVisualItemBase
{
	public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.TextItem";

	public WrapTextItem(dynamic item)
		: base((object)item) { }

	public WrapTextItem()
		: base()
	{
		Debug.WriteLine("WrapTextItem() called.");
	}

	public required string Text
	{
		get => Item.Text;
		set => Item.Text = value;
	}

	public string Font
	{
		get => Item.Font;
		set => Item.Font = value;
	}
	public Color FontColor
	{
		get => Item.FontColor;
		set => Item.FontColor = value;
	}
	public Animation FontSize
	{
		get => Item.FontSize;
	}

	public bool Bold
	{
		get => Item.Bold;
		set => Item.Bold = value;
	}
	public bool Italic
	{
		get => Item.Italic;
		set => Item.Italic = value;
	}
	public bool IsDividedPerCharacter
	{
		//fixed spell
		get => Item.IsDevidedPerCharacter;
		set => Item.IsDevidedPerCharacter = value;
	}

	[Obsolete("Use IsDividedPerCharacter instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool IsDevidedPerCharacter
	{
		get => IsDividedPerCharacter;
		set => IsDividedPerCharacter = value;
	}

	public bool IsTrimEndSpace
	{
		get => Item.IsTrimEndSpace;
		set => Item.IsTrimEndSpace = value;
	}

	public ImmutableList<TextDecoration> Decorations
	{
		get => Item.Decorations;
		set => Item.Decorations = value;
	}
	public double DisplayInterval
	{
		get => Item.DisplayInterval;
		set => Item.DisplayInterval = value;
	}
}
