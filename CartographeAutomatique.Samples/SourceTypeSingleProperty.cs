namespace CartographeAutomatique.Samples.SourceTypeSingleProperty;



public class A
{

    public string C { get; set; }
}

[MapTo(typeof(A))]
public class B
{

    public required C C { get; set; }
}

public class C(string inner)
{

    public string Inner { get; set; } = inner;
}

public class SourceTypeSingleProperty
{
    [Fact]
    public void Should_map_type_with_single_member()
    {
        var b = new B { C = new C("value") };
        var a = b.MapToA();

        Assert.Equal("value", a.C);
    }
}