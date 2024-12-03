using CartographeAutomatique;

namespace CartographeAutomatique.Tests.Samples.SimpleMapping;

[MapTo(typeof(Point3))]
public class Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class Point3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class SimpleMapping
{
    [Fact]
    void Should_generate_mappings()
    {
        var vector3 = new Vector3()
        {
            X = 1,
            Y = 1,
            Z = 3,
        };

        Point3 point = vector3.MapToPoint3();

        Assert.Equal(vector3.X, point.X);
        Assert.Equal(vector3.Y, point.Y);
        Assert.Equal(vector3.Z, point.Z);
    }
}
