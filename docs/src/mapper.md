# Defining a mapper

When using CartographeAutomatique you can generate mappers using the `MapTo` or `MapFrom` Attributes.

## Map from source to target

Let's take a look at a simple example: 

```csharp
public record Vehicle(string? Color, string? Brand);

[MapTo(typeof(Vehicle))]
public class Truck
{
    public string? Color { get; set; }
    public string? Brand { get; set; }
}
```

The above would generate a `MapToVehicle` extension method on `Truck`: 

```csharp
Truck truck = new Truck() { Color = "Red", Brand = "Peugeot" };
Vehicle vehicle = truck.MapToVehicle();
```

## Map from target to source

```csharp
[MapFrom(typeof(Truck))]
public record Vehicle(string? Color, string? Brand);

public class Truck
{
    public string? Color { get; set; }
    public string? Brand { get; set; }
}
```

The above would generate a `MapToVehicle` extension method on `Truck`:

```csharp
Truck truck = new Truck() { Color = "Red", Brand = "Peugeot" };
Vehicle vehicle = truck.MapToVehicle();

Assert.Equal(truck.Color, vehicle.Color);
Assert.Equal(truck.Brand, vehicle.Brand);
```

<div class="warning">
Note that both approach generate the same mapping extension, the only difference 
is where you put the mapping attribute.
</div>