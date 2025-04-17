using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Exo;
using YukkuriMovieMaker.ItemEditor;
using YukkuriMovieMaker.Project;
using YukkuriMovieMaker.UndoRedo;

namespace YmmeUtil.Ymm4.Wrap;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.BaseItem
/// </summary>
public partial record WrapBaseItem
	: INotifyPropertyChanged,
		INotifyPropertyChanging,
		INotifyDataErrorInfo,
		IUndoRedoable,
		IEditable,
		IFileItem
{
	public dynamic RawItem => _item;

	// 以下はYMM4本体側の実装が変わってもいいように
	// ラップしたプロパティ

	public bool HasErrors => _item.HasErrors;
	public bool IsHidden
	{
		get => _item.IsHidden;
		set => _item.IsHidden = value;
	}
	public bool IsLocked
	{
		get => _item.IsLocked;
		set => _item.IsLocked = value;
	}
	public int Group
	{
		get => _item.Group;
		set => _item.Group = value;
	}
	public KeyFrames KeyFrames => _item.KeyFrames;
	public int Frame
	{
		get => _item.Frame;
		set => _item.Frame = value;
	}
	public int Length
	{
		get => _item.Length;
		set => _item.Length = value;
	}
	public int Layer
	{
		get => _item.Layer;
		set => _item.Layer = value;
	}
	public double PlaybackRate
	{
		get => _item.PlaybackRate;
		set => _item.PlaybackRate = value;
	}
	public TimeSpan ContentOffset
	{
		get => _item.ContentOffset;
		set => _item.ContentOffset = value;
	}
	public TimeSpan ContentLength => _item.ContentLength;
	public IEnumerable<TimeSpan> ContentSeparations => _item.ContentSeparations;
	public string Label => _item.Label;
	public string Description => _item.Description;
	public string Remark
	{
		get => _item.Remark;
		set => _item.Remark = value;
	}
	public Color ItemColor
	{
		get => _item.ItemColor;
		set => _item.ItemColor = value;
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	public event PropertyChangingEventHandler? PropertyChanging;
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
	public event EventHandler<UndoRedoEventArgs>? UndoRedoCommandCreated;

	readonly dynamic _item;

	public WrapBaseItem(dynamic item)
	{
		_item = item;
	}

	/// <summary>
	/// 安全にプロパティ名でプロパティの値を取得する
	/// </summary>
	/// <param name="propName">プロパティ名</param>
	/// <param name="dynamicValue">値</param>
	/// <returns></returns>
	public bool TryGetDynamicProperty(string propName, out dynamic? dynamicValue)
	{
		try
		{
			dynamicValue = Internal.Reflect.GetProp(_item, propName);
		}
		catch (Exception e)
		{
			Debug.WriteLine($"プロパティ取得失敗: {propName} {e.Message}");
			dynamicValue = default;
			return false;
		}
		return true;
	}

	/// <summary>
	/// 安全にフィールド名でフィールドの値を取得する
	/// </summary>
	/// <param name="fieldName">フィールド名</param>
	/// <param name="dynamicValue">値</param>
	/// <returns></returns>
	public bool TryGetDynamicField(string fieldName, out dynamic? dynamicValue)
	{
		try
		{
			dynamicValue = Internal.Reflect.GetField(_item, fieldName);
		}
		catch (Exception e)
		{
			Debug.WriteLine($"フィールド取得失敗: {fieldName} {e.Message}");
			dynamicValue = default;
			return false;
		}
		return true;
	}

	/// <inheritdoc/>
	public void BeginEdit()
	{
		_item.BeginEdit();
	}

	/// <inheritdoc/>
	public async ValueTask EndEditAsync()
	{
		await _item.EndEditAsync();
	}

	/// <inheritdoc/>
	public IEnumerable<string> GetFiles()
	{
		return _item.GetFiles();
	}

	/// <inheritdoc/>
	public void ReplaceFile(string from, string to)
	{
		_item.ReplaceFile(from, to);
	}

	/// <inheritdoc/>
	public IEnumerable<TimelineResource> GetResources()
	{
		return _item.GetResources();
	}

	/// <inheritdoc/>
	public IEnumerable GetErrors(string? propertyName)
	{
		return _item.GetErrors(propertyName);
	}

	public WrapBaseItem GetClone()
	{
		return new(_item.GetClone());
	}

	public (WrapBaseItem left, WrapBaseItem right) Split(int frame)
	{
		(dynamic left, dynamic right) x = _item.Split(frame);
		return (new(x.left), new(x.right));
	}

	public void SetFPS(int fps)
	{
		_item.SetFPS(fps);
	}

	public IAsyncEnumerable<ExoItem> GetExoItemsAsync(ExoOutputDescription outputDescription)
	{
		return _item.GetExoItemsAsync(outputDescription);
	}
}
