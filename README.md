# JsonKnownTypes .Net Standard
[![lisence](https://img.shields.io/badge/nuget-v0.2.0-orange?logo=nuget&style=flat-square)](https://www.nuget.org/packages/JsonKnownTypes/)  
Please, update from v0.1.0, there was critical bugs

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
  [JsonConverter(typeof(JsonKnownTypesConverter<BaseClass>))]
  public class BaseClass
  {
    public string Summary { get; set; }
  }
  
  public class ChildClass : BaseClass
  {
    public string Detailed { get; set; }
  }
```
Serialization and Deserialization:
```c#
  var entityJson = JsonConvert.SerializeObject(entity);
  var obj = DeserializeObject<BaseClass>(entityJson)
```
Json representation:
```json
{ "Summary":"someValue", "$type":"BaseClass" }
{ "Summary":"someValue", "Detailed":"someValue", "$type":"ChildClass" }
```
### JsonKnownType
If you need to add custom discriminator just use `JsonKnowType` attribute.  
By default for discriminattor property using `"$type"` name, if you need to change it use `JsonKnownDiscriminator` attribute. 
```c#
  [JsonConverter(typeof(JsonKnownTypesConverter<BaseClass>))]
  [JsonDiscriminator(Name = "myType")] //add custom discriminator name
  [JsonKnownType(typeof(BaseClass1Heir))] //could be deleted if you didn't turn off AutoJsonKnownType
  [JsonKnownType(typeof(BaseClass2Heir), "myDiscriminator")]
  public class BaseClass { ... }
  
  public class BaseAbstractClass1Heir : BaseClass  { ... }
  
  public class BaseAbstractClass2Heir : BaseClass  { ... }
```
Json representation:
```
{ ... , "myType":"BaseClass" }

{ ... , "myType":"BaseClass1Heir" }

{ ... , "myType":"myDiscriminator" }
```
### JsonKnownThisType
Add discriminator for type which is used with it
```c#
  [JsonConverter(typeof(JsonKnownTypesConverter<BaseClass>))]
  [JsonKnownThisType("do_you_know_that")]
  public class BaseClass { ... }
  
  [JsonKnownThisType("html_is_programming_language")]
  public class BaseAbstractClass1Heir : BaseClass  { ... }
  
  [JsonKnownThisType("just_joke=)")]
  public class BaseAbstractClass2Heir : BaseClass  { ... }
```
Json representation:
```
{ ... , "$type":"do_you_know_that" }

{ ... , "$type":"html_is_programming_language" }

{ ... , "$type":"just_joke=)" }
```
### Configuration
For change default discriminator settings use:
```c#
  JsonKnownTypesSettingsManager.DefaultDiscriminatorSettings = new JsonDiscriminatorSettings
  {
    Name = "name",
    AutoJsonKnown = false
  };
```
> Name change default `"$type"` name to yours  

> If `AutoJsonKnownType` is false you should to add `JsonKnownType` or `JsonKnownThisType` attribute for each relative class manualy or it throw an Exception
### Use manualy
```c#
  public class BaseClass { ... }
  public class BaseAbstractClass1Heir : BaseClass  { ... }
  public class BaseAbstractClass2Heir : BaseClass  { ... }
```
```c#
  var converter = new JsonKnownTypesConverter<BaseClass>()
  
  var entityJson = JsonConvert.SerializeObject(entity, converter);
  var obj = DeserializeObject<BaseClass>(entityJson, converter)
```
> Need to add converter to method just if you don't use `JsonConvert` attribute
## License

Authored by: Dmitry Kaznacheev (dmitry-bym)

This project is under MIT license. You can obtain the license copy [here](https://github.com/dmitry-bym/JsonKnownTypes/blob/master/LICENSE).

This work using work of James Newton-King, author of Json.NET. https://www.newtonsoft.com/json
