using Xunit.Abstractions;

using YmmeUtil.Common;

namespace tests;

public class UpdateCheckerTest(ITestOutputHelper output)
{
	private readonly ITestOutputHelper _output = output;
	[Fact]
	public async void SimpleCheck()
	{
		//チェックするgithubリポジトリ
		var checker = UpdateChecker
			.Build("InuInu2022", "YmmeUtil");

		// ローカルバージョンがリポジトリの最新バージョンと比較して
		// アップデートが利用可能かどうかを非同期で確認します。
		var isAvailable = await checker.IsAvailableAsync(
			//your plugin class here
			typeof(UpdateChecker)
		);
		var msg = isAvailable ? "available" : "not available";
		_output.WriteLine($"Update {msg}");

		//githubリポジトリのreleasesの最新のtagをチェック
		//※privateリポジトリでは使えません（常にv0.0.0になります）
		var repoVer = await checker.GetRepositoryVersionAsync();
		_output.WriteLine($"repo: {repoVer}");

		//取得URLをゲットする
		var url = await checker.GetDownloadUrlAsync(
			//取得するファイル名（一部でもOK） ※実はプラグインファイル以外もOK
			"YourAwesomePlugin.ymme",
			//ダウンロードURLの取得に失敗したときに返すURL
			//とりあえずreleasesのURLにしておけば手動でDLしてもらえる
			"https://github.com/InuInu2022/YmmeUtil/releases"
		);
		_output.WriteLine($"dl url: {url}");

	}
}