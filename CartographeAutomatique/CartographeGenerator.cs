using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CartographeAutomatique;

[Generator]
public class CartographeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "MapToAttribute.g.cs",
            SourceText.From(Constant.AttributeSourceCode, Encoding.UTF8)));

        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: "Generators.MapToAttribute",
            predicate: static (syntaxNode, _) => syntaxNode is TypeDeclarationSyntax,
            transform: static (context, _) => GetClassDeclarationForSourceGen(context));

        context.RegisterSourceOutput(pipeline, GenerateCode);
    }

    private static List<TypeMapping> GetClassDeclarationForSourceGen(GeneratorAttributeSyntaxContext context) =>
        context.TargetNode switch
        {
            ClassDeclarationSyntax classDeclaration => PopulateClassMapping(context, classDeclaration),
            RecordDeclarationSyntax recordDeclaration => PopulateRecordMapping(context, recordDeclaration),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static List<TypeMapping> PopulateRecordMapping(GeneratorAttributeSyntaxContext context,
        RecordDeclarationSyntax recordDeclarationSyntax)
    {
        var classMappings = new List<TypeMapping>();

        foreach (var attributeListSyntax in recordDeclarationSyntax.AttributeLists)
            foreach (var arguments in
                     from attributeSyntax in attributeListSyntax.Attributes
                     select attributeSyntax.ArgumentList?.Arguments)
            {
                var targetTypeArgument = arguments?.FirstOrDefault();
                var exhaustive = IsExhaustive(arguments);
                var strategy = GetMappingStrategy(arguments);

                if (targetTypeArgument?.Expression is not TypeOfExpressionSyntax typeOfExpressionSyntax) continue;

                var targetIdentifierName = typeOfExpressionSyntax.Type as IdentifierNameSyntax;
                var targetSymbolInfo = ModelExtensions.GetSymbolInfo(context.SemanticModel, targetIdentifierName!);
                var targetTypeSyntax = targetSymbolInfo.TypeDeclarationSyntaxFromSymbolInfo();

                if (targetTypeSyntax is null) continue;

                classMappings.Add(
                    new TypeMapping(
                        recordDeclarationSyntax,
                        targetTypeSyntax,
                        exhaustive,
                        strategy,
                        context
                    )
                );
            }

        return classMappings;
    }

    private static List<TypeMapping> PopulateClassMapping(GeneratorAttributeSyntaxContext context,
        ClassDeclarationSyntax classDeclarationSyntax)
    {
        var classMappings = new List<TypeMapping>();

        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
            foreach (var arguments in
                     from attributeSyntax in attributeListSyntax.Attributes
                     select attributeSyntax.ArgumentList?.Arguments)
            {
                var targetTypeArgument = arguments?.FirstOrDefault();
                var exhaustive = IsExhaustive(arguments);
                var strategy = GetMappingStrategy(arguments);

                if (targetTypeArgument?.Expression is not TypeOfExpressionSyntax typeOfExpressionSyntax) continue;

                var targetIdentifierName = typeOfExpressionSyntax.Type as IdentifierNameSyntax;
                var targetSymbolInfo = ModelExtensions.GetSymbolInfo(context.SemanticModel, targetIdentifierName!);
                var targetTypeSyntax = targetSymbolInfo.TypeDeclarationSyntaxFromSymbolInfo();

                if (targetTypeSyntax is null) continue;

                classMappings.Add(
                    new TypeMapping(
                        classDeclarationSyntax,
                        targetTypeSyntax,
                        exhaustive,
                        strategy,
                        context
                    )
                );
            }

        return classMappings;
    }

    private static void GenerateCode(SourceProductionContext context, List<TypeMapping> classMappings)
    {
        foreach (var classMapping in classMappings)
        {
            var generatedMapping = classMapping.GenerateMapping();
            context.AddSource($"{classMapping.SourceClassName}To{classMapping.TargetClassName}.g.cs",
                SourceText.From(generatedMapping, Encoding.UTF8));
        }
    }

    private static MappingStrategyInternal GetMappingStrategy(SeparatedSyntaxList<AttributeArgumentSyntax>? arguments)
    {
        var strategy = arguments?
            .SingleOrDefault(arg => arg.NameEquals?.Name.ToString() == "MappingStrategy")
            ?.Expression as MemberAccessExpressionSyntax;

        return strategy?.Name.ToString() switch
        {
            "Constructor" => MappingStrategyInternal.Constructor,
            _ => MappingStrategyInternal.Setter
        };
    }

    private static bool IsExhaustive(SeparatedSyntaxList<AttributeArgumentSyntax>? arguments)
    {
        var exhaustiveArgument = arguments?
            .SingleOrDefault(arg => arg.NameEquals?.Name.ToString() == "Exhaustive")?
            .Expression as LiteralExpressionSyntax;

        return exhaustiveArgument?.Kind() == SyntaxKind.TrueLiteralExpression;
    }
}