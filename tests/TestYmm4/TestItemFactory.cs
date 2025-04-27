using System.Dynamic;
using System.Windows.Media;

using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Project.Items;

namespace TestYmm4;

// モックオブジェクト作成のためのヘルパークラス
public static class TestItemFactory
{
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
        expando.KeyFrames = new MockKeyFrames();

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
		var expando = new TextItem
		{
			// テキスト系プロパティを追加
			Text = "サンプルテキスト",
			Font = "游ゴシック",
			FontColor = Colors.Black,
			//FontSize = new Animation(20.0, 1.0, 100.0),
			Bold = false,
			Italic = false,
			IsDevidedPerCharacter = false,
			IsTrimEndSpace = true,
			Decorations = [], // テキスト装飾リスト
			DisplayInterval = 0.0
		};

		return expando;
    }
}
