namespace YmmeUtil.Bridge;

/// <summary>
/// アイテムエディタ（右側のパネル）にアクセスするためのクラス
/// </summary>
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
