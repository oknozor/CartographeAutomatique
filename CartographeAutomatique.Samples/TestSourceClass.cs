using CartographeAutomatique.Tests;

namespace CartographeAutomatique.Samples;

public class TestSourceClass
{
    public string A { get; set; }
}


public class Test
{
    [Fact]
    void test()
    {
        var testSourceClass = new TestSourceClass()
        {
            A = "A",
        };
        
    }
}