using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                       && (arg.Value.Value as ISymbol).Name == targetClassName))
        };
    }

    private static string ImplicitPrimitiveConversionTo(
        this ITypeSymbol source,
        ITypeSymbol target,
        string sourceIdentifier
    )
    {
        if (target.SpecialType != SpecialType.None && source.SpecialType == target.SpecialType)
        {
            return sourceIdentifier;
        }

        return (source.SpecialType, target.SpecialType) switch
        {
            // TODO: Complete with relevant special types (AKA built-in types), all cases cannot be covered though
            (SpecialType.System_String, SpecialType.System_Single) =>
                $"Single.Parse({sourceIdentifier})",
            (SpecialType.System_String, SpecialType.System_Int16) =>
                $"Int16.Parse({sourceIdentifier})",
            (SpecialType.System_String, SpecialType.System_Int32) =>
                $"Int32.Parse({sourceIdentifier})",
            (SpecialType.System_String, SpecialType.System_Int64) =>
                $"Int64.Parse({sourceIdentifier})",
            (SpecialType.System_String, SpecialType.System_UInt16) =>
                $"Int16.Parse({sourceIdentifier})",
            (SpecialType.System_String, SpecialType.System_UInt32) =>
                $"UInt32.Parse({sourceIdentifier})",
            (SpecialType.System_String, SpecialType.System_UInt64) =>
                $"UInt64.Parse({sourceIdentifier})",

            (SpecialType.System_Single, SpecialType.System_String)
                or
                (SpecialType.System_Int16, SpecialType.System_String)
                or
                (SpecialType.System_Int32, SpecialType.System_String)
                or
                (SpecialType.System_Int64, SpecialType.System_String)
                or
                (SpecialType.System_UInt16, SpecialType.System_String)
                or
                (SpecialType.System_UInt32, SpecialType.System_String)
                or
                (SpecialType.System_String, SpecialType.System_Enum)
                or (SpecialType.System_UInt64, SpecialType.System_String) =>
                $"{sourceIdentifier}.ToString(System.Globalization.CultureInfo.InvariantCulture)",
            _ => $"{sourceIdentifier}.MapTo{target.Name}()",
        };
    }

    public static string? ImplicitConversionTo(
        this ITypeSymbol source,
        ITypeSymbol target,
        string sourceMemberAccess
    )
    {
        var targetType =
            Array1Symbol.FromTypeSymbol(target)
            ?? Collection1Symbol.FromTypeSymbol(target)
            ?? UnknownTypeSymbol.FromTypeSymbol(target);
        var sourceType =
            Array1Symbol.FromTypeSymbol(source)
            ?? Collection1Symbol.FromTypeSymbol(source)
            ?? UnknownTypeSymbol.FromTypeSymbol(source);

        if (targetType is null || sourceType is null)
            return null;
        if (targetType.SameAs(sourceType))
            return sourceMemberAccess;

        var converter = targetType.OuterTypeSymbol()?.Name switch
        {
            // TODO: make exhaustive by completing with relevant ICollection'1
            _ when targetType.IsArray() => "ToArray()",
            "List" => "ToList()",
            "ImmutableList" => "ToImmutableList()",
            _ => null,
        };

        if (converter is null)
        {
            return source.ImplicitPrimitiveConversionTo(target, sourceMemberAccess);
        }

        var collectionImplicitConversion = sourceType
            .InnerTypeSymbol()
            .ImplicitConversionTo(targetType.InnerTypeSymbol(), "t");
        return $"{sourceMemberAccess}.Select(t => {collectionImplicitConversion}).{converter}";
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
}