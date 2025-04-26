using System;
using System.Globalization;
using YukkuriMovieMaker.Project;

namespace YmmeUtil.Ymm4.Internal;

public static class EnumUtil
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "SMA0025:Enum System Method")]
	public static bool IsBlendValueDefined(int blendValue)
	{
		return Enum.IsDefined(typeof(Blend), blendValue);
	}

	public static int GetEnumValue(object enumValue)
	{
		return Convert.ToInt32(enumValue, CultureInfo.InvariantCulture);
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "SMA0025:Enum System Method")]
	public static object SetEnumValue(Type enumType, int value)
	{
		return Enum.ToObject(enumType, value);
	}
}
