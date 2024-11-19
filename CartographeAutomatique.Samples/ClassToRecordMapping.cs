using Generators;

namespace CartographeAutomatique.Tests.Samples.ClassToRecordMapping;

public record Car(string Color, string Brand);

[MapTo(typeof(Car))]
public class Truck
{
    public string Color { get; set; }
    public string Brand { get; set; }
};

public class ClassToRecordMapping
{
    [Fact]
    void Should_map_record_to_class()
    {
        var truck = new Truck()
        {
            Color = "Red",
            Brand = "Peugeot"
        };

        truck.MapToCar();
    }
}