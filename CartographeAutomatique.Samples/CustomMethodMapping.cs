using Generators;

namespace CartographeAutomatique.Samples.CustomMethodMapping;

[MapTo(typeof(StreetNumber))]
public class AddressNumber
{
    [Mapping(With = "float.Parse")]
    public string? Value { get; set; }

}

public class StreetNumber
{
    public float Value { get; set; }
}

public class CustomMethodMapping
{
    [Fact]
    void Should_map_with_custom_method()
    {
        var address = new AddressNumber()
        {
            Value = "41",
        };

        var streetNumber = address.MapToStreetNumber();

        Assert.Equal(41, streetNumber.Value);
    }
}

