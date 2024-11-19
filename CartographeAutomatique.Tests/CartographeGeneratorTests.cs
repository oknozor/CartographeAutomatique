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
        var assertion = new SourceGenerationAssertion(Fixtures.Vector3ClassText, [
            new("Vector3ToPoint", Fixtures.ExpectedGeneratedCodeVector3),
        ]);

        CodeGenerationAssertion(assertion);
    }

    [Fact]
    public void GenerateNonExhaustiveMapping()
    {
        var assertion = new SourceGenerationAssertion(Fixtures.NonExhaustiveMapping, [
            new("Vector3ToPoint2", Fixtures.ExpectedGeneratedCodeNonExhaustive),
        ]);

        CodeGenerationAssertion(assertion);
    }

    [Fact]
    public void GenerateMultipleMappingOnSameClass()
    {
        var assertion = new SourceGenerationAssertion(Fixtures.MultipleMappingAttributes, [
            new("Vector3ToPoint", Fixtures.ExpectedGeneratedCodeVector3),
            new("Vector3ToPoint2", Fixtures.ExpectedGeneratedCodeNonExhaustive)
        ]);

        CodeGenerationAssertion(assertion);
    }


    [Fact]
    public void GenerateRecursiveMappingCode()
    {
        var assertion = new SourceGenerationAssertion(Fixtures.RecursiveMappings, [
            new("Line3ToLine2", Fixtures.RecursiveMappingsExpectedLineMapping),
            new("Point3ToPoint2", Fixtures.RecursiveMappingsExpectedPointMapping)
        ]);

        CodeGenerationAssertion(assertion);
    }

    [Fact]
    public void GenerateFieldMappingCode()
    {
        var assertion =
            new SourceGenerationAssertion(Fixtures.FieldMappings,
                [new("AuthorToSongWriter", Fixtures.ExpectedFieldMappings)]);
        CodeGenerationAssertion(assertion);
    }

    [Fact]
    public void GenerateRecordToClassMappingCode()
    {
        var assertion =
            new SourceGenerationAssertion(Fixtures.RecordToClassMapping,
                [new("CarToTruck", Fixtures.ExpectedSimpleRecordToClassMapping)]);
        CodeGenerationAssertion(assertion);
    }

    [Fact]
    public void GenerateClassToRecordMappingCode()
    {
        var assertion =
            new SourceGenerationAssertion(Fixtures.ClassToRecordMapping,
                [new("TruckToCar", Fixtures.ExpectedClassToRecord)]);
        CodeGenerationAssertion(assertion);
    }
    [Fact]
    public void GenerateRecordToRecordMappingCode()
    {
        var assertion =
            new SourceGenerationAssertion(Fixtures.RecordToRecord,
                [new("ColorToHexColor", Fixtures.ExpectedRecordToRecordMappings)]);
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