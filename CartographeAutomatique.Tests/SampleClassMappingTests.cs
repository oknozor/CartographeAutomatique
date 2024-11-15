using Generators;
using Xunit;

namespace CartographeAutomatique.Tests;


[MapTo(typeof(Point))]
[MapTo(typeof(Point2), Exhaustive = false)]
public class Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class Point
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

/*[MapTo(typeof(Line2), Exhaustive = false)]
public class Line3
{
    public Point A { get; set; }
    public Point B { get; set; }
}

public class Line2
{
    public Point2 A { get; set; }
    public Point2 B { get; set; }
}*/


public class SampleMapGeneratorTests
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

        Point point = vector3.MapToPoint();

        Assert.Equal(vector3.X, point.X);
        Assert.Equal(vector3.Y, point.Y);
        Assert.Equal(vector3.Z, point.Z);
    }
    
    [Fact]
    void Should_generate_non_exhaustive_mappings()
    {
        var vector3 = new Vector3
        {
            X = 1,
            Y = 1,
            Z = 1,
        };

        Point2 point = vector3.MapToPoint2();

        Assert.Equal(vector3.X, point.X);
        Assert.Equal(vector3.Y, point.Y);
    }
}