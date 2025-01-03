namespace CartographeAutomatique;

internal static class Constant
{
    internal const string AttributeSourceCode =
        //language=csharp
        """
        #nullable enable
        // <auto-generated/>
        namespace CartographeAutomatique
        {
            [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
            public class MapToAttribute : System.Attribute
            {
                public MapToAttribute(System.Type targetClassName, 
                    bool Exhaustive = true, 
                    MappingStrategy MappingStrategy = MappingStrategy.Setter)
                {
                    TargetClassName = targetClassName;
                    this.Exhaustive = Exhaustive;
                    this.MappingStrategy = MappingStrategy;
                }
        
                public System.Type TargetClassName { get; }
                public bool Exhaustive { get; set; }
                public MappingStrategy MappingStrategy  { get; set; }
            }
            
            [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
            public class MapFromAttribute : System.Attribute
            {
                public MapFromAttribute(System.Type sourceClassName, 
                    bool Exhaustive = true, 
                    MappingStrategy MappingStrategy = MappingStrategy.Setter)
                {
                    SourceClassName = sourceClassName;
                    this.Exhaustive = Exhaustive;
                    this.MappingStrategy = MappingStrategy;
                }
        
                public System.Type SourceClassName { get; }
                public bool Exhaustive { get; set; }
                public MappingStrategy MappingStrategy  { get; set; }
            }
        
            [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Parameter, AllowMultiple = true)]
            public class MappingAttribute : System.Attribute
            {
                public MappingAttribute() {}
                public string? With { get; set;} = null;
                public string? TargetField { get; set;} = null;
                public System.Type? TargetType { get; set;} = null;
            }
            
            public enum MappingStrategy
            {
                Setter = 1,
                Constructor = 2,
            }
        }
        """;
}