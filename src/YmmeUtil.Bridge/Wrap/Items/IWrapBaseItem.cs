using System.Collections;
using System.ComponentModel;
using System.Windows.Media;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Exo;
using YukkuriMovieMaker.Project;
using YukkuriMovieMaker.UndoRedo;

namespace YmmeUtil.Bridge.Wrap.Items;

public interface IWrapBaseItem
{
	/// <summary>
	/// アイテムの動的オブジェクト。
	/// <para>YMM4本体の実装に依存するため、基本的には直接使用しないでください。</para>
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	dynamic RawItem { get; }
	string RawItemTypeName { get; }
	bool HasErrors { get; }
	bool IsHidden { get; set; }
	bool IsLocked { get; set; }
	int Group { get; set; }
	KeyFrames KeyFrames { get; }
	int Frame { get; set; }
	int Length { get; set; }
	int Layer { get; set; }
	double PlaybackRate { get; set; }
	TimeSpan ContentOffset { get; set; }
	TimeSpan ContentLength { get; }
	IEnumerable<TimeSpan> ContentSeparations { get; }
	string Label { get; }
	string Description { get; }
	string Remark { get; set; }
	Color ItemColor { get; set; }

	event PropertyChangedEventHandler? PropertyChanged;
	event PropertyChangingEventHandler? PropertyChanging;
	event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
	event EventHandler<UndoRedoEventArgs>? UndoRedoCommandCreated;

	void BeginEdit();
	ValueTask EndEditAsync();
	bool Equals(object? obj);
	bool Equals(WrapBaseItem? other);
	WrapBaseItem GetClone();
	IEnumerable GetErrors(string? propertyName);
	IAsyncEnumerable<ExoItem> GetExoItemsAsync(ExoOutputDescription outputDescription);
	IEnumerable<string> GetFiles();
	int GetHashCode();
	IEnumerable<TimelineResource> GetResources();
	void ReplaceFile(string from, string to);
	void SetFPS(int fps);
	(WrapBaseItem left, WrapBaseItem right) Split(int frame);
	string ToJson();
	string ToString();
	bool TryGetDynamicField(string fieldName, out dynamic? dynamicValue);
	bool TryGetDynamicProperty(string propName, out dynamic? dynamicValue);
}
