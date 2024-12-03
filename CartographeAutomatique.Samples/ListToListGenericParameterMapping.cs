namespace CartographeAutomatique.Samples.ListToListGenericParameterMapping;

using CartographeAutomatique;

[MapTo(typeof(Article))]
public class Fruit
{
    public string? Name { get; set; }
}

public class Article
{
    public string? Name { get; set; }
}

[MapTo(typeof(Basket))]
public class Garden
{
    [Mapping(TargetField = "Articles")]
    public List<Fruit>? Fruits { get; set; }
}

public class Basket
{
    public required List<Article> Articles { get; set; }
}

public class ListToListGenericParameterMapping
{
    [Fact]
    void Should_generate_mappings()
    {
        var garden = new Garden()
        {
            Fruits = [new Fruit() { Name = "Apple" }, new Fruit() { Name = "Pear" }],
        };

        Basket basket = garden.MapToBasket();

        Assert.Equal("Apple", basket.Articles[0].Name);
        Assert.Equal("Pear", basket.Articles[1].Name);
    }
}
