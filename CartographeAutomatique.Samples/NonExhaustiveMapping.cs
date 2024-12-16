using CartographeAutomatique;
using Xunit;

namespace CartographeAutomatique.Tests.Samples.NonExhaustiveMapping;

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

public class NonExhaustiveMapping
{
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