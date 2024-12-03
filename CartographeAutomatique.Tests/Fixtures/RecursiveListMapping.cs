namespace CartographeAutomatique.Tests;

public class RecursiveListMapping : IFixture
{
    private const string Source =
        //language=csharp
        """
            using System.Collections.Generic;
            using CartographeAutomatique;

            namespace TestNamespace;

            [MapTo(typeof(B))]
            public class A
            {
                public List<List<List<string>>> Names { get; set; }
            }

            public class B
            {
                public List<string[][]> Names { get; set; }
            }
            """;

    private const string ExpectedMapping =
        //language=csharp
        """
            // <auto-generated/>
            namespace TestNamespace;

            public static partial class AMappingExtensions
            {
                public static TestNamespace.B MapToB(this TestNamespace.A source) =>
                   	new()
            		{
            			Names = source.Names.Select(t => t.Select(t => t.Select(t => t).ToArray()).ToArray()).ToList()
            		};
            }
            """;

    public SourceGenerationAssertion GetAssertion() =>
        new(Source, [new SourceGenerationOutput("AToB", ExpectedMapping)]);
}
