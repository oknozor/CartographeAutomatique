namespace CartographeAutomatique.Tests;


public class ListToList : IFixture
{
    private const string Source =
        //language=csharp
        """
        using System.Collections.Generic;
        using Generators;
        
        namespace TestNamespace;

        [MapTo(typeof(Article))]
        public class Fruit
        {
            public string Name { get; set; }
            public string Color { get; set; }
        }
        
        public class Article
        {
            public string Name { get; set; }
            public string Color { get; set; }
        }
        
        [MapTo(typeof(Basket))]
        public class Garden
        {
            [Mapping(TargetField = "Articles")]
            public List<Fruit> Fruits { get; set; }
        }
        
        public class Basket
        {
            public List<Article> Articles { get; set; }
        }
        """;

    private const string ExpectedBasketMapping =
        //language=csharp
        """
        // <auto-generated/>
        namespace TestNamespace;
        
        public static partial class GardenMappingExtensions
        {
            public static TestNamespace.Basket MapToBasket(this Garden source) =>
               	new()
        		{
        			Articles = source.Fruits.Select(t => t.MapToArticle()).ToList()
        		};
        }
        """;

    private const string ExpectedFruitMapping =
        //language=csharp
        """
        // <auto-generated/>
        namespace TestNamespace;
        
        public static partial class FruitMappingExtensions
        {
            public static TestNamespace.Article MapToArticle(this Fruit source) =>
               	new()
        		{
        			Name = source.Name,
        			Color = source.Color
        		};
        }
        """;

    public SourceGenerationAssertion GetAssertion() =>
        new(Source, [
            new SourceGenerationOutput("FruitToArticle", ExpectedFruitMapping),
            new SourceGenerationOutput("GardenToBasket", ExpectedBasketMapping),
        ]);
}