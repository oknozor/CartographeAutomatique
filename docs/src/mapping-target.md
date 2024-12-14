# Mapping target

As we saw in the previous examples, CartographeAutomatique will generate mapping 
for any matching field name between source and target type.

Sometime we will need to adjust the target field name explicitly.

**Example:**

```csharp
[MapTo(typeof(Bike))]
public record Vehicle(
    [Mapping(TargetField = "BikeColor")]
    string Color,
    string Brand
);

public class Bike
{
    public string? BikeColor { get; set; }
    public string? Brand { get; set; }
}
```

Using `[Mapping(TargetField = "BikeColor")]` will update the mapping target to the desired filed.

```csharp
var vehicle = new Vehicle("Red", "Motobecane");
var bike = vehicle.MapToBike();

Assert.Equal(bike.BikeColor, vehicle.Color);
Assert.Equal(bike.Brand, vehicle.Brand);
```

## Disambiguate multiple mappings

Sometimes class will not map to a single target. 
For instance if we add a new target mapping to a `Bike` class to our previous example 
we would need to explicitly state the target class along with the target fieldname.

**Example:**

```csharp
[MapTo(typeof(Truck))]
[MapTo(typeof(Bike))]
public record Vehicle(
    [Mapping(TargetType = typeof(Truck), TargetField = "TruckColor")]
    [Mapping(TargetType = typeof(Bike), TargetField = "BikeColor")]
    string Color,
    string Brand
);
```