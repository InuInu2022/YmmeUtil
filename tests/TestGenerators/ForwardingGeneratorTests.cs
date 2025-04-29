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
			        public int Value { get; }
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
			@"set\s+=>\s+_internalObject\.Value\s+=\s+value"
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
			        public int Id { get; }

			        [WrapForward]
			        public string Name { get; }
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
			@"set\s+=>\s+_internalObject\.Id\s+=\s+value"
		};

		var expectedPatternsName = new[]
		{
			@"public\s+string\s+Name",
			@"get\s+=>\s+_internalObject\.Name",
			@"set\s+=>\s+_internalObject\.Name\s+=\s+value"
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
					public int Value { get; }
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
			@"set\s+=>\s+_customField\.Value\s+=\s+value"
		};

		// テストの実行
		RunTest(source, expectedPatterns);
	}

	private static void RunTest(string source, string[] expectedPatterns)
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
	}
}
