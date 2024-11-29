using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CartographeAutomatique;

public class PropertyOrParameter(
    TypeSyntax? type,
    string identifier,
    AttributeSyntax? attribute = null)
{
    public TypeSyntax? Type { get; } = type;
    public string Identifier { get; } = identifier;

    public AttributeArgumentSyntax? TargetField() => attribute?.ArgumentList?
        .Arguments
        .FirstOrDefault(arg => arg.NameEquals?.Name.ToString() == "TargetField");

    public AttributeArgumentSyntax? WithMethod() => attribute?.ArgumentList?
        .Arguments
        .FirstOrDefault(arg => arg.NameEquals?.Name.ToString() == "With");
}