using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestYmm4")]
namespace YmmeUtil.Ymm4;

/// <summary>
/// YMM4本体のバージョン判定用クラス
/// </summary>
public static class Ymm4Version
{
	public static Version Current { get; internal set; }
		= YukkuriMovieMaker.Commons.AppVersion.Current;

	#region semantic

	/// <summary>
	/// YMM4がドッキングウィンドウ実装済みのバージョンかどうか
	/// </summary>
	public static bool HasDocked => Current >= Version4450;

	/// <summary>
	/// .NET 9.0が必要なバージョンかどうか
	/// </summary>
	public static bool HasRequiredNet90 => Current >= Version4350;

	/// <summary>
	/// .NET 8.0が必要なバージョンかどうか
	/// </summary>
	public static bool HasRequiredNet80 => Current >= Version4230 && Current < Version4350;

	/// <summary>
	/// 開発者モードが実装されたバージョンかどうか
	/// </summary>
	public static bool HasDeveloperMode => Current >= Version4330;

	/// <summary>
	/// ymme形式のプラグインインストーラが導入されたバージョンかどうか
	/// </summary>
	public static bool HasPluginInstaller => Current >= Version4290;

	/// <summary>
	/// プラグインに対応したバージョンかどうか
	/// </summary>
	/// <remarks>
	/// プラグインが読み込まれることがないためこの判定は使うタイミングがないかもしれません。
	/// </remarks>
	public static bool IsSupportPlugin => Current >= Version4200;

	#endregion semantic

	#region version_milestone

	/// <summary>
	/// ドッキングウィンドウが導入されたバージョン。 v4.45.0
	/// </summary>
	public static Version Version4450 => new(4, 45, 0);

	/// <summary>
	/// .NET9.0以降が必要になったバージョン。 v4.35.0
	/// </summary>
	public static Version Version4350 => new(4, 35, 0);

	/// <summary>
	/// 開発者モードが実装されたバージョン。 v4.33.0
	/// </summary>
	public static Version Version4330 => new(4, 33, 0);

	/// <summary>
	/// ymme形式のプラグインインストーラが導入されたバージョン。 v4.29.0
	/// </summary>
	public static Version Version4290 => new(4, 29, 0);

	/// <summary>
	/// .NET 8.0以降が必要になったバージョン。 v4.23.0
	/// </summary>
	public static Version Version4230 => new(4, 23, 0);

	/// <summary>
	/// プラグインに対応したバージョン。 v4.20.0
	/// </summary>
	public static Version Version4200 => new(4, 20, 0);

	#endregion version_milestone
}
