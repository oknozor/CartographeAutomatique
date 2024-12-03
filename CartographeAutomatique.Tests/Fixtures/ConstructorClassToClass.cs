namespace CartographeAutomatique.Tests;

public class ConstructorClassToClass
{
    private const string Source =
        //language=csharp
        """
            namespace TestNamespace;
            using CartographeAutomatique;

            [MapTo(typeof(Point), MappingStrategy = MappingStrategy.Constructor)]
            public class Vector
            {
                [Mapping(TargetField = "x")] public float X { get; set; }
                [Mapping(TargetField = "y")] public float Y { get; set; }
                [Mapping(TargetField = "z")] public float Z { get; set; }
            }

            public class Point
            {
                public Point(float x, float y, float z)
                {
                    X = x;
                    Y = y;
                    Z = z;
                }

                public float X { get; set; }
                public float Y { get; set; }
                public float Z { get; set; }
            }
            """;

    private const string Expected =
        //language=csharp
        """
            // <auto-generated/>
            namespace TestNamespace;

            public static partial class VectorMappingExtensions
            {
                public static TestNamespace.Point MapToPoint(this TestNamespace.Vector source) =>
                   	new(x: source.X,y: source.Y,z: source.Z);
            }
            """;

    public SourceGenerationAssertion GetAssertion() =>
        new(Source, [new SourceGenerationOutput("VectorToPoint", Expected)]);
}
