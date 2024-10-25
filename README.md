# YmmeUtil
YMM4 Plugin utilities library

## これはなに

YMM4のプラグイン（`ymme`等）を開発するときに便利なユーティリティライブラリです。

## 注意点

- これ自体はYMM4のプラグインではありません。
- ただの.NETのクラスライブラリです。
- YMM4のプラグインを開発するときに参照して使う用です。

## 使い方

複数のプロジェクトがあるので参照して使ってください。

### `YmmeUtil.Common`

YMM4の実装に依存しない便利クラスが入っています。

- `AssemblyUtil.GetVersion()` / `GetVersionString()`
  - プラグインのクラスのType型を渡すことでプラグインのVersionを取得できます
    - AssemblyInfoのVersionです
  - MinVerなどのsem ver.ライブラリを使う前提です
- `UpdateChecker`
  - Githubのreleasesで配布されているYMM4プラグインの更新確認やDL URLの取得を行うクラス
  - 詳しくは `tests/UpdateCheckerTest.cs` をご覧ください！

### `YmmeUtil.Ymm4`

YMM4本体に依存する便利クラスが入っています。

- `TaskbarUtil`
  - タスクバーに進捗を表示するための処理が入っています
    - プラグインの処理で時間が掛かる時など用
  - `GetMainTaskbarInfo()`
    - YMM4のメインウィンドウの`TaskbarInfo`を取得（ない場合は生成）
  - `StartIndeterminate()` / `PauseIndeterminate()` / `FinishIndeterminate()`
    - トータルの進捗が分からない時用の進捗表示
  - `ShowError()`
    - エラー表示
  - `ShowNormal()`
    - 通常表示
  - `ShowProgress()` / `FinishProgress()`
    - 進捗％表示

## ライブラリ

- [GithubReleaseDownloader](https://github.com/russkyc/github-release-downloader/blob/master/LICENSE)
  - MIT License
  - Copyright (c) 2023 Russell Camo (@russkyc)
- [Mayerch1.GithubUpdateCheck](https://github.com/Mayerch1/GithubUpdateCheck/blob/master/LICENSE)
  - MIT License
  - Copyright (c) 2020 Christian Mayer
