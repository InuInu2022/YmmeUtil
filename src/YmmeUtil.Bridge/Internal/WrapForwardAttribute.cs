namespace YmmeUtil.Bridge.Internal;

[AttributeUsage(AttributeTargets.Property)]
public class WrapForwardAttribute : Attribute
{
	public string FieldName { get; }

	public WrapForwardAttribute()
	{
		FieldName = "this";
	}

	public WrapForwardAttribute(string fieldName)
	{
		FieldName = fieldName;
	}
}
