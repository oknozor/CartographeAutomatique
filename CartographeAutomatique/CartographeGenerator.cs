using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CartographeAutomatique;

enum MappingKind
{
    MapTo,
    MapFrom
}

[Generator]
public class CartographeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "MapToAttribute.g.cs",
            SourceText.From(Constant.AttributeSourceCode, Encoding.UTF8)));

        var mapToPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: "Generators.MapToAttribute",
            predicate: static (syntaxNode, _) => syntaxNode is TypeDeclarationSyntax,
            transform: static (context, _) => PopulateTypeMapping(context, MappingKind.MapTo));

        var mapFromPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: "Generators.MapFromAttribute",
            predicate: static (syntaxNode, _) => syntaxNode is TypeDeclarationSyntax,
            transform: static (context, _) => PopulateTypeMapping(context, MappingKind.MapFrom));

        context.RegisterSourceOutput(mapToPipeline,
            (productionContext, list) => GenerateCode(productionContext, list, MappingKind.MapTo));

        context.RegisterSourceOutput(mapFromPipeline,
            (productionContext, list) => GenerateCode(productionContext, list, MappingKind.MapTo));
    }

    private static List<TypeMapping> PopulateTypeMapping(GeneratorAttributeSyntaxContext context, MappingKind mappingKind)
    {
        var typeDeclarationSyntax = (context.TargetNode as TypeDeclarationSyntax)!;
        var classMappings = new List<TypeMapping>();

        foreach (var attributeListSyntax in typeDeclarationSyntax.AttributeLists)
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
                        mappingKind,
                        typeDeclarationSyntax,
                        targetTypeSyntax,
                        exhaustive,
                        strategy,
                        context
                    )
                );
            }

        return classMappings;
    }

    private static void GenerateCode(SourceProductionContext context, List<TypeMapping> classMappings, MappingKind mappingKind)
    {
        foreach (var classMapping in classMappings)
        {
            var generatedMapping = classMapping.GenerateMapping();
            var sourceFileName = mappingKind switch
            {
                MappingKind.MapTo => $"{classMapping.SourceClassName}To{classMapping.TargetClassName}.g.cs",
                MappingKind.MapFrom => $"{classMapping.SourceClassName}From{classMapping.TargetClassName}.g.cs",
                _ => throw new ArgumentOutOfRangeException(nameof(mappingKind), mappingKind, null)
            };

            context.AddSource(sourceFileName, SourceText.From(generatedMapping, Encoding.UTF8));
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