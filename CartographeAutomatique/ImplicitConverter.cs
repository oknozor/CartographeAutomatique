using System.Linq;
using Microsoft.CodeAnalysis;
using static Microsoft.CodeAnalysis.SpecialType;

namespace CartographeAutomatique;

public static class ImplicitConverter
{
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
            "ImmutableArray" => "ToImmutableArray()",
            _ => null
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

    private static string ImplicitPrimitiveConversionTo(
    this ITypeSymbol source,
    ITypeSymbol target,
    string sourceIdentifier
)
    {
        var notSpecialTypeTarget = target.SpecialType == None;
        var isSameType = target.FullyQualifiedName() == source.FullyQualifiedName();

        if (isSameType)
            return sourceIdentifier;

        if (notSpecialTypeTarget)
        {
            var constructorWithSingleArgument
                = (target.OriginalDefinition as INamedTypeSymbol)?.Constructors
                .FirstOrDefault(constructor => constructor.Parameters.Length == 1);

            if (constructorWithSingleArgument is not null)
            {
                var arg = constructorWithSingleArgument.Parameters.First();
                var sameType = arg.Type.Name == source.Name
                               && arg.Type.ContainingNamespace.Name == source.ContainingNamespace.Name;

                if (sameType)
                {
                    return $"new {target.FullyQualifiedName()}({sourceIdentifier})";
                }
            }
        }

        if (!source.IsRecord)
        {
            var propertySymbols = source.GetMembers()
                .OfType<IPropertySymbol>();

            var isTypeWithSingleField = propertySymbols.Count() == 1;
            if (isTypeWithSingleField)
            {
                var propertySymbol = propertySymbols.First();
                var propertySymbolType = propertySymbol.Type;
                var sameType = propertySymbolType.FullyQualifiedName() == target.FullyQualifiedName();
                if (sameType)
                    return $"{sourceIdentifier}.{propertySymbol.Name}";
            }
        }

        if (source.IsRecord)
        {
            var recordWithSingleMember
                = (source.OriginalDefinition as INamedTypeSymbol)?.Constructors
                .FirstOrDefault(constructor => constructor.Parameters.Length == 1);

            if (recordWithSingleMember is not null)
            {
                var arg = recordWithSingleMember.Parameters.First();
                var sameType = arg.Type.Name == target.Name
                               && arg.Type.ContainingNamespace.Name == target.ContainingNamespace.Name;

                if (sameType)
                {
                    return $"{sourceIdentifier}.{arg.Name}";
                }
            }
        }

        // This is probably a misuse of special types but special type enum is never set
        var targetSpecialType = target.TypeKind == TypeKind.Enum ? System_Enum : target.SpecialType;
        var sourceSpecialType = source.TypeKind == TypeKind.Enum ? System_Enum : source.SpecialType;

        return (sourceSpecialType, targetSpecialType) switch
        {
            // TODO: Complete with relevant special types (AKA built-in types), all cases cannot be covered though
            (System_String, System_Single) =>
                $"Single.Parse({sourceIdentifier})",
            (System_String, System_Int16) =>
                $"Int16.Parse({sourceIdentifier})",
            (System_String, System_Int32) =>
                $"Int32.Parse({sourceIdentifier})",
            (System_String, System_Int64) =>
                $"Int64.Parse({sourceIdentifier})",
            (System_String, System_UInt16) =>
                $"Int16.Parse({sourceIdentifier})",
            (System_String, System_UInt32) =>
                $"UInt32.Parse({sourceIdentifier})",
            (System_String, System_UInt64) =>
                $"UInt64.Parse({sourceIdentifier})",
            (System_String, System_Enum) =>
                $"Enum.Parse<{target.FullyQualifiedName()}>({sourceIdentifier})",
            (System_Enum, System_String) =>
                $"{sourceIdentifier}.ToString()",
            (System_Int16, System_Enum)
                or (System_Int32, System_Enum)
                or (System_Int64, System_Enum)
                => $"({target.FullyQualifiedName()}){sourceIdentifier}",
            (System_Enum, System_Int16) => $"(short){sourceIdentifier}",
            (System_Enum, System_Int32) => $"(int){sourceIdentifier}",
            (System_Enum, System_Int64) => $"(long){sourceIdentifier}",
            (System_Single, System_String)
                or
                (System_Int16, System_String)
                or
                (System_Int32, System_String)
                or
                (System_Int64, System_String)
                or
                (System_UInt16, System_String)
                or
                (System_UInt32, System_String)
                or
                (System_String, System_Enum)
                or (System_UInt64, System_String) =>
                $"{sourceIdentifier}.ToString(System.Globalization.CultureInfo.InvariantCulture)",
            _ => $"{sourceIdentifier}.MapTo{target.Name}()"
        };
    }
}