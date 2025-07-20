using System.Diagnostics;
using YmmeUtil.Bridge.Wrap.Items;

namespace YmmeUtil.Bridge.Wrap;

/// <summary>
/// 動的なアイテムオブジェクトから適切なラッパーオブジェクトを生成するファクトリ
/// </summary>
public static class ItemFactory
{
	/// <summary>
	/// 動的なアイテムオブジェクトから、型に応じた適切なラッパーオブジェクトを作成します
	/// </summary>
	/// <param name="item">YMM4のアイテムオブジェクト</param>
	/// <returns>適切な型のラッパーオブジェクト</returns>
	public static WrapBaseItem Create(dynamic item)
	{
		if (item is null)
			return new WrapBaseItem(item);

		// 型名を取得して適切なラッパーオブジェクトに変換
		var typeName = item.GetType().Name;

		return typeName switch
		{
			"TextItem" => new WrapTextItem(item),
			"ImageItem" => new WrapImageItem(item),
			"VideoItem" => new WrapVideoItem(item),
			"AudioItem" => new WrapAudioItem(item),
			"VoiceItem" => new WrapVoiceItem(item),
			"ShapeItem" => new WrapShapeItem(item),
			"TachieItem" => new WrapTachieItem(item),
			"TachieFaceItem" => new WrapTachieFaceItem(item),
			"EffectItem" => new WrapEffectItem(item),
			"TransitionItem" => new WrapTransitionItem(item),
			"SceneItem" => new WrapSceneItem(item),
			"FrameBufferItem" => new WrapFrameBufferItem(item),
			"GroupItem" => new WrapGroupItem(item),
			// 未知のタイプの場合は基本クラスを使用
			_ => new WrapBaseItem(item),
		};
	}
}
