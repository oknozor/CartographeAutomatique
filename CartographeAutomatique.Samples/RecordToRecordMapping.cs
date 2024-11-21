using Generators;
using Xunit;

namespace CartographeAutomatique.Tests.Samples.RecordToRecordMapping;


[MapTo(typeof(HexColor))]
public record Color([Mapping(TargetType = typeof(HexColor), TargetField = "HexValue")] string Value);

public record HexColor(string HexValue);


public class RecordToRecordMapping
{
    [Fact]
    void Should_map_record_to_record()
    {
        var blue = new Color("#0000FF");
        var hexBlue = blue.MapToHexColor();

        Assert.Equal(blue.Value, hexBlue.HexValue);
    }
}