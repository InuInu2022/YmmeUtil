using System.Reflection;

using YmmeUtil.Sandbox.View;

using YukkuriMovieMaker.Plugin;

namespace YmmeUtil.Sandbox;

[PluginDetails(AuthorName = "InuInu", ContentId = "")]
public class WithYmm4 : IToolPlugin
{
	public string Name { get; } = "Sandbox.WithYmm4";
	public PluginDetailsAttribute Details =>
		GetType().GetCustomAttribute<PluginDetailsAttribute>() ?? new();

	public Type ViewModelType { get; } = typeof(MainViewModel);
	public Type ViewType { get; } = typeof(MainView);
}
