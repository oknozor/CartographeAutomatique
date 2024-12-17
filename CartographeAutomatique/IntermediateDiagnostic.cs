using Microsoft.CodeAnalysis;

namespace CartographeAutomatique;

public record IntermediateDiagnostic(string message)
{
    internal Diagnostic ToDiagnostic() =>
        Diagnostic.Create(new DiagnosticDescriptor(
            "CA8_GEN",
            "CArtograpeGeneratorError",
            message,
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        ), Location.None);
}