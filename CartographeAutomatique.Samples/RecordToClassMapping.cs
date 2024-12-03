using CartographeAutomatique;
using Xunit;

namespace CartographeAutomatique.Tests.Samples;

[MapTo(typeof(Truck))]
public record Car(string Color, string Brand);

public class Truck
{
    public string? Color { get; set; }
    public string? Brand { get; set; }
}

public class RecordToClassMapping
{
    [Fact]
    void Should_map_record_to_class()
    {
        var car = new Car("red", "r21");

        var truck = car.MapToTruck();

        Assert.Equal("red", truck.Color);
        Assert.Equal("r21", truck.Brand);
    }
}
