using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CartographeAutomatique;

internal class PropertyOrParameter
{
    private readonly AttributeSyntax? Attribute;

    public PropertyOrParameter(TypeSyntax type, string identifier, AttributeSyntax? attribute)
    {
        Attribute = attribute;
        Type = type;
        Identifier = identifier;
    }

    public PropertyOrParameter(TypeSyntax type, string identifier)
    {
        Type = type;
        Identifier = identifier;
    }

    public TypeSyntax Type { get; }
    public string Identifier { get; }

    public AttributeArgumentSyntax? TargetField() => Attribute?.ArgumentList?
        .Arguments
        .First(arg => arg.NameEquals?.Name.ToString() == "TargetField");
}