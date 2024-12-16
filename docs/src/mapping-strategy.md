# Mapping strategy

By default, CartographeAutomatique will use setters to construct a class type and constructor 
for record target type. 

Here is a sample of the generated code for a mapping to `Point` class: 

```csharp
public static partial class Vector3MappingExtensions
{
    public static TestNamespace.Point MapToPoint(this TestNamespace.Vector3 source) =>
       	new()
		{
			X = source.X,
			Y = source.Y,
			Z = source.Z
		};
}
```

Assuming `Point` is a record, the generated code would look like this: 

```csharp
public static partial class ColorMappingExtensions
{
    public static TestNamespace.Point MapToPoint(this TestNamespace.Vector3 source) =>
       	new(X: source.X, Y: source.Y, Z: source.Z);
}
```

You can change this default behavior using `MappingStrategyAttribute`.

**Example:**

```csharp
[MapTo(typeof(Point), MappingStrategy = MappingStrategy.Constructor)]
public class Vector
{
    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }
}

public class Point
{
    public Point(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}
```

<div class="warning">
Note that attributes and constructor parameters will match from "PascalCase" to "camelCase" and vice versa.
For instance in the example below the `Vector.X` attribute will be mapped via `Point` constructor parameter `x`
</div>