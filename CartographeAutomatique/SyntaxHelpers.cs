using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace CartographeAutomatique;

public static class SyntaxHelpers
{
    public static AttributeData? GetMatchingMappingAttribute(
        this IEnumerable<AttributeData> attributes,
        string targetClassName
    )
    {
        var targetMappingCandidates = attributes
            .Where(a => a.AttributeClass!.Name == "MappingAttribute")
            .ToList();


        return targetMappingCandidates.Count switch
        {
            0 => null,
            1 => targetMappingCandidates.FirstOrDefault(),
            _ => targetMappingCandidates.SingleOrDefault(attr => attr.NamedArguments.Any(
                arg => arg.Key == "TargetType"
                       && (arg.Value.Value as ISymbol)?.Name == targetClassName))
        };
    }

    public static IArrayTypeSymbol? TryIntoArray(this ITypeSymbol? symbol)
    {
        if (symbol is IArrayTypeSymbol array)
        {
            return array;
        }

        return null;
    }

    // Note that usage of `<ImplicitUsings>enable</ImplicitUsings>`in csproj prevent from directly looking up
    // ICollection implementors. That's why we only match against the containing namespace
    public static bool IsCollection(this ITypeSymbol? targetSymbol) =>
        targetSymbol?.ContainingNamespace.ToDisplayString() == "System.Collections.Generic";

    public static ITypeSymbol? FirstGenericParameterName(this INamedTypeSymbol targetSymbol) =>
        targetSymbol.TypeArguments.FirstOrDefault();

    public static bool CamelCaseEquals(this string src, string other)
    {
        if (src.Length != other.Length)
            return false;

        return char.ToLowerInvariant(src[0]).Equals(char.ToLowerInvariant(other[0])) &&
               string.Equals(src.Substring(1), other.Substring(1), StringComparison.Ordinal);
    }

    internal static string FullyQualifiedName(this ITypeSymbol typeSymbol) =>
        $"{typeSymbol.ContainingNamespace.ToDisplayString()}.{typeSymbol.Name}";
}