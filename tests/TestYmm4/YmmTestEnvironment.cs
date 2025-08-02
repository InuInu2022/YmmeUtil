using System;
using System.Reflection;
using YukkuriMovieMaker.Plugin.Brush;

namespace TestYmm4;

/// <summary>
/// テスト環境でYMM4の初期化を行うためのユーティリティクラス
/// </summary>
public static class YmmTestEnvironment
{
	private static bool _isInitialized;

	/// <summary>
	/// YMM4のテスト環境を初期化します。
	/// 各テストクラスのコンストラクタで呼び出すことを想定しています。
	/// </summary>
	public static void Initialize()
	{
		if (_isInitialized)
			return;

		// YMM4アセンブリをロード
		YmmAssemblyLoader.LoadYmmAssembly();

		// 必要な初期化処理
		InitializeBrushFactory();
		// 他に必要な初期化処理があればここに追加

		_isInitialized = true;
		Console.WriteLine(
			"YMM4 test environment initialized successfully"
		);
	}

	/// <summary>
	/// リフレクションを使ってBrushFactoryのDefaultTypeを初期化します
	/// </summary>
	private static void InitializeBrushFactory()
	{
		try
		{
			// BrushFactoryのDefaultTypeプロパティに直接アクセス
			var brushFactoryType = typeof(BrushFactory);
			var defaultTypeField =
				brushFactoryType.GetField(
					"defaultType",
					BindingFlags.NonPublic
						| BindingFlags.Static
				);

			if (defaultTypeField != null)
			{
				// 直接DefaultTypeフィールドを設定
				defaultTypeField.SetValue(
					null,
					typeof(YukkuriMovieMaker.Plugin.Brush.Brush)
				);
				Console.WriteLine(
					"BrushFactory initialized successfully"
				);
			}
			else
			{
				Console.WriteLine(
					"Could not find defaultType field in BrushFactory"
				);

				// フィールド名が異なる場合に備えて、非公開静的フィールドをすべて表示
				var allFields = brushFactoryType.GetFields(
					BindingFlags.NonPublic
						| BindingFlags.Static
				);
				foreach (var field in allFields)
				{
					Console.WriteLine(
						$"Found field: {field.Name}"
					);
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(
				$"Failed to initialize BrushFactory: {ex.Message}"
			);
		}
	}
}
