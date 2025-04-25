namespace YmmeUtil.Ymm4.Wrap;
public record WrapTextItem : WrapBaseItem
{
	protected WrapTextItem(WrapBaseItem original) : base(original)
	{
	}

	public string Text { get; set; } = string.Empty;

}