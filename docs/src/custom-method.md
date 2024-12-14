# Custom mapping method

By default, CartographeAutomatique will map a source field to a target field and assume
field types are either the same or can be implicitly converted 
(for instance parsing a string into a number). Sometimes the source and target type will not match
or a custom logic is needed to map between type.

**Example:** 

```csharp

[MapTo(typeof(Car))]
public class Vehicle
{
    public required string Color { get; set; }
    public required string Brand { get; set; }

    [Mapping(
        With = "Vehicle.ConvertKilometersToMiles",
        TargetField = "MilesDriven")
    ]
    public int KilometersDriven { get; set; }

    public static int ConvertKilometersToMiles(int kilometers) => (int)(kilometers * 0.621371);
}

public record Car(string Color, string Brand, int MilesDriven);
```

In the above example we are mapping `KilometersDriven` to `MilesDriven` using `With` parameter on 
the `Mapping` attribute.