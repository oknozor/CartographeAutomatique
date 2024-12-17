using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CartographeAutomatique;

public enum MappingKind
{
    MapTo,
    MapFrom,
}

[Generator]
public class CartographeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        context.RegisterPostInitializationOutput(ctx =>
            ctx.AddSource(
                "MapToAttribute.g.cs",
                SourceText.From(Constant.AttributeSourceCode, Encoding.UTF8)
            )
        );

        var mapToPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: "CartographeAutomatique.MapToAttribute",
            predicate: static (syntaxNode, _) => true,
            transform: static (context, _) => PopulateTypeMapping(context, MappingKind.MapTo)
        );

        var mapFromPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: "CartographeAutomatique.MapFromAttribute",
            predicate: static (syntaxNode, _) => true,
            transform: static (context, _) => PopulateTypeMapping(context, MappingKind.MapFrom)
        );

        context.RegisterSourceOutput(
            mapToPipeline,
            (productionContext, list) => GenerateCode(productionContext, list, MappingKind.MapTo)
        );

        context.RegisterSourceOutput(
            mapFromPipeline,
            (productionContext, list) => GenerateCode(productionContext, list, MappingKind.MapTo)
        );
    }

    private static List<TypeMapping> PopulateTypeMapping(
        GeneratorAttributeSyntaxContext context,
        MappingKind mappingKind
    )
    {
        var classMappings = new List<TypeMapping>();
        var diagnostics = new List<IntermediateDiagnostic>();
        var attributes = context.Attributes
            .Where(attr => attr.AttributeClass!.Name is "MapToAttribute" or "MapFromAttribute");
        var sourceType = context.TargetSymbol as INamedTypeSymbol;

        foreach (var attribute in attributes)
        {
            var targetType = attribute.ConstructorArguments[0].Value as INamedTypeSymbol;
            var defaultExhaustiveness = (bool)attribute.AttributeConstructor!.Parameters[1].ExplicitDefaultValue!;
            var defaultMappingStrategy =
                (MappingStrategyInternal)(int)attribute.AttributeConstructor.Parameters[2].ExplicitDefaultValue!;

            var isExhaustiveArg = attribute.NamedArguments
                .SingleOrDefault(arg => arg.Key == "Exhaustive")
                .Value
                .Value;

            var isExhaustive = isExhaustiveArg is not null ? (bool)isExhaustiveArg : defaultExhaustiveness;

            var mappingStrategyArg = attribute.NamedArguments
                .SingleOrDefault(arg => arg.Key == "MappingStrategy")
                .Value
                .Value;

            var mappingStrategy = mappingStrategyArg is not null
                ? (MappingStrategyInternal)(int)mappingStrategyArg
                : defaultMappingStrategy;

            classMappings.Add(
                new TypeMapping(
                    mappingKind,
                    sourceType!,
                    targetType!,
                    isExhaustive,
                    mappingStrategy,
                    context,
                    diagnostics
                )
            );
        }

        return classMappings;
    }

    private static void GenerateCode(
        SourceProductionContext context,
        List<TypeMapping> classMappings,
        MappingKind mappingKind
    )
    {
        foreach (var classMapping in classMappings)
        {
            var generatedMapping = classMapping.GenerateMapping(context);
            var sourceFileName = mappingKind switch
            {
                MappingKind.MapTo =>
                    $"{classMapping.SourceNameSpace()}_{classMapping.SourceType().Name}To{classMapping.TargetNameSpace()}_{classMapping.TargetType().Name}.g.cs",
                MappingKind.MapFrom =>
                    $"{classMapping.SourceNameSpace()}_{classMapping.SourceType().Name}From{classMapping.TargetNameSpace()}_{classMapping.TargetType().Name}.g.cs",
                _ => throw new ArgumentOutOfRangeException(nameof(mappingKind), mappingKind, null),
            };

            context.AddSource(sourceFileName, SourceText.From(generatedMapping, Encoding.UTF8));
        }
    }
}