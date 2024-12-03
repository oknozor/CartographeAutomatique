using System.Globalization;
using CartographeAutomatique;

namespace CartographeAutomatique.Samples.ImplicitMapping;

[MapTo(typeof(Point))]
public class Vector3
{
    public float X { get; set; }
    public int Y { get; set; }
    public string? Z { get; set; }
}

public class Point
{
    public string? X { get; set; }
    public string? Y { get; set; }
    public float Z { get; set; }
}

public class ImplicitMapping
{
    [Fact]
    void Should_generate_mappings()
    {
        var vector3 = new Vector3()
        {
            X = 1,
            Y = 1,
            Z = "3",
        };

        Point point = vector3.MapToPoint();

        Assert.Equal(vector3.X, float.Parse(point.X!));
        Assert.Equal(vector3.Y, int.Parse(point.Y!));
        Assert.Equal(vector3.Z, point.Z.ToString(CultureInfo.InvariantCulture));
    }
}
