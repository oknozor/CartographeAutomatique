namespace CartographeAutomatique.Tests;

public class DocSample : IFixture
{
    private const string Source =
        //language=csharp
        """
        using CartographeAutomatique;
        namespace TestNamespace;
        
        [MapTo(typeof(Car), MappingStrategy = MappingStrategy.Constructor)]
        [MapTo(typeof(Bike), Exhaustive = false)]
        public class Vehicle
        {
            [Mapping(TargetType = typeof(Car), TargetField = "CarColor")]
            [Mapping(TargetType = typeof(Bike), TargetField = "BikePaint")]
            public required string Color { get; set; }
        
            public required string Brand { get; set; }
        
            [Mapping(
                With = "Vehicle.ConvertKilometersToMiles",
                TargetType = typeof(Car),
                TargetField = "MilesDriven")
            ]
            public int KilometersDriven { get; set; }
        
            public static int ConvertKilometersToMiles(int kilometers) => (int)(kilometers * 0.621371);
        }
        
        public class Car
        {
            public string CarColor { get; }
            public string Brand { get; }
            public int MilesDriven { get; }
        
            public Car(string carColor, string brand, int milesDriven)
            {
                CarColor = carColor;
                Brand = brand;
                MilesDriven = milesDriven;
            }
        }
        
        public class Bike
        {
            public required string BikePaint { get; set; }
            public required string Brand { get; set; }
        }
        """;

    private const string CarMap =
        //language=csharp
        """
        // <auto-generated/>
        namespace TestNamespace;

        public static partial class VehicleMappingExtensions
        {
            public static TestNamespace.Car MapToCar(this TestNamespace.Vehicle source) =>
               	new(carColor: source.Color,brand: source.Brand,milesDriven: Vehicle.ConvertKilometersToMiles(source.KilometersDriven));
        }
        """;

    private const string BikeMap =
        //language=csharp
        """
        // <auto-generated/>
        namespace TestNamespace;
        
        public static partial class VehicleMappingExtensions
        {
            public static TestNamespace.Bike MapToBike(this TestNamespace.Vehicle source) =>
               	new()
        		{
        			BikePaint = source.Color,
        			Brand = source.Brand
        		};
        }
        """;


    public SourceGenerationAssertion GetAssertion() =>
        new(Source, [
            new SourceGenerationOutput("TestNamespace_VehicleToTestNamespace_Bike", BikeMap),
            new SourceGenerationOutput("TestNamespace_VehicleToTestNamespace_Car", CarMap)
        ]);
}