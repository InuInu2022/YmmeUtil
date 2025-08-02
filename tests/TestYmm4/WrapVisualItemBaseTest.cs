using System;
using System.Collections.Generic;
using System.Windows.Media;
using Xunit;

using YmmeUtil.Bridge.Wrap.Items;

using YukkuriMovieMaker;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Project;

namespace TestYmm4;

public class WrapVisualItemBaseTest
{
	public WrapVisualItemBaseTest()
	{
		YmmAssemblyLoader.LoadYmmAssembly();
	}

	// WrapVisualItemBaseのテスト用の派生クラス
	private record TestVisualItem : WrapVisualItemBase
    {
        public override string RawItemTypeName => "YukkuriMovieMaker.Project.Items.VisualItem";

        public TestVisualItem(dynamic item) : base((object)item) { }
    }

    [Fact]
    public void VisualBasePropsTest()
    {
        // 準備
        var mockItem = TestItemFactory.CreateMockVisualItem();
        var visualItem = new TestVisualItem(mockItem);

        // 検証
        Assert.Equal(100.0, visualItem.X.DefaultValue);
        Assert.Equal(200.0, visualItem.Y.DefaultValue);
        Assert.Equal(0.0, visualItem.Z.DefaultValue);
        Assert.Equal(100.0, visualItem.Opacity.DefaultValue);
        Assert.Equal(Blend.Normal, visualItem.Blend);
        Assert.False(visualItem.IsInverted);
        Assert.False(visualItem.IsAlwaysOnTop);
    }

    [Fact]
    public void VisualSetPropsTest()
    {
        // 準備
        var mockItem = TestItemFactory.CreateMockVisualItem();
        var visualItem = new TestVisualItem(mockItem);

        // 実行
        visualItem.X = new Animation(300.0, -1000.0, 1000.0);
        visualItem.Y = new Animation(400.0, -1000.0, 1000.0);
        visualItem.Opacity = new Animation(50.0, 0.0, 100.0);
        visualItem.Blend = Blend.Multiply; // Blend.Multiply = 1
        visualItem.IsInverted = true;
        visualItem.IsAlwaysOnTop = true;

        // 検証
        Assert.Equal(300.0, visualItem.X.DefaultValue);
        Assert.Equal(400.0, visualItem.Y.DefaultValue);
        Assert.Equal(50.0, visualItem.Opacity.DefaultValue);
        Assert.Equal(Blend.Multiply, visualItem.Blend);
        Assert.True(visualItem.IsInverted);
        Assert.True(visualItem.IsAlwaysOnTop);

        // 元のオブジェクトも変更されていることを確認
        Assert.Equal(300.0, mockItem.X.DefaultValue);
        Assert.Equal(400.0, mockItem.Y.DefaultValue);
        Assert.Equal(50.0, mockItem.Opacity.DefaultValue);
        Assert.Equal(Blend.Multiply, mockItem.Blend); // Blend.Multiply = 1
        Assert.True(mockItem.IsInverted);
        Assert.True(mockItem.IsAlwaysOnTop);
    }
}