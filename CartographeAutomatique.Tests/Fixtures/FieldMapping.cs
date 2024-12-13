namespace CartographeAutomatique.Tests;

public class FieldMapping : IFixture
{
    private const string Source =
        //language=csharp
        """

                namespace TestNamespace;
                using CartographeAutomatique;
                
                [MapTo(typeof(SongWriter))]
                class Author
                {
                    [Mapping(TargetType = typeof(SongWriter), TargetField = "FullName")]
                    string Name { get; set; }
                }

                class SongWriter
                {
                    string FullName { get; set; }
                }
            """;

    private const string Expected =
        //language=csharp
        """
            // <auto-generated/>
            namespace TestNamespace;

            public static partial class AuthorMappingExtensions
            {
                public static TestNamespace.SongWriter MapToSongWriter(this TestNamespace.Author source) =>
                   	new()
            		{
            			FullName = source.Name
            		};
            }
            """;

    public SourceGenerationAssertion GetAssertion() =>
        new(Source, [new("TestNamespace_AuthorToTestNamespace_SongWriter", Expected)]);
}
