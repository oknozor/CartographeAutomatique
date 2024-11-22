using Generators;

namespace CartographeAutomatique.Samples;

[MapTo(typeof(Truck))]
[MapTo(typeof(Bike))]
public record Vehicle(
    [Mapping(TargetType = typeof(Truck), TargetField = "TruckColor")]
    [Mapping(TargetType = typeof(Bike), TargetField = "BikeColor")]
    string Color,
    string Brand
);

public class Truck
{
    public string? TruckColor { get; set; }
    public string? Brand { get; set; }
};

public class Bike
{
    public string? BikeColor { get; set; }
    public string? Brand { get; set; }
};

public class MultipleTargetFieldMapping
{
    [Fact]
    void Should_map_with_multiple_target_fields()
    {
        var vehicle = new Vehicle("Red", "Motobecane");
        var bike = vehicle.MapToBike();
        var truck = vehicle.MapToTruck();

        Assert.Equal(bike.BikeColor, vehicle.Color);
        Assert.Equal(bike.Brand, vehicle.Brand);

        Assert.Equal(truck.TruckColor, vehicle.Color);
        Assert.Equal(truck.Brand, vehicle.Brand);
    }
}