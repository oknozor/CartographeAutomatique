using Microsoft.CodeAnalysis;

namespace CartographeAutomatique;

public record IntermediateDiagnostic(string Message)
{
    internal Diagnostic ToDiagnostic() =>
        Diagnostic.Create(new DiagnosticDescriptor(
            "CA8_GEN",
            "CArtograpeGeneratorError",
            Message,
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        ), Location.None);
}