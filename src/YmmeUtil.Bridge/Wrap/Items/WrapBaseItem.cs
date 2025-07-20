using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Text.Json;
using System.Windows.Media;
using YmmeUtil.Bridge.Internal;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Exo;
using YukkuriMovieMaker.ItemEditor;
using YukkuriMovieMaker.Project;
using YukkuriMovieMaker.UndoRedo;

namespace YmmeUtil.Bridge.Wrap.Items;

/// <summary>
/// ラッパーオブジェクト YukkuriMovieMaker.Project.Items.BaseItem
/// </summary>
public partial record WrapBaseItem
	: INotifyPropertyChanged,
		INotifyPropertyChanging,
		INotifyDataErrorInfo,
		IUndoRedoable,
		IEditable,
		IFileItem,
		IWrapBaseItem
{
	public dynamic RawItem => Item;
	public virtual string RawItemTypeName => "YukkuriMovieMaker.Project.Items.BaseItem";

	// 以下はYMM4本体側の実装が変わってもいいように
	// ラップしたプロパティ

	public bool HasErrors => Item.HasErrors;
	public bool IsHidden
	{
		get => Item.IsHidden;
		set => Item.IsHidden = value;
	}
	public bool IsLocked
	{
		get => Item.IsLocked;
		set => Item.IsLocked = value;
	}
	public int Group
	{
		get => Item.Group;
		set => Item.Group = value;
	}
	public KeyFrames KeyFrames => Item.KeyFrames;
	public int Frame
	{
		get => Item.Frame;
		set => Item.Frame = value;
	}
	public int Length
	{
		get => Item.Length;
		set => Item.Length = value;
	}
	public int Layer
	{
		get => Item.Layer;
		set => Item.Layer = value;
	}
	public double PlaybackRate
	{
		get => Item.PlaybackRate;
		set => Item.PlaybackRate = value;
	}
	public TimeSpan ContentOffset
	{
		get => Item.ContentOffset;
		set => Item.ContentOffset = value;
	}
	public TimeSpan ContentLength => Item.ContentLength;
	public IEnumerable<TimeSpan> ContentSeparations => Item.ContentSeparations;
	public string Label => Item.Label;
	public string Description => Item.Description;
	public string Remark
	{
		get => Item.Remark;
		set => Item.Remark = value;
	}
	public Color ItemColor
	{
		get => Item.ItemColor;
		set => Item.ItemColor = value;
	}

	protected dynamic Item { get; init; }

	public event PropertyChangedEventHandler? PropertyChanged;
	public event PropertyChangingEventHandler? PropertyChanging;
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
	public event EventHandler<UndoRedoEventArgs>? UndoRedoCommandCreated;

	public WrapBaseItem(dynamic item)
	{
		Item = item;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage(
		"Design",
		"MA0056:Do not call overridable members in constructor",
		Justification = "<保留中>"
	)]
	public WrapBaseItem()
	{
		Debug.WriteLine($"RawItemTypeName: {RawItemTypeName}");
		Type? itemType = null;
		try
		{
			// 現在ロードされているすべてのアセンブリから型を検索
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				itemType = assembly.GetType(RawItemTypeName);
				if (itemType is not null)
				{
					break;
				}
			}
		}
		catch (System.Exception ex)
		{
			Debug.WriteLine($"RawItemTypeName: {RawItemTypeName} not found. {ex.Message}");
		}

		itemType ??= typeof(ExpandoObject);
		var rawItem = Activator.CreateInstance(itemType);
		Item = rawItem ?? new ExpandoObject();
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
			dynamicValue = Reflect.GetProp(Item, propName);
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
			dynamicValue = Reflect.GetField(Item, fieldName);
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
		Item.BeginEdit();
	}

	/// <inheritdoc/>
	public async ValueTask EndEditAsync()
	{
		await Item.EndEditAsync();
	}

	/// <inheritdoc/>
	public IEnumerable<string> GetFiles()
	{
		return Item.GetFiles();
	}

	/// <inheritdoc/>
	public void ReplaceFile(string from, string to)
	{
		Item.ReplaceFile(from, to);
	}

	/// <inheritdoc/>
	public IEnumerable<TimelineResource> GetResources()
	{
		return Item.GetResources();
	}

	/// <inheritdoc/>
	public IEnumerable GetErrors(string? propertyName)
	{
		return Item.GetErrors(propertyName);
	}

	public WrapBaseItem GetClone()
	{
		return new(Item.GetClone());
	}

	public (WrapBaseItem left, WrapBaseItem right) Split(int frame)
	{
		(dynamic left, dynamic right) x = Item.Split(frame);
		return (new(x.left), new(x.right));
	}

	public void SetFPS(int fps)
	{
		Item.SetFPS(fps);
	}

	public IAsyncEnumerable<ExoItem> GetExoItemsAsync(ExoOutputDescription outputDescription)
	{
		return Item.GetExoItemsAsync(outputDescription);
	}

	public string ToJson()
	{
		return JsonSerializer.Serialize(Item, new JsonSerializerOptions { WriteIndented = true });
	}
}
