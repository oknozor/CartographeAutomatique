namespace CartographeAutomatique.Samples.ListToArrayMapping;

using CartographeAutomatique;

[MapTo(typeof(Person))]
public class NameList
{
    public required List<string> Names { get; set; }
}

public class Person
{
    public required string[] Names { get; set; }
}

public class ListToArrayMapping
{
    [Fact]
    void Should_generate_mappings()
    {
        var a = new NameList() { Names = ["Riri", "Fifi", "Loulou"] };

        Person b = a.MapToPerson();

        Assert.Equal(a.Names, b.Names);
    }
}
