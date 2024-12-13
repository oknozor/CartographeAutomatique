namespace CartographeAutomatique.Samples.DocSample;

using CartographeAutomatique;

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


public class MappingExample
{
    [Fact]
    public void Should_Map_Vehicle_To_Car_And_Bike()
    {
        var vehicle = new Vehicle
        {
            Color = "Red",
            Brand = "Yamaha",
            KilometersDriven = 100
        };

        var car = vehicle.MapToCar();
        var bike = vehicle.MapToBike();

        Assert.Equal("Red", car.CarColor);
        Assert.Equal("Yamaha", car.Brand);
        Assert.Equal(62, car.MilesDriven);

        Assert.Equal("Red", bike.BikePaint);
        Assert.Equal("Yamaha", bike.Brand);
    }
}