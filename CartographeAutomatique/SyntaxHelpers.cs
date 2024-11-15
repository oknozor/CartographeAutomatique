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

    public static ITypeSymbol? GetPropertyTypeSymbol(this PropertyDeclarationSyntax propertyDeclaration, GeneratorSyntaxContext context)
    {
        var propertySymbol = context.SemanticModel.GetDeclaredSymbol(propertyDeclaration) as IPropertySymbol;
        return propertySymbol?.Type;
    }

    public static ClassDeclarationSyntax? GetClassDeclarationForType(this ITypeSymbol targetTypeSymbol, GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration) return null;
        if (context.SemanticModel.GetDeclaredSymbol(classDeclaration) is ITypeSymbol declaredSymbol && SymbolEqualityComparer.Default.Equals(declaredSymbol, targetTypeSymbol))
        {
            return classDeclaration;
        }

        return null;
    }


}