using System;
using System.Windows.Media;
using Xunit;

using YmmeUtil.Ymm4.Wrap.Items;

using YukkuriMovieMaker.Commons;

namespace TestYmm4;

public class WrapTextItemTest
{
	public WrapTextItemTest()
	{
		YmmAssemblyLoader.LoadYmmAssembly();
	}

	[Fact]
    public void TextPropsTest()
    {
        // 準備
        //var mockItem = TestItemFactory.CreateMockTextItem();
		var textItem = new WrapTextItem()
		{
			Text = "サンプルテキスト",
			Font = "游ゴシック",
			FontColor = Colors.Black,
			Bold = false,
			Italic = false,
			IsDividedPerCharacter = false,
			IsTrimEndSpace = true,
			Decorations = [],
			DisplayInterval = 0.0
		};

        // 検証
        Assert.Equal("サンプルテキスト", textItem.Text);
        Assert.Equal("游ゴシック", textItem.Font);
        Assert.Equal(Colors.Black, textItem.FontColor);
        //Assert.Equal(20.0, textItem.FontSize.DefaultValue);
        Assert.False(textItem.Bold);
        Assert.False(textItem.Italic);
        Assert.False(textItem.IsDividedPerCharacter);
        Assert.True(textItem.IsTrimEndSpace);
        Assert.Empty(textItem.Decorations);
        Assert.Equal(0.0, textItem.DisplayInterval);
    }

    [Fact]
    public void TextSetPropsTest()
    {
        // 準備
        //var mockItem = TestItemFactory.CreateMockTextItem();
        var textItem = new WrapTextItem()
		{
			Text = "元のテキスト",
		};

        // 実行
        textItem.Text = "変更されたテキスト";
        textItem.Font = "メイリオ";
        textItem.FontColor = Colors.Red;
        textItem.Bold = true;
        textItem.Italic = true;
        textItem.IsDividedPerCharacter = true;
        textItem.IsTrimEndSpace = false;
        textItem.DisplayInterval = 0.5;

        // 検証
        Assert.Equal("変更されたテキスト", textItem.Text);
        Assert.Equal("メイリオ", textItem.Font);
        Assert.Equal(Colors.Red, textItem.FontColor);
        Assert.True(textItem.Bold);
        Assert.True(textItem.Italic);
        Assert.True(textItem.IsDividedPerCharacter);
        Assert.False(textItem.IsTrimEndSpace);
        Assert.Equal(0.5, textItem.DisplayInterval);

        // 元のオブジェクトも変更されていることを確認
        Assert.Equal("変更されたテキスト", textItem.RawItem.Text);
        Assert.Equal("メイリオ", textItem.RawItem.Font);
        Assert.Equal(Colors.Red, textItem.RawItem.FontColor);
        Assert.Equal(32.0, textItem.RawItem.FontSize.Value);
        Assert.True(textItem.RawItem.Bold);
        Assert.True(textItem.RawItem.Italic);
        Assert.True(textItem.RawItem.IsDividedPerCharacter);
        Assert.False(textItem.RawItem.IsTrimEndSpace);
        Assert.Equal(0.5, textItem.RawItem.DisplayInterval);
    }
}