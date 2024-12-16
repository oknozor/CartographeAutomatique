# Exhaustive mapping

By default, CartographeAutomatique will try to map only fields required to construct the target type.

**Example:**

The example below will produce the expected mapper.

```csharp
[MapTo(typeof(Point2))]
public class Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class Point2
{
    public float X { get; set; }
    public float Y { get; set; }
}
```

If you want to force CartographeAutomatique to generate a mapping for every field in the source type
you can use the `Exhaustive = false` attribute parameter: 

```csharp
[MapTo(typeof(Point2), Exhaustive = true)]
public class Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class Point2
{
    public float X { get; set; }
    public float Y { get; set; }
}
```

The code above will not compile because there is no `Z` field in the target type.  

