using Generators;
using Xunit;

namespace CartographeAutomatique.Tests;


[MapTo(typeof(Point))]
[MapTo(typeof(Point2), Exhaustive = false)]
public partial class Vector3
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
        var vector2 = new Vector3
        {
            X = 1,
            Y = 1,
            Z = 1,
        };

        Point2 point = vector2.MapToPoint2();

        Assert.Equal(vector2.X, point.X);
        Assert.Equal(vector2.Y, point.Y);
    }
}