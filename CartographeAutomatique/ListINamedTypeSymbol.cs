using Microsoft.CodeAnalysis;

namespace CartographeAutomatique;

public interface ICollection1Symbol
{
    bool IsArray();
    ITypeSymbol InnerTypeSymbol();
    ITypeSymbol? OuterTypeSymbol();
    bool HasSameOuterType(ICollection1Symbol other);
    bool HasSameInnerType(ICollection1Symbol other);
}

public static class TypeExtensions
{
    public static bool SameAs(this ICollection1Symbol self, ICollection1Symbol other)
        => self.HasSameOuterType(other) && self.HasSameInnerType(other);
}

public class UnknownTypeSymbol(INamedTypeSymbol innerTypeSymbol) : ICollection1Symbol
{
    public static ICollection1Symbol? FromTypeSymbol(ITypeSymbol symbol)
    {
        if (symbol is INamedTypeSymbol namedTypeSymbol)
        {
            return new UnknownTypeSymbol(namedTypeSymbol);
        }

        return null;
    }

    public bool IsArray() => false;

    public ITypeSymbol InnerTypeSymbol() => innerTypeSymbol;

    public ITypeSymbol? OuterTypeSymbol() => null;

    public bool HasSameOuterType(ICollection1Symbol other) => false;

    public bool HasSameInnerType(ICollection1Symbol other) => other.InnerTypeSymbol().Name == InnerTypeSymbol().Name;
}

public class Array1Symbol(ITypeSymbol innerTypeSymbol) : ICollection1Symbol
{
    public static ICollection1Symbol? FromTypeSymbol(ITypeSymbol symbol)
    {
        var targetArray = symbol.TryIntoArray();

        return targetArray?.ElementType == null ? null : new Array1Symbol(targetArray.ElementType);
    }

    public bool IsArray() => true;

    public ITypeSymbol InnerTypeSymbol() => innerTypeSymbol;

    public ITypeSymbol? OuterTypeSymbol() => null;

    public bool HasSameOuterType(ICollection1Symbol other) => other.IsArray();

    public bool HasSameInnerType(ICollection1Symbol other) => InnerTypeSymbol().Name == other.InnerTypeSymbol().Name &&
                                                              InnerTypeSymbol().ContainingNamespace.Name ==
                                                              other.InnerTypeSymbol().ContainingNamespace.Name;
}

public class Collection1Symbol(ITypeSymbol? outerTypeSymbol, ITypeSymbol innerTypeSymbol) : ICollection1Symbol
{
    public static ICollection1Symbol? FromTypeSymbol(ITypeSymbol symbol)
    {
        if (symbol is not INamedTypeSymbol namedTypeSymbol) return null;
        var innerTypeSymbol = namedTypeSymbol.FirstGenericParameterName();

        if (innerTypeSymbol != null)
            return !namedTypeSymbol.IsCollection() ? null : new Collection1Symbol(namedTypeSymbol, innerTypeSymbol);

        return null;
    }

    public bool IsArray() => false;
    public ITypeSymbol InnerTypeSymbol() => innerTypeSymbol;
    public ITypeSymbol? OuterTypeSymbol() => outerTypeSymbol;

    public bool HasSameOuterType(ICollection1Symbol other) =>
        !other.IsArray()
        && OuterTypeSymbol()?.Name == other.OuterTypeSymbol()!.Name
        && OuterTypeSymbol()?.ContainingNamespace.Name == other.OuterTypeSymbol()!.ContainingNamespace.Name;

    public bool HasSameInnerType(ICollection1Symbol other)
        => InnerTypeSymbol().Name == other.InnerTypeSymbol().Name
           && InnerTypeSymbol().ContainingNamespace.Name == other.InnerTypeSymbol().ContainingNamespace.Name;
}