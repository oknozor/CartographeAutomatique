namespace CartographeAutomatique.Tests;

public class FieldMappingImplicit
{
    private const string Source =
        //language=csharp
        """
        
            namespace TestNamespace;
        
            [MapTo(typeof(SongWriter))]
            class Author
            {
                [TargetMapping(TargetField = "FullName")]
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
            public static TestNamespace.SongWriter MapToSongWriter(this Author source) =>
               	new()
        		{
        			FullName = source.Name
        		};
        }
        """;

    public SourceGenerationAssertion GetAssertion() =>
        new(Source, [new("AuthorToSongWriter", Expected)]);
}