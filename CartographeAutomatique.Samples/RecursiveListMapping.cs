namespace CartographeAutomatique.Samples.RecursiveListMapping;

using CartographeAutomatique;

[MapTo(typeof(B))]
public class A
{
    public required List<List<List<string>>> Names { get; set; }
}

public class B
{
    public required List<string[][]> Names { get; set; }
}

public class RecursiveListMapping
{
    [Fact]
    void Should_generate_mappings()
    {
        var a = new A
        {
            Names =
            [
                [
                    ["Riri", "Fifi", "Loulou"],
                ],
            ],
        };

        var b = a.MapToB();
        var names = b.Names.SelectMany(t => t.SelectMany(t => t)).ToArray();

        Assert.Equal(["Riri", "Fifi", "Loulou"], names);
    }
}
