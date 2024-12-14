namespace CartographeAutomatique.Tests.Samples.ClassToRecordMapping;

public record Vehicle(string? Color, string? Brand);

[MapTo(typeof(Vehicle))]
public class Truck
{
    public string? Color { get; set; }
    public string? Brand { get; set; }
}

public class ClassToRecordMapping
{
    [Fact]
    void Should_map_record_to_class()
    {
        Truck truck = new Truck() { Color = "Red", Brand = "Peugeot" };
        Vehicle vehicle = truck.MapToVehicle();
    }
}
