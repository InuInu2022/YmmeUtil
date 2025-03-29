using System.Reflection;

using YukkuriMovieMaker.Plugin;

namespace YmmeUtil.Sandbox;

[PluginDetails(AuthorName = "InuInu", ContentId = "")]
public class WithYmm4 : IPlugin
{
	public string Name { get; } = "Sandbox.WithYmm4";
	public PluginDetailsAttribute Details =>
		GetType().GetCustomAttribute<PluginDetailsAttribute>() ?? new();
}
