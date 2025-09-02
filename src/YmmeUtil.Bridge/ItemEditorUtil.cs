using System.Diagnostics.CodeAnalysis;

using YmmeUtil.Ymm4;

namespace YmmeUtil.Bridge;

/// <summary>
/// アイテムエディタ（右側のパネル）にアクセスするためのクラス
/// </summary>
public static class ItemEditorUtil
{

	public static bool TryGetItemEditor(
		[NotNullWhen(true)]
		out WrapItemEditor? itemEditor)
	{
		itemEditor = default;

		if (Ymm4Version.HasDocked)
		{
			var mainWinVM = TimelineUtil.GetMainViewModel();
			if (mainWinVM is null)
			{
				return false;
			}

			var itemEditorVM = Internal.Reflect.GetField(
				mainWinVM,
				"itemEditorAreaViewModel",
				isPrivate: true
			);
			if (itemEditorVM is null)
			{
				return false;
			}

			var vm = Internal.Reflect.GetProp(
				itemEditorVM,
				"ViewModel"
			);
			if (vm is null) return false;

			itemEditor = new WrapItemEditor(vm);

			return true;
		}
		else
		{
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
}
