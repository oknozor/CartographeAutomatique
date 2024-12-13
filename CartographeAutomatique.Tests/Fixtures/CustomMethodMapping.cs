namespace CartographeAutomatique.Tests;

public class CustomMethodMapping
{
    private const string Source =
        //language=csharp
        """
            namespace TestNamespace;
            using CartographeAutomatique;

            [MapTo(typeof(StreetNumber))]
            public class AddressNumber
            {
                [Mapping(With = "MapperHelpers.ParseFloat")]
                public string? Value { get; set; }
                
            }

            public class StreetNumber
            {
                public float Value { get; set; };
            }

            public static class MapperHelpers {
                public static float ParseFloat(string value)
                {
                    return float.Parse(value);
                }
            }
            """;

    private const string Expected =
        //language=csharp
        """
            // <auto-generated/>
            namespace TestNamespace;

            public static partial class AddressNumberMappingExtensions
            {
                public static TestNamespace.StreetNumber MapToStreetNumber(this TestNamespace.AddressNumber source) =>
                   	new()
            		{
            			Value = MapperHelpers.ParseFloat(source.Value)
            		};
            }
            """;

    public SourceGenerationAssertion GetAssertion() =>
        new(Source, [new SourceGenerationOutput("TestNamespace_AddressNumberToTestNamespace_StreetNumber", Expected)]);
}
