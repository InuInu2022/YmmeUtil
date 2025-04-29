using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using YmmeUtil.Generators;

namespace TestGenerators;

public class ForwardingGeneratorTests
{
	[Fact]
	public void SimpleForwardingTest()
	{
		// テスト対象のソースコード
		const string source = """
			using System;

			namespace TestNamespace
			{
			    [AttributeUsage(AttributeTargets.Property)]
			    public class WrapForwardAttribute : Attribute
			    {
			        public WrapForwardAttribute() { }
			        public WrapForwardAttribute(string fieldName) { }
			    }

			    public partial class TestClass
			    {
			        private readonly InnerClass _internalObject = new InnerClass();

			        [WrapForward]
			        public int Value { get; set;}
			    }

			    public class InnerClass
			    {
			        public int Value { get; set; }
			    }
			}
			""";

		// 期待される生成コードのパターン
		var expectedPatterns = new[]
		{
			@"namespace\s+TestNamespace",
			@"public\s+partial\s+class\s+TestClass",
			@"public\s+int\s+Value",
			@"get\s+=>\s+_internalObject\.Value",
			@"set\s+=>\s+_internalObject\.Value\s+=\s+value",
		};

		// テストの実行
		RunTest(source, expectedPatterns);
	}

	[Fact]
	public void MultiplePropertiesTest()
	{
		// テスト対象のソースコード
		const string source = """
			using System;

			namespace TestNamespace
			{
			    [AttributeUsage(AttributeTargets.Property)]
			    public class WrapForwardAttribute : Attribute
			    {
			        public WrapForwardAttribute() { }
			        public WrapForwardAttribute(string fieldName) { }
			    }

			    public partial class TestClass
			    {
			        private readonly InnerClass _internalObject = new InnerClass();

			        [WrapForward]
			        public int Id { get; set;}

			        [WrapForward]
			        public string Name { get; set;}
			    }

			    public class InnerClass
			    {
			        public int Id { get; set; }
			        public string Name { get; set; }
			    }
			}
			""";

		// 期待される生成コードのパターン
		var expectedPatternsId = new[]
		{
			@"public\s+int\s+Id",
			@"get\s+=>\s+_internalObject\.Id",
			@"set\s+=>\s+_internalObject\.Id\s+=\s+value",
		};

		var expectedPatternsName = new[]
		{
			@"public\s+string\s+Name",
			@"get\s+=>\s+_internalObject\.Name",
			@"set\s+=>\s+_internalObject\.Name\s+=\s+value",
		};

		// テストの実行
		RunTest(source, expectedPatternsId);
		RunTest(source, expectedPatternsName);
	}

	[Fact]
	public void CustomFieldNameTest()
	{
		// テスト対象のソースコード
		const string source = """
			using System;

			namespace TestNamespace
			{
				[AttributeUsage(AttributeTargets.Property)]
				public class WrapForwardAttribute : Attribute
				{
					public WrapForwardAttribute() { }
					public WrapForwardAttribute(string fieldName) { }
				}

				public partial class TestClass
				{
					private readonly InnerClass _customField = new InnerClass();

					[WrapForward("_customField")]
					public int Value { get; set; }
				}

				public class InnerClass
				{
					public int Value { get; set; }
				}
			}
			""";

		// 期待される生成コードのパターン
		var expectedPatterns = new[]
		{
			@"public\s+int\s+Value",
			@"get\s+=>\s+_customField\.Value",
			@"set\s+=>\s+_customField\.Value\s+=\s+value",
		};

		// テストの実行
		RunTest(source, expectedPatterns);
	}

	[Fact]
	public void GetOnlyPropertyTest()
	{
		// テスト対象のソースコード
		const string source = """

			using System;

			namespace TestNamespace
			{
				[AttributeUsage(AttributeTargets.Property)]
				public class WrapForwardAttribute : Attribute
				{
					public WrapForwardAttribute() { }
					public WrapForwardAttribute(string fieldName) { }
				}

				public partial class TestClass
				{
					private readonly InnerClass _internalObject = new InnerClass();

					[WrapForward]
					public int ReadOnlyValue { get; }
				}

				public class InnerClass
				{
					public int ReadOnlyValue { get; }
				}
			}
			""";

		// 期待される生成コードのパターン
		var expectedPatterns = new[]
		{
			@"public\s+int\s+ReadOnlyValue",
			@"get\s+=>\s+_internalObject\.ReadOnlyValue",
			// setアクセサーが含まれていないことを確認
		};

		// 生成されないことを期待するパターン
		var unexpectedPatterns = new[] { @"set\s+=>\s+_internalObject\.ReadOnlyValue\s+=\s+value" };

		// テストの実行
		RunTest(source, expectedPatterns, unexpectedPatterns);
	}

	[Fact]
	public void SetOnlyPropertyTest()
	{
		// テスト対象のソースコード
		const string source = """

			using System;

			namespace TestNamespace
			{
				[AttributeUsage(AttributeTargets.Property)]
				public class WrapForwardAttribute : Attribute
				{
					public WrapForwardAttribute() { }
					public WrapForwardAttribute(string fieldName) { }
				}

				public partial class TestClass
				{
					private readonly InnerClass _internalObject = new InnerClass();

					[WrapForward]
					public int WriteOnlyValue { set; }
				}

				public class InnerClass
				{
					public int WriteOnlyValue { set; }
				}
			}
			""";

		// 期待される生成コードのパターン
		var expectedPatterns = new[]
		{
			@"public\s+int\s+WriteOnlyValue",
			@"set\s+=>\s+_internalObject\.WriteOnlyValue\s+=\s+value",
			// getアクセサーが含まれていないことを確認
		};

		// 生成されないことを期待するパターン
		var unexpectedPatterns = new[] { @"get\s+=>\s+_internalObject\.WriteOnlyValue" };

		// テストの実行
		RunTest(source, expectedPatterns, unexpectedPatterns);
	}

	[Fact]
	public void MixedAccessorsPropertyTest()
	{
		// テスト対象のソースコード
		const string source = """
			using System;

			namespace TestNamespace
			{
				[AttributeUsage(AttributeTargets.Property)]
				public class WrapForwardAttribute : Attribute
				{
					public WrapForwardAttribute() { }
					public WrapForwardAttribute(string fieldName) { }
				}

				public partial class TestClass
				{
					private readonly InnerClass _internalObject = new InnerClass();

					[WrapForward]
					public int ReadOnlyProperty { get; }

					[WrapForward]
					public string WriteOnlyProperty { set; }

					[WrapForward]
					public double NormalProperty { get; set; }
				}

				public class InnerClass
				{
					public int ReadOnlyProperty { get; }
					public string WriteOnlyProperty { set; }
					public double NormalProperty { get; set; }
				}
			}
			""";

		// ReadOnlyProperty のテスト
		var expectedPatternsReadOnly = new[]
		{
			@"public\s+int\s+ReadOnlyProperty",
			@"get\s+=>\s+_internalObject\.ReadOnlyProperty",
		};
		var unexpectedPatternsReadOnly = new[]
		{
			@"set\s+=>\s+_internalObject\.ReadOnlyProperty\s+=\s+value",
		};

		// WriteOnlyProperty のテスト
		var expectedPatternsWriteOnly = new[]
		{
			@"public\s+string\s+WriteOnlyProperty",
			@"set\s+=>\s+_internalObject\.WriteOnlyProperty\s+=\s+value",
		};
		var unexpectedPatternsWriteOnly = new[]
		{
			@"get\s+=>\s+_internalObject\.WriteOnlyProperty",
		};

		// NormalProperty のテスト
		var expectedPatternsNormal = new[]
		{
			@"public\s+double\s+NormalProperty",
			@"get\s+=>\s+_internalObject\.NormalProperty",
			@"set\s+=>\s+_internalObject\.NormalProperty\s+=\s+value",
		};

		// テストの実行
		RunTest(source, expectedPatternsReadOnly, unexpectedPatternsReadOnly);
		RunTest(source, expectedPatternsWriteOnly, unexpectedPatternsWriteOnly);
		RunTest(source, expectedPatternsNormal);
	}

	private static void RunTest(
		string source,
		string[] expectedPatterns,
		string[]? unexpectedPatterns = null
	)
	{
		// 必要な基本参照を取得
		var references = new[]
		{
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
			MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
		};

		// コンパイルオプションの設定
		var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

		// 入力コンパイルを作成
		var compilation = CSharpCompilation.Create(
			assemblyName: "TestAssembly",
			syntaxTrees: new[] { CSharpSyntaxTree.ParseText(source) },
			references: references,
			options: options
		);

		// インクリメンタルジェネレーターの作成と実行
		var generator = new ForwardingGenerator();
		var driver = CSharpGeneratorDriver.Create(
			generators: new[] { generator.AsSourceGenerator() },
			additionalTexts: ImmutableArray<AdditionalText>.Empty,
			parseOptions: compilation.SyntaxTrees.First().Options as CSharpParseOptions
		);

		// ソースジェネレーターの実行
		_ = driver.RunGeneratorsAndUpdateCompilation(
			compilation,
			out var outputCompilation,
			out var diagnostics
		);

		// 診断情報をチェック
		Assert.Empty(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));

		// 生成されたファイルを取得して検証
		var trees = outputCompilation.SyntaxTrees.Except(compilation.SyntaxTrees);
		var allContent = string.Join("\n", trees.Select(t => t.GetText().ToString()));

		// 各パターンが生成されたコードに含まれるか確認
		foreach (var pattern in expectedPatterns)
		{
			bool matches = Regex.IsMatch(allContent, pattern, RegexOptions.Singleline);
			Assert.True(
				matches,
				$"Generated code does not match expected pattern: {pattern}\nGenerated code:\n{allContent}"
			);
		}

		// 除外するパターンが生成されたコードに含まれていないかを確認
		if (unexpectedPatterns != null)
		{
			foreach (var pattern in unexpectedPatterns)
			{
				bool matches = Regex.IsMatch(allContent, pattern, RegexOptions.Singleline);
				Assert.False(
					matches,
					$"Generated code unexpectedly contains pattern: {pattern}\nGenerated code:\n{allContent}"
				);
			}
		}
	}
}
