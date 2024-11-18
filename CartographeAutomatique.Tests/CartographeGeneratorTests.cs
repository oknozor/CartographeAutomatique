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
    public void GenerateSimpleMapping()
    {
        var assertion = new SourceGenerationAssertion(SourceFixtures.Vector3ClassText, [
            new("Vector3ToPoint", SourceFixtures.ExpectedGeneratedCodeVector3),
        ]);

        CodeGenerationAssertion(assertion);
    }

    [Fact]
    public void GenerateNonExhaustiveMapping()
    {
        var assertion = new SourceGenerationAssertion(SourceFixtures.NonExhaustiveMapping, [
            new("Vector3ToPoint2", SourceFixtures.ExpectedGeneratedCodeNonExhaustive),
        ]);

        CodeGenerationAssertion(assertion);
    }

    [Fact]
    public void GenerateMultipleMappingOnSameClass()
    {
        var assertion = new SourceGenerationAssertion(SourceFixtures.MultipleMappingAttributes, [
            new("Vector3ToPoint", SourceFixtures.ExpectedGeneratedCodeVector3),
            new("Vector3ToPoint2", SourceFixtures.ExpectedGeneratedCodeNonExhaustive)
        ]);

        CodeGenerationAssertion(assertion);
    }


    [Fact]
    public void GenerateRecursiveMappingCode()
    {
        var assertion = new SourceGenerationAssertion(SourceFixtures.RecursiveMappings, [
            new("Line3ToLine2", SourceFixtures.RecursiveMappingsExpectedLineMapping),
            new("Point3ToPoint2", SourceFixtures.RecursiveMappingsExpectedPointMapping)
        ]);

        CodeGenerationAssertion(assertion);
    }

    [Fact]
    public void GenerateFieldMappingCode()
    {
        var assertion =
            new SourceGenerationAssertion(SourceFixtures.FieldMappings,
                [new("AuthorToSongWriter", SourceFixtures.ExpectedFieldMappings)]);
        CodeGenerationAssertion(assertion);
    }


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