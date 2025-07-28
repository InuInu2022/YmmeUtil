using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Reactive.Bindings;
using YmmeUtil.Bridge.Wrap;
using YmmeUtil.Bridge.Wrap.Items;
using YukkuriMovieMaker.Commons;

namespace YmmeUtil.Bridge;

public static class ItemEditorUtil
{
	public static bool TryGetItemEditor(out WrapItemEditor? itemEditor)
	{
		itemEditor = default;
		var mainWinVM = TimelineUtil.GetMainViewModel();
		if (mainWinVM is null)
		{
			return false;
		}

		var itemEditorVM = Internal.Reflect.GetProp(mainWinVM, "ItemEditorAreaViewModel");
		if (itemEditorVM is null)
		{
			return false;
		}

		var vm = Internal.Reflect.GetProp(itemEditorVM, "ViewModel");
		if (vm is null)
			return false;

		var vmValue = Internal.Reflect.GetProp(vm, "Value");
		if (vmValue is null)
			return false;

		itemEditor = new WrapItemEditor(vmValue);
		return true;
	}
}

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
