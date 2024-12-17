namespace CartographeAutomatique.Tests;

public class ClassToClass : IFixture
{
    private const string Source =
        //language=csharp
        """
            namespace TestNamespace;

            [CartographeAutomatique.MapTo(typeof(TestNamespace.Point)]
            public partial class Vector3
            {

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

            public static partial class Vector3MappingExtensions
            {
                public static TestNamespace.Point MapToPoint(this TestNamespace.Vector3 source) =>
                   	new()
            		{
            			X = source.X,
            			Y = source.Y,
            			Z = source.Z
            		};
            }
            """;

    public SourceGenerationAssertion GetAssertion() =>
        new(Source, [new SourceGenerationOutput("TestNamespace_Vector3ToTestNamespace_Point", Expected)]);
}
