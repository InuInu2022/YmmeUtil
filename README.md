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

### バージョン不一致の解決

複数のプラグインから異なるバージョンのYmmeUtilを参照すると、
最初にYmm4本体に読み込まれたDllしか使用されず、正しく機能しない問題があります。
（.NETの仕様）

[ILRepack](https://www.nuget.org/packages/ILRepack.Lib.MSBuild.Task/) を利用してYmmeUtilのDllをプラグインDLLに埋め込むことで回避できる可能性があります。
詳しくは sandbox/plugin/ObjectListTest の `ILRepack.targets` を見てください。

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
    - ![indeterminate](./docs/taskbar_indeterminate.gif)
  - `ShowError()`
    - エラー表示
  - `ShowNormal()`
    - 通常表示
  - `ShowProgress()` / `FinishProgress()`
    - 進捗％表示
- `WindowUtil`
  - `GetYmmMainWindow()`
    - YMM4のフォーカスされたメインウィンドウを一つ、取得します
  - `GetYmmMainWindows()`
    - YMM4のメインウィンドウをすべて取得します
  - `GetToolWindow(string windowName)`
    - ツールウィンドウを取得します
  - `FocusBack()`
    - 外部のアプリにフォーカスが移った後に呼ぶとYMM4にフォーカスを戻すことができる

## `YmmeUtil.Bridge`

プラグイン向けに公開されていないYMM4本体機能へアクセスするためのラッパーライブラリです。

YMM4本体のアップデートで機能しなくなる可能性があります。
ただし、その場合でも直接参照するよりこのラッパーを経由することで、プラグイン側の処理は変えなくても済むようになります。

- `TimelineUtil`
  - タイムラインにアクセスするためのクラス
- `ItemEditorUtil`
  - アイテムエディタ（右側のパネル）にアクセスするためのクラス
- `Wrap/`
  - `Items/`
    - 各アイテムのラッパークラス
  - `ItemFactory`
    - 動的なアイテムオブジェクトから適切なラッパーオブジェクトを生成するファクトリクラス
  - `ViewModels/`
    - `WrapTimelineItemViewModel`
      - タイムラインに表示されているアイテムのViewModelのラッパー

## ライブラリ

- [GithubReleaseDownloader](https://github.com/russkyc/github-release-downloader/blob/master/LICENSE)
  - MIT License
  - Copyright (c) 2023 Russell Camo (@russkyc)
- [Mayerch1.GithubUpdateCheck](https://github.com/Mayerch1/GithubUpdateCheck/blob/master/LICENSE)
  - MIT License
  - Copyright (c) 2020 Christian Mayer
