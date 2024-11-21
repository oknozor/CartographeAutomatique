namespace CartographeAutomatique.Tests;

public class MultipleFieldMapping : IFixture
{
    private const string Source =
        //language=csharp
        """
        namespace TestNamespace;

        [MapTo(typeof(Truck))]
        [MapTo(typeof(Bike))]
        internal record Vehicle(
            [Mapping(TargetType = typeof(Truck), TargetField = "TruckColor")]
            [Mapping(TargetType = typeof(Bike), TargetField = "BikeColor")]
            string Color, 
            string Brand
        );

        internal class Truck
        {
            public string TruckColor { get; set; }
            public string Brand { get; set; }
        };

        internal class Bike
        {
            public string BikeColor { get; set; }
            public string Brand { get; set; }
        };
        """;

    private const string ExpectedTruckMapping =
        //language=csharp
        """
        // <auto-generated/>
        namespace TestNamespace;
        
        public static partial class VehicleMappingExtensions
        {
            public static TestNamespace.Truck MapToTruck(this Vehicle source) =>
               	new()
        		{
        			TruckColor = source.Color,
        			Brand = source.Brand
        		};
        }
        """;

    private const string ExpectedBikeMapping =
        //language=csharp
        """
        // <auto-generated/>
        namespace TestNamespace;

        public static partial class VehicleMappingExtensions
        {
            public static TestNamespace.Bike MapToBike(this Vehicle source) =>
               	new()
        		{
        			BikeColor = source.Color,
        			Brand = source.Brand
        		};
        }
        """;

    public SourceGenerationAssertion GetAssertion() =>
        new(Source, [
            new("VehicleToTruck", ExpectedTruckMapping),
            new("VehicleToBike", ExpectedBikeMapping)
        ]);
}