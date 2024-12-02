namespace CartographeAutomatique.Tests;

public class ClassToRecord : IFixture
{
    private const string Source =
        //language=csharp
        """
        namespace TestNamespace;
        using Generators;
        
        internal record Car(string Color, string Brand);

        [MapTo(typeof(Car)]
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

        public static partial class TruckMappingExtensions
        {
            public static TestNamespace.Car MapToCar(this Truck source) =>
               	new(Color: source.Color,Brand: source.Brand);
        }
        """;

    public SourceGenerationAssertion GetAssertion() =>
        new(Source,
            [new("TruckToCar", Expected)]);
}