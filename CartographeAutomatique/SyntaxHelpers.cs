using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CartographeAutomatique;

public static class SyntaxHelpers
{
    public static bool IsPrimitiveType(this ITypeSymbol typeSymbol)
    {
        return typeSymbol.SpecialType switch
        {
            SpecialType.System_Boolean or
                SpecialType.System_Byte or
                SpecialType.System_SByte or
                SpecialType.System_Int16 or
                SpecialType.System_UInt16 or
                SpecialType.System_Int32 or
                SpecialType.System_UInt32 or
                SpecialType.System_Int64 or
                SpecialType.System_UInt64 or
                SpecialType.System_Single or
                SpecialType.System_Double or
                SpecialType.System_Char or
                SpecialType.System_String => true,
            _ => false
        };
    }

    public static INamedTypeSymbol? GetPropertyTypeSymbol(this TypeSyntax? type, GeneratorSyntaxContext context)
    {
        var propertyTypeSymbol = context.SemanticModel.GetTypeInfo(type!).Type;

        if (propertyTypeSymbol is INamedTypeSymbol namedTypeSymbol)
        {
            return namedTypeSymbol;
        }

        return null;
    }

    public static AttributeSyntax? GetMatchingMappingAttribute(this MemberDeclarationSyntax memberDeclarationSyntax, string targetClassName)
    {
        var targetMappingCandidates = memberDeclarationSyntax
            .AttributeLists
            .SelectMany(x => x.Attributes)
            .Where(a => a.Name is IdentifierNameSyntax { Identifier.ValueText: "Mapping" })
            .ToList();

        if (targetMappingCandidates.Count == 0) return null;

        if (targetMappingCandidates.Count == 1)
        {
            return targetMappingCandidates.FirstOrDefault(x => x.ArgumentList != null);
        }

        return targetMappingCandidates
            .SingleOrDefault(x =>
                x.ArgumentList != null && x.ArgumentList.Arguments
                    .Any(arg =>
                        arg.Expression is TypeOfExpressionSyntax { Type: IdentifierNameSyntax identifierNameSyntax } &&
                        identifierNameSyntax.Identifier.Text == targetClassName)
            );
    }

    public static AttributeSyntax? GetMatchingMappingAttribute(this ParameterSyntax parameter, string targetClassName)
    {
        var targetMappingCandidates = parameter
            .AttributeLists
            .SelectMany(x => x.Attributes)
            .Where(a => a.Name is IdentifierNameSyntax { Identifier.ValueText: "Mapping" })
            .ToList();

        if (targetMappingCandidates.Count == 0) return null;

        if (targetMappingCandidates.Count == 1)
        {
            return targetMappingCandidates.FirstOrDefault(x => x.ArgumentList != null);
        }

        return targetMappingCandidates
            .SingleOrDefault(x => x.ArgumentList != null && x.ArgumentList.Arguments
                .Any(arg =>
                    arg.Expression is TypeOfExpressionSyntax
                    {
                        Type: IdentifierNameSyntax identifierNameSyntax
                    } &&
                    identifierNameSyntax.Identifier.Text == targetClassName)
            );
    }

    public static TypeDeclarationSyntax? TypeDeclarationSyntaxFromSymbolInfo(this SymbolInfo targetSymbolInfo)
    {
        if (targetSymbolInfo.Symbol is not INamedTypeSymbol
            {
                DeclaringSyntaxReferences.Length: > 0
            } targetNamedType) return null;

        var node = targetNamedType.DeclaringSyntaxReferences[0].GetSyntax();
        if (node is TypeDeclarationSyntax typeDeclarationSyntax)
        {
            return typeDeclarationSyntax;
        }

        return null;
    }

    // Note that usage of `<ImplicitUsings>enable</ImplicitUsings>`in csproj prevent from directly looking up
    // ICollection implementors. That's why we only match against the containing namespace
    public static bool IsCollection1(this INamedTypeSymbol? targetSymbol) =>
        targetSymbol?.ContainingNamespace.ToDisplayString() == "System.Collections.Generic" && targetSymbol.TypeArguments.Length == 1;

    public static ITypeSymbol FirstGenericParameterName(this INamedTypeSymbol targetSymbol) => targetSymbol.TypeArguments.First();
}