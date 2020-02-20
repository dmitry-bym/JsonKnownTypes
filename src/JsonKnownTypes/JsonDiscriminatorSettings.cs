namespace JsonKnownTypes
{
    public class JsonDiscriminatorSettings
    {
        public string Name { get; set; } = "$type";
        public bool AutoJsonKnownType { get; set; } = true;
    }
}
