using System.Linq;
using Microsoft.CodeAnalysis;

namespace CartographeAutomatique;

public class PropertyOrParameter(
    ITypeSymbol type,
    string identifier,
    AttributeData? attribute = null
)
{
    public ITypeSymbol Type { get; } = type;
    public string Identifier { get; } = identifier;

    public string? TargetField() => attribute?.NamedArguments
        .SingleOrDefault(arg => arg.Key == "TargetField")
        .Value
        .Value as string;

    public string? WithMethod() => attribute?.NamedArguments
        .SingleOrDefault(arg => arg.Key == "With")
        .Value
        .Value as string;
}