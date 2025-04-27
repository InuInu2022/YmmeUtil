using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Media;

using YukkuriMovieMaker.Commons;

namespace TestYmm4;

// モックオブジェクト作成のためのヘルパークラス
public static class TestItemFactory
{
    static TestItemFactory()
    {
        // テストクラスが使用される前にYMM4のアセンブリをロード
        YmmAssemblyLoader.LoadYmmAssembly();
    }

    // モックベースアイテムを作成するヘルパーメソッド
    public static dynamic CreateMockBaseItem()
    {
        dynamic expando = new ExpandoObject();
        expando.IsHidden = false;
        expando.IsLocked = false;
        expando.Group = 0;
        expando.Frame = 100;
        expando.Length = 300;
        expando.Layer = 1;
        expando.PlaybackRate = 1.0;
        expando.ContentOffset = TimeSpan.FromSeconds(0);
        expando.ContentLength = TimeSpan.FromSeconds(10);
        expando.ContentSeparations = new List<TimeSpan>();
        expando.Label = "テストラベル";
        expando.Description = "テスト説明";
        expando.Remark = "テスト備考";
        expando.ItemColor = Colors.Red;
        expando.HasErrors = false;

        // KeyFramesの実際のインスタンスを作成して設定
        // YMM4のアセンブリから直接作成を試みる
        var keyFrames = YmmAssemblyLoader.CreateYmmInstance("YukkuriMovieMaker.Commons.KeyFrames");
        expando.KeyFrames = keyFrames ?? new MockKeyFrames();

        // GetFilesメソッドのモック
        var dict = expando as IDictionary<string, object>;
        dict!["GetFiles"] = new Func<IEnumerable<string>>(() =>
            new List<string> { "test1.png", "test2.png" });

        return expando;
    }

    // VisualItemのモックを作成
    public static dynamic CreateMockVisualItem()
    {
        dynamic expando = CreateMockBaseItem();

        // ビジュアル系プロパティを追加
        expando.X = new Animation(100.0, -1000.0, 1000.0);
        expando.Y = new Animation(200.0, -1000.0, 1000.0);
        expando.Z = new Animation(0.0, -1000.0, 1000.0);
        expando.Opacity = new Animation(100.0, 0.0, 100.0);
        expando.Blend = 0; // Normal
        expando.IsInverted = false;
        expando.IsAlwaysOnTop = false;

        return expando;
    }

    // TextItemのモックを作成
    public static dynamic CreateMockTextItem()
    {
        dynamic expando = CreateMockVisualItem();

        // テキスト系プロパティを追加
        expando.Text = "サンプルテキスト";
        expando.Font = "游ゴシック";
        expando.FontColor = Colors.Black;
        expando.FontSize = new Animation(20.0, 1.0, 100.0);
        expando.Bold = false;
        expando.Italic = false;
        expando.IsDevidedPerCharacter = false;
        expando.IsTrimEndSpace = true;
        expando.Decorations = new List<object>(); // テキスト装飾リスト
        expando.DisplayInterval = 0.0;

        return expando;
    }
}
