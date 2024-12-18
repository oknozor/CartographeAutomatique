namespace CartographeAutomatique.Tests;

public class RecordToClass : IFixture
{
    private const string Source =
        //language=csharp
        """
            namespace TestNamespace;
            using CartographeAutomatique;

            [MapTo(typeof(Truck))]
            internal record Car(string Color, string Brand);

            internal class Truck
            {
                public string Color { get; set; }
                public string Brand { get; set; }
            };
            """;
    private const string Expected =
        //language=csharp
        """
            // <auto-generated/>
            namespace TestNamespace;

            public static partial class CarMappingExtensions
            {
                public static TestNamespace.Truck MapToTruck(this TestNamespace.Car source) =>
                   	new()
            		{
            			Color = source.Color,
            			Brand = source.Brand
            		};
            }
            """;

    public SourceGenerationAssertion GetAssertion() => new(Source, [new("TestNamespace_CarToTestNamespace_Truck", Expected)]);
}
