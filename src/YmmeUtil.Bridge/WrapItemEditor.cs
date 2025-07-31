using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Reactive.Bindings;
using YmmeUtil.Bridge.Wrap;
using YmmeUtil.Bridge.Wrap.Items;
using YukkuriMovieMaker.Commons;

namespace YmmeUtil.Bridge;

/// <summary>
/// アイテムエディタ（右側のパネル）のラッパークラス
/// </summary>
public partial record WrapItemEditor
{
	public WrapItemEditor(dynamic editor)
	{
		RawItemEditor = editor;
	}

	public dynamic RawItemEditor { get; }

	public IEditorInfo EditorInfo
	{
		get => Internal.Reflect.GetProp<IEditorInfo>(RawItemEditor, nameof(EditorInfo));
		set => RawItemEditor.EditorInfo = value;
	}

	[SuppressMessage("Usage", "SMA0040:Missing Using Statement", Justification = "<保留中>")]
	public ReactivePropertySlim<ImmutableList<IWrapBaseItem>> Items
	{
		get
		{
			var raws = Internal.Reflect.GetProp(RawItemEditor, nameof(Items));
			if (raws is ReactivePropertySlim<ImmutableList<dynamic>> rp)
			{
				var list = rp
					.Value?.Select(item => ItemFactory.Create(item))
					.OfType<IWrapBaseItem>()
					.ToImmutableList();
				return list is not null ? new(list) : new();
			}
			return new();
		}
		set
		{
			if (value is null)
				return;
			if (value.IsDisposed)
				return;
			RawItemEditor.Items = value.Value.Select(i => i.RawItem).ToImmutableList();
		}
	}
}
