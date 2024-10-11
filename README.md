# YmmeUtil
YMM4 Plugin utilities library

## これはなに

YMM4のプラグイン（`ymme`等）を開発するときに便利なユーティリティライブラリです。

## 注意点

- これ自体はYMM4のプラグインではありません。
- ただの.NETのクラスライブラリです。
- YMM4のプラグインを開発するときに参照して使う用です。

## 使い方

### `YmmeUtil`

- `AssemblyUtil.GetVersion()` / `GetVersionString()`
  - プラグインのクラスのType型を渡すことでプラグインのVersionを取得できます
    - AssemblyInfoのVersionです
  - MinVerなどのsem ver.ライブラリを使う前提です
- `UpdateChecker`
  - Githubのreleasesで配布されているYMM4プラグインの更新確認やDL URLの取得を行うクラス
  - 詳しくは `tests/UpdateCheckerTest.cs` をご覧ください！

## ライブラリ

- GithubReleaseDownloader
  - MIT License
  - Copyright (c) 2023 Russell Camo (@russkyc)
- Mayerch1.GithubUpdateCheck
  - MIT License
  - Copyright (c) 2020 Christian Mayer
