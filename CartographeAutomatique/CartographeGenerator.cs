using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(
                (s, _) => s is TypeDeclarationSyntax,
                (ctx, _) => GetClassDeclarationForSourceGen(ctx))
            .Where(mappings => mappings.Count > 0);

        var incrementalValueProvider = provider.Collect();

        context.RegisterSourceOutput(context.CompilationProvider.Combine(incrementalValueProvider),
            (ctx, t) => GenerateCode(ctx, t.Left, t.Right));
    }

    private static List<ClassMapping> GetClassDeclarationForSourceGen(GeneratorSyntaxContext context)
    {
        return context.Node switch
        {
            ClassDeclarationSyntax classDeclaration => PopulateClassMapping(context, classDeclaration),
            RecordDeclarationSyntax recordDeclaration => PopulateRecordMapping(context, recordDeclaration),
            _ => throw new NotImplementedException()
        };
    }

    private static List<ClassMapping> PopulateRecordMapping(GeneratorSyntaxContext context,
        RecordDeclarationSyntax recordDeclarationSyntax)
    {
        var classMappings = new List<ClassMapping>();

        foreach (var attributeListSyntax in recordDeclarationSyntax.AttributeLists)
            foreach (var arguments in
                     from attributeSyntax in attributeListSyntax.Attributes
                     where attributeSyntax.Name.ToString() != $"{Constant.Namespace}.{Constant.AttributeName}"
                     select attributeSyntax.ArgumentList?.Arguments)
            {
                var targetTypeArgument = arguments?.FirstOrDefault();

                var exhaustiveArgument = arguments?
                    .SingleOrDefault(arg => arg.NameEquals?.Name.ToString() == "Exhaustive")?
                    .Expression as LiteralExpressionSyntax;

                var mappingStrategyArgument = arguments?
                    .SingleOrDefault(arg => arg.NameEquals?.Name.ToString() == "MappingStrategy")
                    ?.Expression as MemberAccessExpressionSyntax;

                var strategy = mappingStrategyArgument?.Name.ToString() switch
                {
                    "Constructor" => MappingStrategyInternal.Constructor,
                    _ => MappingStrategyInternal.Setter,
                };

                var exhaustive = exhaustiveArgument?.Kind() == SyntaxKind.TrueLiteralExpression;

                if (targetTypeArgument?.Expression is not TypeOfExpressionSyntax typeOfExpressionSyntax) continue;

                var targetIdentifierName = typeOfExpressionSyntax.Type as IdentifierNameSyntax;
                var targetSymbolInfo = ModelExtensions.GetSymbolInfo(context.SemanticModel, targetIdentifierName!);
                var targetTypeSyntax = targetSymbolInfo.TypeDeclarationSyntaxFromSymbolInfo();

                if (targetTypeSyntax is null) continue;

                classMappings.Add(
                    new ClassMapping(
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

    private static List<ClassMapping> PopulateClassMapping(GeneratorSyntaxContext context,
        ClassDeclarationSyntax classDeclarationSyntax)
    {
        var classMappings = new List<ClassMapping>();

        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
            foreach (var arguments in
                     from attributeSyntax in attributeListSyntax.Attributes
                     where attributeSyntax.Name.ToString() != $"{Constant.Namespace}.{Constant.AttributeName}"
                     select attributeSyntax.ArgumentList?.Arguments)
            {
                var targetTypeArgument = arguments?.FirstOrDefault();

                var exhaustiveArgument = arguments?
                    .SingleOrDefault(arg => arg.NameEquals?.Name.ToString() == "Exhaustive")?
                    .Expression as LiteralExpressionSyntax;

                var mappingStrategyArgument = arguments?
                    .SingleOrDefault(arg => arg.NameEquals?.Name.ToString() == "MappingStrategy")
                    ?.Expression as MemberAccessExpressionSyntax;

                var strategy = mappingStrategyArgument?.Name.ToString() switch
                {
                    "Constructor" => MappingStrategyInternal.Constructor,
                    _ => MappingStrategyInternal.Setter,
                };

                var exhaustive = exhaustiveArgument?.Kind() == SyntaxKind.TrueLiteralExpression;

                if (targetTypeArgument?.Expression is not TypeOfExpressionSyntax typeOfExpressionSyntax) continue;

                var targetIdentifierName = typeOfExpressionSyntax.Type as IdentifierNameSyntax;
                var targetSymbolInfo = ModelExtensions.GetSymbolInfo(context.SemanticModel, targetIdentifierName!);
                var targetTypeSyntax = targetSymbolInfo.TypeDeclarationSyntaxFromSymbolInfo();

                if (targetTypeSyntax is null) continue;

                classMappings.Add(
                    new ClassMapping(
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

    private void GenerateCode(SourceProductionContext context, Compilation compilation,
        ImmutableArray<List<ClassMapping>> classDeclarations)
    {
        var classMappings = classDeclarations.SelectMany(t => t)
            .ToList();

        foreach (var classMapping in classMappings)
        {
            var generatedMapping = classMapping.GenerateMapping();
            context.AddSource($"{classMapping.SourceClassName}To{classMapping.TargetClassName}.g.cs",
                SourceText.From(generatedMapping, Encoding.UTF8));
        }
    }
}