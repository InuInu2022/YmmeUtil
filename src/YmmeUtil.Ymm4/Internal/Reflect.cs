using System.Reflection;

namespace YmmeUtil.Ymm4.Internal;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011")]
internal static class Reflect
{
	/// <summary>
	/// GetProp from ViewModel class
	/// </summary>
	/// <param name="vm">ViewModel class</param>
	/// <param name="propName">Property name of ViewModel class</param>
	/// <param name="isPrivate">Indicates if the property is private</param>
	/// <returns></returns>
	/// <seealso cref="GetProp{T}(dynamic, string, bool)"/>
	internal static dynamic? GetProp(dynamic vm, string propName, bool isPrivate = false)
	{
		Type vmType = vm.GetType();

		var propertyInfo = vmType.GetProperty(
			propName,
			bindingAttr: isPrivate
				? BindingFlags.NonPublic | BindingFlags.Instance
				: BindingFlags.Public | BindingFlags.Instance
		);

		return propertyInfo?.GetValue(vm);
	}

	/// <inheritdoc cref="GetProp(dynamic, string, bool)"/>
	internal static T GetProp<T>(dynamic vm, string propName, bool isPrivate = false)
	{
		return (T)GetProp(vm, propName, isPrivate);
	}

	/// <summary>
	/// GetField from ViewModel class
	/// </summary>
	/// <param name="vm">ViewModel class</param>
	/// <param name="fieldName">Field name of ViewModel class</param>
	/// <param name="isPrivate">Indicates if the field is private</param>
	/// <returns></returns>
	/// <seealso cref="GetField{T}(dynamic, string, bool)"/>
	internal static dynamic? GetField(dynamic vm, string fieldName, bool isPrivate = false)
	{
		Type vmType = vm.GetType();
		var fieldInfo = vmType.GetField(
			fieldName,
			bindingAttr: isPrivate
				? BindingFlags.NonPublic | BindingFlags.Instance
				: BindingFlags.Public | BindingFlags.Instance
		);

		return fieldInfo?.GetValue(vm);
	}

	/// <inheritdoc cref="GetField(dynamic, string, bool)"/>
	internal static T GetField<T>(dynamic vm, string fieldName, bool isPrivate = false)
	{
		return (T)GetField(vm, fieldName, isPrivate);
	}
}
