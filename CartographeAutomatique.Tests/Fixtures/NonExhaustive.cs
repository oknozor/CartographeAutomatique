namespace CartographeAutomatique.Tests;

public class NonExhaustive : IFixture
{
    private const string Source =
        //language=csharp
        """
            namespace TestNamespace;

            [CartographeAutomatique.MapTo(typeof(Point2), Exhaustive = false]
            public class Vector3
            {
                public float X { get; set; }
                public float Y { get; set; }
                public float Z { get; set; }
            }

            public class Point2
            {
                public float X { get; set; }
                public float Y { get; set; }
            };
            """;

    private const string Expected =
        //language=csharp
        """
            // <auto-generated/>
            namespace TestNamespace;

            public static partial class Vector3MappingExtensions
            {
                public static TestNamespace.Point2 MapToPoint2(this TestNamespace.Vector3 source) =>
                   	new()
            		{
            			X = source.X,
            			Y = source.Y
            		};
            }
            """;

    public SourceGenerationAssertion GetAssertion() =>
        new(Source, [new SourceGenerationOutput("Vector3ToPoint2", Expected)]);
}
