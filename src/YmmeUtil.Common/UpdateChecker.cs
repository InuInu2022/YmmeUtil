using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using GithubReleaseDownloader;
using GithubReleaseDownloader.Entities;

using Mayerch1.GithubUpdateCheck;

namespace YmmeUtil.Common;

public sealed class UpdateChecker
{
	private readonly GithubUpdateCheck update;
	private readonly string username;
	private readonly string repository;
	private string? repoVersion;

	[SuppressMessage("", "S1450")]
	private Release? release;

	private UpdateChecker(string username, string repository)
	{
		update = new GithubUpdateCheck(username, repository, CompareType.Boolean);
		this.username = username;
		this.repository = repository;
	}

	/// <summary>
	/// UpdateChecker
	/// </summary>
	/// <param name="username">githubのusername</param>
	/// <param name="repository">githubのrepository name</param>
	/// <returns></returns>
	public static UpdateChecker Build(
		string username = "InuInu2022",
		string repository = "YmmeUtil"
	)
	{
		return new UpdateChecker(username, repository);
	}

	public async ValueTask<string> GetRepositoryVersionAsync(bool useCache = false)
	{
		if (useCache && repoVersion is not null)
		{
			return repoVersion;
		}

		release = await ReleaseManager
			.Instance.GetLatestAsync(username, repository)
			.ConfigureAwait(false);

		repoVersion = release?.TagName ?? "v0.0.0";
		return repoVersion;
	}

	[SuppressMessage("Correctness", "SS034", Justification = "<保留中>")]
	[SuppressMessage("Usage", "VSTHRD002", Justification = "<保留中>")]
	[SuppressMessage("Info Code Smell", "S1133", Justification = "<保留中>")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete($"Use {nameof(GetRepositoryVersionAsync)}", false)]
	internal string GetRepositoryVersion(bool useCache = false)
	{
		return GetRepositoryVersionAsync(useCache).AsTask().Result;
	}

	/// <summary>
	/// ローカルバージョンがリポジトリの最新バージョンと比較して
	/// アップデートが利用可能かどうかを非同期で確認します。
	/// </summary>
	/// <returns>アップデートが利用可能の場合はtrue、利用不可の場合はfalse。</returns>
	public Task<bool> IsAvailableAsync(Type plugin)
	{
		var v = "v" + AssemblyUtil.GetVersionString(plugin);
		return update.IsUpdateAvailableAsync(v, VersionChange: VersionChange.Build);
	}

	[SuppressMessage("Correctness", "SS034", Justification = "<保留中>")]
	[SuppressMessage("Usage", "VSTHRD002", Justification = "<保留中>")]
	[SuppressMessage("Info Code Smell", "S1133", Justification = "<保留中>")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete($"Use {nameof(IsAvailableAsync)}", false)]
	internal bool IsAvailable(Type plugin)
	{
		return IsAvailableAsync(plugin).Result;
	}

	/// <summary>
	/// 最新のgithub releaseからプラグインアセットを取得する
	/// </summary>
	/// <param name="fileName">DLしたい添付プラグインファイルの名前。一部でもOK</param>
	/// <param name="fallbackUrl">取得に失敗したときに返すURL。</param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public async ValueTask<string> GetDownloadUrlAsync(string fileName, [Url] string fallbackUrl)
	{
		var urlValidator = new UrlAttribute();
		if (!urlValidator.IsValid(fallbackUrl))
		{
			throw new ArgumentException($"Invalid url: {fallbackUrl}", fallbackUrl);
		}

		try
		{
			release = await ReleaseManager
				.Instance.GetLatestAsync(username, repository)
				.ConfigureAwait(false);

			return release
				?.Assets?.FirstOrDefault(a =>
					a.Name.Contains(fileName, StringComparison.OrdinalIgnoreCase)
				)
				?.DownloadUrl ?? fallbackUrl;
		}
		catch (System.Exception)
		{
			return fallbackUrl;
		}
	}

	[SuppressMessage("Correctness", "SS034", Justification = "<保留中>")]
	[SuppressMessage("Usage", "VSTHRD002", Justification = "<保留中>")]
	[SuppressMessage("Info Code Smell", "S1133", Justification = "<保留中>")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete($"Use {nameof(GetDownloadUrlAsync)}", false)]
	internal string GetDownloadUrl(string fileName, [Url] string fallbackUrl)
	{
		return GetDownloadUrlAsync(fileName, fallbackUrl).AsTask().Result;
	}
}
