using CartographeAutomatique;

namespace CartographeAutomatique.Samples.SimpleFromMapping;

[MapFrom(typeof(Point3))]
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

public class SimpleFromMapping
{
    [Fact]
    void Should_generate_mappings()
    {
        var point = new Point3()
        {
            X = 1,
            Y = 1,
            Z = 3,
        };

        var vector3 = point.MapToVector3();

        Assert.Equal(vector3.X, point.X);
        Assert.Equal(vector3.Y, point.Y);
        Assert.Equal(vector3.Z, point.Z);
    }
}
