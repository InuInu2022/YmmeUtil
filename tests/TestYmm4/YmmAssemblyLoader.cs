using System.IO;
using System.Reflection;

namespace TestYmm4;

/// <summary>
/// テスト環境でYukkuriMovieMaker.dllをロードするためのユーティリティクラス
/// </summary>
public static class YmmAssemblyLoader
{
	private static bool _isInitialized;
	private static Assembly? _ymmAssembly;

	/// <summary>
	/// YukkuriMovieMaker.dllをロードします。
	/// テスト実行前に一度だけ呼び出すことを想定しています。
	/// </summary>
	/// <returns>ロードされたアセンブリ、またはnull（ロード失敗時）</returns>
	public static Assembly? LoadYmmAssembly()
	{
		if (_isInitialized)
			return _ymmAssembly;

		_isInitialized = true;

		try
		{
			// 現在読み込まれているアセンブリからYMM4のアセンブリを探す
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly.GetName().Name == "YukkuriMovieMaker")
				{
					_ymmAssembly = assembly;
					Console.WriteLine(
						$"YukkuriMovieMaker.dll found in loaded assemblies: {assembly.Location}"
					);
					return _ymmAssembly;
				}
			}

			// YMM4のDLLを探すパスを指定
			// 環境変数YMM4_PATHを確認
			string? ymm4Path = Environment.GetEnvironmentVariable("YMM4_PATH");
			var searchPaths = new List<string>();

			if (!string.IsNullOrEmpty(ymm4Path))
			{
				searchPaths.Add(ymm4Path);
			}

			// デフォルトのインストールパス
			searchPaths.AddRange(
				[
					@"C:\Program Files\YukkuriMovieMaker4",
					@"D:\Program Files\YukkuriMovieMaker4",
					@"C:\Program Files (x86)\YukkuriMovieMaker4",
				]
			);

			foreach (var basePath in searchPaths)
			{
				if (string.IsNullOrEmpty(basePath))
					continue;

				string dllPath = Path.Combine(basePath, "YukkuriMovieMaker.dll");
				if (File.Exists(dllPath))
				{
					_ymmAssembly = Assembly.LoadFrom(dllPath);
					Console.WriteLine($"YukkuriMovieMaker.dll loaded from {dllPath}");
					return _ymmAssembly;
				}
			}

			Console.WriteLine("YukkuriMovieMaker.dll not found in the search paths.");
			return null;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error loading YukkuriMovieMaker.dll: {ex.Message}");
			return null;
		}
	}

	/// <summary>
	/// YMM4のアセンブリから型を取得します
	/// </summary>
	/// <param name="typeName">型の完全修飾名</param>
	/// <returns>取得された型、または失敗時はnull</returns>
	public static Type? GetYmmType(string typeName)
	{
		var assembly = LoadYmmAssembly();
		if (assembly == null)
			return null;

		return assembly.GetType(typeName);
	}

	/// <summary>
	/// YMM4の型のインスタンスを作成します
	/// </summary>
	/// <param name="typeName">型の完全修飾名</param>
	/// <returns>作成されたインスタンス、または失敗時はnull</returns>
	public static object? CreateYmmInstance(string typeName)
	{
		var type = GetYmmType(typeName);
		if (type == null)
			return null;

		try
		{
			return Activator.CreateInstance(type);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error creating instance of {typeName}: {ex.Message}");
			return null;
		}
	}
}
