using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Epoxy;
using YmmeUtil.Bridge.Wrap.Items;

namespace YmmeUtil.Sandbox.ObjectListTest.View;

//[ViewModel]
public class ObjectListItem : INotifyPropertyChanged
{
	private readonly IWrapBaseItem _item;

	public ObjectListItem(IWrapBaseItem item)
	{
		_item = item;
		// アイテムの変更を監視
		if (_item is INotifyPropertyChanged notifyItem)
		{
			notifyItem.PropertyChanged += OnItemPropertyChanged;
		}
	}

	public string Label => _item.Label;
	public int Group => _item.Group;
	public int Layer => _item.Layer;
	public Brush ColorBrush => new SolidColorBrush(_item.ItemColor);

	public string Category =>
		_item.RawItem.GetType().Name switch
		{
			"WrapVideoItem" => "Video",
			"WrapAudioItem" => "Audio",
			"WrapImageItem" => "画像",
			"WrapTextItem" => "Text",
			"WrapFrameBufferItem" => "画面の複製",
			"WrapEffectItem" => "エフェクト",
			"WrapSceneItem" => "シーン",
			"WrapTransitionItem" => "トランジション",
			"WrapTachieItem" => "立ち絵",
			"WrapTachieFaceItem" => "表情",
			"WrapShapeItem" => "図形",
			"WrapVoiceItem" => "音声",
			"WrapGroupItem" => "グループ",
			_ => "Other",
		};
	public bool IsLocked
	{
		get => _item.IsLocked;
		set => _item.IsLocked = value;
	}

	public bool IsHidden
	{
		get => _item.IsHidden;
		set => _item.IsHidden = value;
	}

	void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		// アイテムのプロパティが変更されたら、対応するプロパティの変更通知を送る
		switch (e.PropertyName)
		{
			case nameof(IWrapBaseItem.Label):
				OnPropertyChanged(nameof(Label));
				break;
			case nameof(IWrapBaseItem.Group):
				OnPropertyChanged(nameof(Group));
				break;
			case nameof(IWrapBaseItem.Layer):
				OnPropertyChanged(nameof(Layer));
				break;
			case nameof(IWrapBaseItem.ItemColor):
				OnPropertyChanged(nameof(ColorBrush));
				break;
			case nameof(IWrapBaseItem.IsLocked):
				OnPropertyChanged(nameof(IsLocked));
				break;
			case nameof(IWrapBaseItem.IsHidden):
				OnPropertyChanged(nameof(IsHidden));
				break;
			default:
				break;
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
