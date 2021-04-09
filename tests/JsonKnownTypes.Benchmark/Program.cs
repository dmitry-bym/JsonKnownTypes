using System.Linq;
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
public class JsonKnownTypesBenchmark
{
    [Benchmark]
    public string AutoBaseAbstractClass1HeirSerialize() 
        => JsonConvert.SerializeObject(Data1);

    public static BaseAbstractClass1Heir[] Data1
        = new Fixture().CreateMany<BaseAbstractClass1Heir>(100000).ToArray();

    [Benchmark]
    public string AutoBaseAbstractClass2HeirSerialize()
        => JsonConvert.SerializeObject(Data2);

    public static BaseAbstractClass2Heir[] Data2
        = new Fixture().CreateMany<BaseAbstractClass2Heir>(100000).ToArray();

    [Benchmark]
    public BaseAbstractClass[] AutoBaseAbstractClass1HeirDeserialize()
        => JsonConvert.DeserializeObject<BaseAbstractClass[]>(Data1De);

    public static string Data1De
        = JsonConvert.SerializeObject(new Fixture().CreateMany<BaseAbstractClass1Heir>(100000));

    [Benchmark]
    public BaseAbstractClass[] AutoBaseAbstractClass2HeirDeserialize()
        => JsonConvert.DeserializeObject<BaseAbstractClass[]>(Data2De);

    public static string Data2De
        = JsonConvert.SerializeObject(new Fixture().CreateMany<BaseAbstractClass2Heir>(100000));


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
public class NewtonsoftCleanBenchmark
{
    [Benchmark]
    public string AutoBaseAbstractClass1HeirSerialize()
        => JsonConvert.SerializeObject(Data1);

    public static BaseAbstractClass1Heir[] Data1
        = new Fixture().CreateMany<BaseAbstractClass1Heir>(100000).ToArray();

    [Benchmark]
    public string AutoBaseAbstractClass2HeirSerialize()
        => JsonConvert.SerializeObject(Data2);

    public static BaseAbstractClass2Heir[] Data2
        = new Fixture().CreateMany<BaseAbstractClass2Heir>(100000).ToArray();

    [Benchmark]
    public BaseAbstractClass[] AutoBaseAbstractClass1HeirDeserialize()
        => JsonConvert.DeserializeObject<BaseAbstractClass[]>(Data1De);

    public static string Data1De
        = JsonConvert.SerializeObject(new Fixture().CreateMany<BaseAbstractClass1Heir>(100000));

    [Benchmark]
    public BaseAbstractClass[] AutoBaseAbstractClass2HeirDeserialize()
        => JsonConvert.DeserializeObject<BaseAbstractClass[]>(Data2De);

    public static string Data2De
        = JsonConvert.SerializeObject(new Fixture().CreateMany<BaseAbstractClass2Heir>(100000));

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
public class NewtonsoftTypeNameHandlingAllBenchmark
{
    [Benchmark]
    public string AutoBaseAbstractClass1HeirSerialize()
        => JsonConvert.SerializeObject(Data1, new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.All});

    public static BaseAbstractClass1Heir[] Data1
        = new Fixture().CreateMany<BaseAbstractClass1Heir>(100000).ToArray();

    [Benchmark]
    public string AutoBaseAbstractClass2HeirSerialize()
        => JsonConvert.SerializeObject(Data2, new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.All});

    public static BaseAbstractClass2Heir[] Data2
        = new Fixture().CreateMany<BaseAbstractClass2Heir>(100000).ToArray();

    [Benchmark]
    public BaseAbstractClass[] AutoBaseAbstractClass1HeirDeserialize()
        => JsonConvert.DeserializeObject<BaseAbstractClass[]>(Data1De, new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.All});

    public static string Data1De
        = JsonConvert.SerializeObject(new Fixture().CreateMany<BaseAbstractClass1Heir>(100000).ToArray(), new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.All});

    [Benchmark]
    public BaseAbstractClass[] AutoBaseAbstractClass2HeirDeserialize()
        => JsonConvert.DeserializeObject<BaseAbstractClass[]>(Data2De, new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.All});

    public static string Data2De
        = JsonConvert.SerializeObject(new Fixture().CreateMany<BaseAbstractClass2Heir>(100000).ToArray(), new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.All});

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