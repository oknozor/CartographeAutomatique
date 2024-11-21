using Generators;

namespace CartographeAutomatique.Samples.ConstructorClassMapping;

[MapTo(typeof(Point), MappingStrategy = MappingStrategy.Constructor)]
public class Vector
{
    [TargetMapping(TargetField = "x")] public float X { get; set; }
    [TargetMapping(TargetField = "y")] public float Y { get; set; }
    [TargetMapping(TargetField = "z")] public float Z { get; set; }
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

public class ConstructorClassMapping
{
    [Fact]
    void Should_generate_mappings()
    {
        var vector3 = new Vector()
        {
            X = 1,
            Y = 1,
            Z = 3,
        };

        Point point = vector3.MapToPoint();

        Assert.Equal(vector3.X, point.X);
        Assert.Equal(vector3.Y, point.Y);
        Assert.Equal(vector3.Z, point.Z);
    }
}