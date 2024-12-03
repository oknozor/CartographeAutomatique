using CartographeAutomatique;
using Xunit;

namespace CartographeAutomatique.Tests.Samples.RecursiveMapping;

[MapTo(typeof(Line2), Exhaustive = false)]
public class Line3
{
    public Point3? A { get; set; }
    public Point3? B { get; set; }
}

public class Line2
{
    public Point2? A { get; set; }
    public Point2? B { get; set; }
}

[MapTo(typeof(Point2))]
public class Point3
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

public class RecursiveMapping
{
    [Fact]
    void Should_generate_recursive_mappings()
    {
        var line3 = new Line3
        {
            A = new Point3
            {
                X = 1,
                Y = 1,
                Z = 3,
            },
            B = new Point3
            {
                X = 5,
                Y = 4,
                Z = 6,
            },
        };

        Line2 line = line3.MapToLine2();

        Assert.Equal(line.A!.X, line3.A.MapToPoint2().X);
        Assert.Equal(line.A.Y, line3.A.MapToPoint2().Y);
        Assert.Equal(line.B!.X, line3.B.MapToPoint2().X);
        Assert.Equal(line.B.Y, line3.B.MapToPoint2().Y);
    }
}
