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

    public static ITypeSymbol? GetPropertyTypeSymbol(this TypeSyntax type, GeneratorSyntaxContext context)
        => context.SemanticModel.GetTypeInfo(type).Type;

    public static ClassDeclarationSyntax? GetClassDeclarationForType(this ITypeSymbol targetTypeSymbol, GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration) return null;
        if (context.SemanticModel.GetDeclaredSymbol(classDeclaration) is ITypeSymbol declaredSymbol && SymbolEqualityComparer.Default.Equals(declaredSymbol, targetTypeSymbol))
        {
            return classDeclaration;
        }

        return null;
    }

    public static AttributeSyntax? GetMatchingTargetMappingAttribute(this MemberDeclarationSyntax memberDeclarationSyntax, string targetClassName) =>
        memberDeclarationSyntax
            .AttributeLists
            .SelectMany(x => x.Attributes)
            .Where(a => a.Name is IdentifierNameSyntax { Identifier.ValueText: "TargetMapping" })
            .SingleOrDefault(x =>
                x.ArgumentList != null && x.ArgumentList.Arguments
                    .Any(arg =>
                        arg.Expression is TypeOfExpressionSyntax { Type: IdentifierNameSyntax identifierNameSyntax } &&
                        identifierNameSyntax.Identifier.Text == targetClassName)
            );

    public static AttributeSyntax? GetMatchingTargetMappingAttribute(this ParameterSyntax parameter, string targetClassName) =>
        parameter
            .AttributeLists
            .SelectMany(x => x.Attributes)
            .Where(a => a.Name is IdentifierNameSyntax { Identifier.ValueText: "TargetMapping" })
            .SingleOrDefault(x =>
                x.ArgumentList != null && x.ArgumentList.Arguments
                    .Any(arg =>
                        arg.Expression is TypeOfExpressionSyntax { Type: IdentifierNameSyntax identifierNameSyntax } &&
                        identifierNameSyntax.Identifier.Text == targetClassName)
            );


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
}