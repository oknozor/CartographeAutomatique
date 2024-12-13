using System;

namespace CartographeAutomatique.Tests;

public class ImplicitMapping
{
    private const string Source =
        //language=csharp
        """
            namespace TestNamespace;

            [CartographeAutomatique.MapTo(typeof(Point))]
            public partial class Vector3
            {
                public string X { get; set; }
                public string Y { get; set; }
                public float Z { get; set; }
            };

            public partial class Point
            {

                public float X { get; set; }
                public int Y { get; set; }
                public string Z { get; set; }
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
            			X = Single.Parse(source.X),
            			Y = Int32.Parse(source.Y),
            			Z = source.Z.ToString(System.Globalization.CultureInfo.InvariantCulture)
            		};
            }
            """;

    public SourceGenerationAssertion GetAssertion()
    {
        return new SourceGenerationAssertion(
            Source,
            [new SourceGenerationOutput("TestNamespace_Vector3ToTestNamespace_Point", Expected)]
        );
    }
}
