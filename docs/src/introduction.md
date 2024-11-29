### Introduction to Cartographe Automatique

Cartographe Automatique is a Roselyn SourceGenerator designed to streamline the process of mapping between C# types. By annotating your source and target classes with specific attributes, it generates type-safe, performant, and dependency-free mapping methods. This eliminates boilerplate code and ensures maintainable, consistent mapping logic throughout your application.

#### Key Features
1. **Attributes for Flexible Configuration**:
  - **`MapToAttribute`**: Defines a mapping from a source class to one or more target classes.
    - Configurable properties include:
      - `Exhaustive`: Ensures all properties of the target type are mapped.
      - `MappingStrategy`: Specifies whether the mapping uses property setters or a constructor.
  - **`MapFromAttribute`**: Defines a mapping from a source type to the current class.
  - **`MappingAttribute`**: Allows customization of individual property mappings, such as:
    - Renaming properties.
    - Targeting specific fields or types.

2. **Primitive and Collection Type Conversion**:
  - Built-in support for implicit conversion of primitive types and common collection types (e.g., arrays, lists) between source and target classes.

3. **Generated Mapping Methods**:
  - Methods are generated based on the provided attributes, ensuring type safety and optimal performance.

---

### Example: Mapping with Custom Property Names and Strategies

Below is an example showcasing how to use Cartographe Automatique to map between classes with different property names and mapping strategies:

#### Code Example

```csharp
[MapTo(typeof(Car), MappingStrategy = MappingStrategy.Constructor)]
[MapTo(typeof(Bike), Exhaustive = false)]
public class Vehicle
{
    [Mapping(TargetType = typeof(Car), TargetField = "CarColor")]
    [Mapping(TargetType = typeof(Bike), TargetField = "BikePaint")]
    public string Color { get; set; }

    public string Brand { get; set; }

    [Mapping(With = "ConvertKilometersToMiles", TargetType = typeof(Car))]
    public int KilometersDriven { get; set; }

    private static int ConvertKilometersToMiles(int kilometers) => (int)(kilometers * 0.621371);
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
    public string BikePaint { get; set; }
    public string Brand { get; set; }
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
```

---

### Explanation of Features in the Example

1. **Attributes in Action**:
  - The `MapTo` attributes on the `Vehicle` class configure mappings to both `Car` and `Bike` classes.
  - The `Mapping` attribute customizes specific property mappings:
    - `Color` maps to `CarColor` in the `Car` class and `BikePaint` in the `Bike` class.
    - `KilometersDriven` is converted to `MilesDriven` using a custom method `ConvertKilometersToMiles`.

2. **Mapping Strategy**:
  - The `Car` class uses the `Constructor` mapping strategy to initialize immutable properties.
  - The `Bike` class uses the default `Setter` strategy, where properties are assigned directly.

3. **Implicit Type Conversion**:
  - Primitive types, like `int`, are implicitly handled during the conversion of `KilometersDriven` to `MilesDriven`.

4. **Generated Methods**:
  - The `MapToCar` and `MapToBike` methods are automatically generated, encapsulating the mapping logic efficiently.

---

Cartographe Automatique simplifies complex mapping scenarios while maintaining type safety, extensibility, and performance. With its customizable attributes and robust support for both primitive and collection types, it’s an invaluable tool for C# developers.
