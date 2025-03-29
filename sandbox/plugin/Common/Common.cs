using System.Reflection;

using YukkuriMovieMaker.Plugin;

namespace YmmeUtil.Sandbox;

[PluginDetails(AuthorName = "InuInu", ContentId = "")]
public class Common : IPlugin
{
	public string Name { get; } = "Sandbox.Common";
	public PluginDetailsAttribute Details =>
		GetType().GetCustomAttribute<PluginDetailsAttribute>() ?? new();
}