
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace YmmeUtil.Script;

/// <summary>
/// Script interface for UpdateChecker
/// </summary>
[SuppressMessage("Correctness", "SS034", Justification = "<保留中>")]
[SuppressMessage("Usage", "VSTHRD002", Justification = "<保留中>")]
[SuppressMessage("Info Code Smell", "S1133", Justification = "<保留中>")]
public sealed class UpdateChecker
{
	private readonly Common.UpdateChecker _updater;

	private UpdateChecker(
		string username,
		string repository
	)
	{
		_updater = Common.UpdateChecker
			.Build(username, repository);
	}

	/// <summary>
	/// 対象のgithubリポジトリを指定
	/// </summary>
	/// <remarks>
	/// 未指定の場合はYmmeUtilが指定されます
	/// </remarks>
	/// <param name="username"></param>
	/// <param name="repository"></param>
	/// <returns></returns>
	public static UpdateChecker Build(
		string username = "InuInu2022",
		string repository = "YmmeUtil"
	)
	{
		return new UpdateChecker(username, repository);
	}

	/// <inheritdoc cref="YmmeUtil.Common.UpdateChecker.GetRepositoryVersionAsync(bool)"/>
	public string GetRepositoryVersion(bool useCache = false)
	{
		return _updater
			.GetRepositoryVersionAsync(useCache)
			.AsTask().Result;
	}

	/// <inheritdoc cref="YmmeUtil.Common.UpdateChecker.IsAvailableAsync(Type)"/>
	public bool IsAvailable(Type plugin)
	{
		return _updater.IsAvailableAsync(plugin).Result;
	}

	/// <inheritdoc cref="YmmeUtil.Common.UpdateChecker.GetDownloadUrlAsync(string, string)"/>
	public string GetDownloadUrl(string fileName, [Url] string fallbackUrl)
	{
		return _updater
			.GetDownloadUrlAsync(fileName, fallbackUrl)
			.AsTask().Result;
	}
}
