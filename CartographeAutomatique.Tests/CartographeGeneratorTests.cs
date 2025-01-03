using System.Collections.Generic;
using System.Collections.Immutable;
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
    public void GenerateSimpleMapping() =>
        CodeGenerationAssertion(new ClassToClass().GetAssertion());

    [Fact]
    public void GenerateNonExhaustiveMapping() =>
        CodeGenerationAssertion(new NonExhaustive().GetAssertion());

    [Fact]
    public void GenerateMultipleMappingOnSameClass() =>
        CodeGenerationAssertion(new MutlipleMapping().GetAssertion());

    [Fact]
    public void GenerateRecursiveMappingCode() =>
        CodeGenerationAssertion(new Recursive().GetAssertion());

    [Fact]
    public void GenerateFieldMappingCode() =>
        CodeGenerationAssertion(new FieldMapping().GetAssertion());

    [Fact]
    public void GenerateImplicitFieldMappingCode() =>
        CodeGenerationAssertion(new FieldMappingImplicit().GetAssertion());

    [Fact]
    public void GenerateRecordToClassMappingCode() =>
        CodeGenerationAssertion(new RecordToClass().GetAssertion());

    [Fact]
    public void GenerateClassToRecordMappingCode() =>
        CodeGenerationAssertion(new ClassToRecord().GetAssertion());

    [Fact]
    public void GenerateRecordToRecordMappingCode() =>
        CodeGenerationAssertion(new RecordToRecord().GetAssertion());

    [Fact]
    public void GenerateMultipleFieldMappings() =>
        CodeGenerationAssertion(new MultipleFieldMapping().GetAssertion());

    [Fact]
    public void GenerateConstructorClassMapping() =>
        CodeGenerationAssertion(new ConstructorClassToClass().GetAssertion());

    [Fact]
    public void GenerateConstructorCustomMethodMapping() =>
        CodeGenerationAssertion(new CustomMethodMapping().GetAssertion());

    [Fact]
    public void GenerateImplicitMapping() =>
        CodeGenerationAssertion(new ImplicitMapping().GetAssertion());

    [Fact]
    public void GenerateListToListMapping() =>
        CodeGenerationAssertion(new ListToList().GetAssertion());

    [Fact]
    public void GenerateListToArrayMapping() =>
        CodeGenerationAssertion(new ListToArrayMapping().GetAssertion());

    [Fact]
    public void GenerateRecursiveListMapping() =>
        CodeGenerationAssertion(new RecursiveListMapping().GetAssertion());

    [Fact]
    public void GenerateFromMapping() =>
        CodeGenerationAssertion(new SimpleFromClassMapping().GetAssertion());


    [Fact]
    public void GenerateDocSample() =>
        CodeGenerationAssertion(new DocSample().GetAssertion());

    [Fact]
    public void GenerateNestedClass() =>
        CodeGenerationAssertion(new NestedClass().GetAssertion());

    [Fact]
    public void GenerateSingleArgConstructorPropertyMapping() =>
        CodeGenerationAssertion(new PropertyWithSingleArgConstructor().GetAssertion());

    [Fact]
    public void GenerateSingleSourceTypePropertyMapping() =>
        CodeGenerationAssertion(new SourceTypeSingleProperty().GetAssertion());

    [Fact]
    public void GenerateRecordSourceSingleTypeMemberMapping() =>
        CodeGenerationAssertion(new RecordSourceTypeSingleMember().GetAssertion());

    [Fact]
    public void EnumImplicitConversionMapping() =>
        CodeGenerationAssertion(new EnumImplicitConversions().GetAssertion());

    private static void CodeGenerationAssertion(SourceGenerationAssertion assertion)
    {
        var generator = new CartographeGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);

        var referenceComp = CSharpCompilation.Create("test",
            references: Basic.Reference.Assemblies.Net80.References.All, syntaxTrees:
            [
                CSharpSyntaxTree.ParseText("""
                                           namespace TestNamespace
                                           {
                                               public class Point
                                               {
                                                   public float X { get; set; }
                                                   public float Y { get; set; }
                                                   public float Z { get; set; }
                                               }
                                           }
                                           """)
            ]);

        var compilation = CSharpCompilation.Create(
            nameof(CartographeGeneratorTests),
            [CSharpSyntaxTree.ParseText(assertion.InputSource)],
            ImmutableArray<MetadataReference>.CastUp(Basic.Reference.Assemblies.Net80.References.All)
                .Add(referenceComp.ToMetadataReference())
        );

        var runResult = driver.RunGenerators(compilation).GetRunResult();

        foreach (var sourceGenerationOutput in assertion.Outputs)
        {
            var actual = runResult
                .GeneratedTrees.Single(t =>
                    t.FilePath.EndsWith($"{sourceGenerationOutput.GeneratedFileName}.g.cs")
                )
                .GetText()
                .ToString();

            Assert.Equal(
                sourceGenerationOutput.ExpectedSource,
                actual,
                ignoreLineEndingDifferences: true
            );
        }
    }
}


