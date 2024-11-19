using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace CartographeAutomatique.Tests;

public record SourceGenerationAssertion(string InputSource, List<SourceGenerationOutput> Outputs);

public record SourceGenerationOutput(string GeneratedFileName, string ExpectedSource);

public class CartographeGeneratorTests
{
    [Fact]
    public void GenerateSimpleMapping() => CodeGenerationAssertion(new ClassToClass().GetAssertion());

    [Fact]
    public void GenerateNonExhaustiveMapping() => CodeGenerationAssertion(new NonExhaustive().GetAssertion());

    [Fact]
    public void GenerateMultipleMappingOnSameClass() => CodeGenerationAssertion(new MutlipleMapping().GetAssertion());
    
    [Fact]
    public void GenerateRecursiveMappingCode() => CodeGenerationAssertion(new Recursive().GetAssertion());

    [Fact]
    public void GenerateFieldMappingCode() => CodeGenerationAssertion(new FieldMapping().GetAssertion());

    [Fact]
    public void GenerateImplicitFieldMappingCode() => CodeGenerationAssertion(new FieldMappingImplicit().GetAssertion());

    [Fact]
    public void GenerateRecordToClassMappingCode() => CodeGenerationAssertion(new RecordToClass().GetAssertion());

    [Fact]
    public void GenerateClassToRecordMappingCode() => CodeGenerationAssertion(new ClassToRecord().GetAssertion());

    [Fact]
    public void GenerateRecordToRecordMappingCode() => CodeGenerationAssertion(new RecordToRecord().GetAssertion());

    [Fact]
    public void GenerateMultipleFieldMappings() => CodeGenerationAssertion(new MultipleFieldMapping().GetAssertion());

    private void CodeGenerationAssertion(SourceGenerationAssertion assertion)
    {
        var generator = new CartographeGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        var compilation = CSharpCompilation.Create(nameof(CartographeGeneratorTests),
            [CSharpSyntaxTree.ParseText(assertion.InputSource)],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            ]);

        var runResult = driver.RunGenerators(compilation).GetRunResult();

        foreach (var sourceGenerationOutput in assertion.Outputs)
        {
            var actual = runResult.GeneratedTrees
                .Single(t => t.FilePath.EndsWith($"{sourceGenerationOutput.GeneratedFileName}.g.cs"))
                .GetText()
                .ToString();

            Assert.Equal(sourceGenerationOutput.ExpectedSource, actual, ignoreLineEndingDifferences: true);
        }
    }
}