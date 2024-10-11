using System.Reflection;

namespace YmmeUtil;

public static class AssemblyUtil
{

	/// <summary>
	/// ymme (YMM4 Plugin)のバージョンを取得
	/// </summary>
	/// <returns>ymme (YMM4 Plugin)のバージョン。Assembly自体の <see cref="Version"/> が指定されている必要があります。</returns>
	public static Version GetVersion(Type pluginType)
	{
		try
        {
            var pluginAssembly = pluginType.Assembly;
            var pluginVersion = pluginAssembly?.GetName()?.Version;

			return pluginVersion ?? new(0,0,0);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to get YMM4 plugin version: {ex.Message}");
			return new(0,0,0);
        }
	}

	/// <summary>
	/// ymme (YMM4 Plugin)のバージョンを文字列で取得
	/// </summary>
	/// <returns></returns>
    public static string GetVersionString(Type pluginType)
		=> GetVersion(pluginType).ToString();
}