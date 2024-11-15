using Generators;
using Xunit;

namespace CartographeAutomatique.Tests;


[MapTo(typeof(Point))]
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
}