using System.Runtime.CompilerServices;

// YmmeUtil.Bridgeの主要な型をObjectListTest.dllに転送
[assembly: TypeForwardedTo(typeof(YmmeUtil.Bridge.TimelineUtil))]
[assembly: TypeForwardedTo(typeof(YmmeUtil.Bridge.Wrap.ViewModels.WrapTimelineItemViewModel))]
[assembly: TypeForwardedTo(typeof(YmmeUtil.Bridge.Wrap.Items.WrapBaseItem))]
[assembly: TypeForwardedTo(typeof(YmmeUtil.Bridge.Wrap.WrapTimeLine))]

// YmmeUtil.Ymm4の主要な型も転送
[assembly: TypeForwardedTo(typeof(YmmeUtil.Ymm4.TaskbarUtil))]
[assembly: TypeForwardedTo(typeof(YmmeUtil.Ymm4.WindowUtil))]
