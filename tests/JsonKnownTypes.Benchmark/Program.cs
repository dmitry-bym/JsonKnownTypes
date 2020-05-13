using AutoFixture;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using JsonKnownTypes;
using Newtonsoft.Json;

namespace JsonKnownTypes.Benchmark
{
    class Program
    {
        static void Main(string[] args)
            => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

    }
}

[MemoryDiagnoser]
public class Benchmark1
{
    [Benchmark]
    public string AutoBaseAbstractClass1HeirSerialize()
        => JsonConvert.SerializeObject(Data1);

    public static BaseAbstractClass1Heir Data1
        = new Fixture().Create<BaseAbstractClass1Heir>();

    [Benchmark]
    public string AutoBaseAbstractClass2HeirSerialize()
        => JsonConvert.SerializeObject(Data2);

    public BaseAbstractClass2Heir Data2
        = new Fixture().Create<BaseAbstractClass2Heir>();

    [Benchmark]
    public BaseAbstractClass AutoBaseAbstractClass1HeirDeserialize()
        => JsonConvert.DeserializeObject<BaseAbstractClass>(Data1De);

    public static string Data1De
        = JsonConvert.SerializeObject(new Fixture().Create<BaseAbstractClass1Heir>());

    [Benchmark]
    public BaseAbstractClass AutoBaseAbstractClass2HeirDeserialize()
        => JsonConvert.DeserializeObject<BaseAbstractClass>(Data2De);

    public static string Data2De
        = JsonConvert.SerializeObject(new Fixture().Create<BaseAbstractClass2Heir>());


    [JsonConverter(typeof(JsonKnownTypesConverter<BaseAbstractClass>))]
    [JsonDiscriminator(Name = "Type")]
    [JsonKnownType(typeof(BaseAbstractClass1Heir), "Heir1")]
    [JsonKnownType(typeof(BaseAbstractClass2Heir), "Heir2")]
    public class BaseAbstractClass
    {
        public string Summary { get; set; }
    }

    public class BaseAbstractClass1Heir : BaseAbstractClass
    {
        public int SomeInt { get; set; }
    }

    public class BaseAbstractClass2Heir : BaseAbstractClass
    {
        public double SomeDouble { get; set; }
        public string Detailed { get; set; }
    }
}


[MemoryDiagnoser]
public class Benchmark2
{
    [Benchmark]
    public string AutoBaseAbstractClass1HeirSerialize()
        => JsonConvert.SerializeObject(Data1);

    public static BaseAbstractClass1Heir Data1
        = new Fixture().Create<BaseAbstractClass1Heir>();

    [Benchmark]
    public string AutoBaseAbstractClass2HeirSerialize()
        => JsonConvert.SerializeObject(Data2);

    public BaseAbstractClass2Heir Data2
        = new Fixture().Create<BaseAbstractClass2Heir>();

    [Benchmark]
    public BaseAbstractClass AutoBaseAbstractClass1HeirDeserialize()
        => JsonConvert.DeserializeObject<BaseAbstractClass>(Data1De);

    public static string Data1De
        = JsonConvert.SerializeObject(new Fixture().Create<BaseAbstractClass1Heir>());

    [Benchmark]
    public BaseAbstractClass AutoBaseAbstractClass2HeirDeserialize()
        => JsonConvert.DeserializeObject<BaseAbstractClass>(Data2De);

    public static string Data2De
        = JsonConvert.SerializeObject(new Fixture().Create<BaseAbstractClass2Heir>());

    public class BaseAbstractClass
    {
        public string Summary { get; set; }
    }

    public class BaseAbstractClass1Heir : BaseAbstractClass
    {
        public int SomeInt { get; set; }
    }

    public class BaseAbstractClass2Heir : BaseAbstractClass
    {
        public double SomeDouble { get; set; }
        public string Detailed { get; set; }
    }
}