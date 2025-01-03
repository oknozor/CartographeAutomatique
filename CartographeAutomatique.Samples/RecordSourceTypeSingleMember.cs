namespace CartographeAutomatique.Samples.RecordSourceTypeSingleMember;

public record A(string C);

[MapTo(typeof(A))]
public class B
{
    public C C { get; set; }
}

public record C(string Inner);

public class RecordSourceTypeSingleMember
{
    [Fact]
    public void Should_map_type_with_single_member()
    {
        var b = new B { C = new C("value") };
        var a = b.MapToA();

        Assert.Equal("value", a.C);
    }
}