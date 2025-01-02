namespace CartographeAutomatique.Samples;

public class EnumConversions
{
    [Fact]
    void MapEnumColors()
    {
        var enumColors = new EnumColors()
        {
            StrColor = Color.Red,
            ShortColor = Color.Black,
            IntColor = Color.Green,
            LongColor = Color.Red,
        };

        var primitives = enumColors.MapToPrimitiveColors();
        Assert.Equal("Red", primitives.StrColor);
        Assert.Equal(2, primitives.ShortColor);
        Assert.Equal(3, primitives.IntColor);
        Assert.Equal(1, primitives.LongColor);
    }
}


[MapTo(typeof(EnumColors))]
[MapFrom(typeof(EnumColors))]
public class PrimitiveColors
{
    public string StrColor { get; set; }
    public short ShortColor { get; set; }
    public int IntColor { get; set; }
    public long LongColor { get; set; }
}

public class EnumColors
{
    public Color StrColor { get; set; }
    public Color ShortColor { get; set; }
    public Color IntColor { get; set; }
    public Color LongColor { get; set; }
}

public enum Color
{
    Red = 1,
    Black = 2,
    Green = 3
}