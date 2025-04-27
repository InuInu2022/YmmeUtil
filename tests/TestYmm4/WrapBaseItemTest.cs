using System;
using System.Collections.Generic;
using System.Windows.Media;
using Xunit;

using YmmeUtil.Ymm4.Wrap.Items;

using YukkuriMovieMaker;
using YukkuriMovieMaker.Plugin;
using YukkuriMovieMaker.Commons;

namespace TestYmm4;

// KeyFramesクラスをモック化するためのクラス
public class MockKeyFrames : KeyFrames
{
}

public class WrapBaseItemTest
{
	public WrapBaseItemTest()
	{
		YmmAssemblyLoader.LoadYmmAssembly();
	}

	[Fact]
	public void BasicPropsTest()
	{
		// 準備
		var mockItem = TestItemFactory.CreateMockBaseItem();
		var wrapItem = new WrapBaseItem(mockItem);

		// 検証
		Assert.False(wrapItem.IsHidden);
		Assert.False(wrapItem.IsLocked);
		Assert.Equal(0, wrapItem.Group);
		Assert.Equal(100, wrapItem.Frame);
		Assert.Equal(300, wrapItem.Length);
		Assert.Equal(1, wrapItem.Layer);
		Assert.Equal(1.0, wrapItem.PlaybackRate);
		Assert.Equal(TimeSpan.FromSeconds(0), wrapItem.ContentOffset);
		Assert.Equal(TimeSpan.FromSeconds(10), wrapItem.ContentLength);
		Assert.Empty(wrapItem.ContentSeparations);
		Assert.Equal("テストラベル", wrapItem.Label);
		Assert.Equal("テスト説明", wrapItem.Description);
		Assert.Equal("テスト備考", wrapItem.Remark);
		Assert.Equal(Colors.Red, wrapItem.ItemColor);
		Assert.False(wrapItem.HasErrors);
		Assert.NotNull(wrapItem.KeyFrames);
	}

    [Fact]
    public void SetPropsTest()
    {
        // 準備
        var mockItem = TestItemFactory.CreateMockBaseItem();
        var wrapItem = new WrapBaseItem(mockItem);

        // 実行
        wrapItem.IsHidden = true;
        wrapItem.IsLocked = true;
        wrapItem.Group = 2;
        wrapItem.Frame = 200;
        wrapItem.Length = 500;
        wrapItem.Layer = 3;
        wrapItem.PlaybackRate = 1.5;
        wrapItem.ContentOffset = TimeSpan.FromSeconds(2);
        wrapItem.Remark = "変更された備考";
        wrapItem.ItemColor = Colors.Blue;

        // 検証 - ラッパーオブジェクトの状態
        Assert.True(wrapItem.IsHidden);
        Assert.True(wrapItem.IsLocked);
        Assert.Equal(2, wrapItem.Group);
        Assert.Equal(200, wrapItem.Frame);
        Assert.Equal(500, wrapItem.Length);
        Assert.Equal(3, wrapItem.Layer);
        Assert.Equal(1.5, wrapItem.PlaybackRate);
        Assert.Equal(TimeSpan.FromSeconds(2), wrapItem.ContentOffset);
        Assert.Equal("変更された備考", wrapItem.Remark);
        Assert.Equal(Colors.Blue, wrapItem.ItemColor);

        // 元のオブジェクトの状態も変更されていることを確認
        Assert.True(mockItem.IsHidden);
        Assert.True(mockItem.IsLocked);
        Assert.Equal(2, mockItem.Group);
        Assert.Equal(200, mockItem.Frame);
        Assert.Equal(500, mockItem.Length);
        Assert.Equal(3, mockItem.Layer);
        Assert.Equal(1.5, mockItem.PlaybackRate);
        Assert.Equal(TimeSpan.FromSeconds(2), mockItem.ContentOffset);
        Assert.Equal("変更された備考", mockItem.Remark);
        Assert.Equal(Colors.Blue, mockItem.ItemColor);
    }

    [Fact]
    public void GetTypeNameTest()
    {
        // 準備
        var mockItem = TestItemFactory.CreateMockBaseItem();
        var wrapItem = new WrapBaseItem(mockItem);

        // 検証
        Assert.Equal("YukkuriMovieMaker.Project.Items.BaseItem", wrapItem.RawItemTypeName);
    }

    [Fact]
    public void SameRawItemTest()
    {
        // 準備
        var mockItem = TestItemFactory.CreateMockBaseItem();
        var wrapItem = new WrapBaseItem(mockItem);

        // 検証
        Assert.Same(mockItem, wrapItem.RawItem);
    }

    [Fact]
    public void GetFilesTest()
    {
        // 準備
        var mockItem = TestItemFactory.CreateMockBaseItem();
        var wrapItem = new WrapBaseItem(mockItem);

        // 実行
        var files = wrapItem.GetFiles();

        // 検証
        Assert.Collection(files,
            file => Assert.Equal("test1.png", file),
            file => Assert.Equal("test2.png", file)
        );
    }
}
