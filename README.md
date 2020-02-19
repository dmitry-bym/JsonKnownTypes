# JsonKnownTypes .Net Standard
Help to serialize and deserialize polymorphic types. Add distractor to json data

- [Documentation](#Documentation)
- [License](#License)

## Requirements
- NET Standard 2.0 compatible project
- [Json.NET](https://github.com/JamesNK/Newtonsoft.Json) by Newtonsoft

## Documentation
### Getting started
There is simple way to use just add one attribute to base class or interface
```c#
  [JsonConverter(typeof(JsonKnownTypeConverter<BaseClass>))]
  public class BaseClass
  {
    public string Summary { get; set; }
  }
```

If you need to add custom discriminator just use `JsonKnowType` attribute.  
By default for discriminattor property using `"$type"` name, if you need to change it use `JsonKnownDiscriminator` attribute. 
```c#
  [JsonConverter(typeof(JsonKnownTypeConverter<BaseClass>))]
  [JsonKnownDiscriminator(Name = "myType")] //add custom discriminator name
  [JsonKnownType(typeof(BaseClass1Heir))] //could be deleted if you didn't turn off AutoJsonKnownType
  [JsonKnownType(typeof(BaseClass2Heir), "myDiscriminator")]
  public class BaseClass { ... }
  
  public class BaseAbstractClass1Heir : BaseClass  { ... }
  
  public class BaseAbstractClass2Heir : BaseClass  { ... }
```
### Configuration
For change default discriminator settings use:
```c#
  JsonKnownSettingsService.DiscriminatorAttribute = new JsonKnownDiscriminatorAttribute
  {
    Name = "type",
    AutoJsonKnownType = false
  };
```
### Use Manualy 
```c#
  var entityJson = JsonConvert.SerializeObject(entity, new JsonKnownTypeConverter<BaseClass>());
```
## License

Authored by: Dmitry Kaznacheev (dmitry-bym)

This project is under MIT license. You can obtain the license copy [here](https://github.com/dmitry-bym/JsonKnownTypes/blob/master/LICENSE).

This work using work of James Newton-King, author of Json.NET. https://www.newtonsoft.com/json
