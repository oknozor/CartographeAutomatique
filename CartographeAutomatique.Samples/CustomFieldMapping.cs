using Generators;
using Xunit;

namespace CartographeAutomatique.Tests.Samples.CustomFieldMapping;

[MapTo(typeof(SongWriter))]
public class Author
{
    [TargetMapping(TargetField = "FullName")]
    public string Name { get; init; }
}

public class SongWriter
{
    public string FullName { get; set; }
}

public class CustomFieldMapping
{
    [Fact]
    void Should_generate_field_mappings()
    {
        var author = new Author()
        {
            Name = "Jean Philippe Jaworsky"
        };

        var songWriter = author.MapToSongWriter();

        Assert.Equal(author.Name, songWriter.FullName);
    }
}