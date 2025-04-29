using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace YmmeUtil.Generators;

[Generator]
public class ForwardingGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		// クラス宣言を含むシンタックスノードを登録
		var classDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (s, _) => s is ClassDeclarationSyntax,
				transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node
			)
			.Where(cds => cds.Members
				.OfType<PropertyDeclarationSyntax>()
				.Any(p => p.AttributeLists
					.SelectMany(a => a.Attributes)
					.Any(a => a.Name.ToString().Contains("WrapForward"))
				)
			);

		// 出力の登録
		context.RegisterSourceOutput(
			classDeclarations,
			static (spc, classDeclaration) => GenerateForwardingCode(spc, classDeclaration)
		);
	}

	private static void GenerateForwardingCode(
		SourceProductionContext context,
		ClassDeclarationSyntax classSyntax
	)
	{
		var namespaceName = GetNamespace(classSyntax);
		var className = classSyntax.Identifier.Text;
		var accessModifier = GetAccessModifier(classSyntax);
		var properties = classSyntax
			.Members.OfType<PropertyDeclarationSyntax>()
			.Where(p =>
				p.AttributeLists.SelectMany(a => a.Attributes)
					.Any(a => a.Name.ToString().Contains("WrapForward"))
			);

		if (!properties.Any())
			return;

		var sb = new StringBuilder();
		foreach (var property in properties)
		{
			var propertyName = property.Identifier.Text;
			var typeName = property.Type.ToString();

			var attribute = property
				.AttributeLists.SelectMany(a => a.Attributes)
				.First(a => a.Name.ToString().Contains("WrapForward"));

			var fieldName =
				attribute.ArgumentList?.Arguments.FirstOrDefault()?.ToString().Trim('"')
				?? "_internalObject";

			sb.AppendLine(
				$$"""
				public {{typeName}} {{propertyName}}
				{
					get => {{fieldName}}.{{propertyName}};
					set => {{fieldName}}.{{propertyName}} = value;
				}
			"""
			);
		}

		var source = $$"""
			namespace {{namespaceName}}
			{
				{{accessModifier}} partial class {{className}}
				{
			{{sb.ToString()}}
				}
			}
			""";

		context.AddSource(
			$"{className}_Forwarding.g.cs",
			SourceText.From(source, Encoding.UTF8)
		);
	}

	private static string GetAccessModifier(ClassDeclarationSyntax classSyntax)
	{
		if (classSyntax.Modifiers.Any(m => m.Text == "public"))
			return "public";
		if (classSyntax.Modifiers.Any(m => m.Text == "internal"))
			return "internal";
		if (classSyntax.Modifiers.Any(m => m.Text == "private"))
			return "private";
		if (classSyntax.Modifiers.Any(m => m.Text == "protected"))
			return "protected";

		return ""; // デフォルト（明示的なアクセス修飾子なし）
	}

	private static string GetNamespace(ClassDeclarationSyntax classSyntax)
	{
		var namespaceSyntax = classSyntax.Parent;
		while (
			namespaceSyntax != null
			&& namespaceSyntax is not NamespaceDeclarationSyntax
		)
		{
			namespaceSyntax = namespaceSyntax.Parent;
		}

		return (namespaceSyntax as NamespaceDeclarationSyntax)?.Name.ToString()
			?? "GlobalNamespace";
	}
}