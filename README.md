# JsonKnownTypes .Net Standard
[![nuget](https://img.shields.io/badge/nuget-v0.5.2-orange?style=flat-square)](https://www.nuget.org/packages/JsonKnownTypes)
[![downloads](https://img.shields.io/nuget/dt/JsonKnownTypes?style=flat-square)](https://www.nuget.org/packages/JsonKnownTypes)
[![lisence](https://img.shields.io/badge/lisence-MIT-green?style=flat-square)](https://github.com/dmitry-bym/JsonKnownTypes/blob/master/LICENSE)

Helps to serialize and deserialize polymorphic types. Adds discriminator to json.

- [Documentation](#Documentation)
- [License](#License)

## Requirements
- NET Standard 2.0 compatible project
- [Json.NET](https://github.com/JamesNK/Newtonsoft.Json) by Newtonsoft

## Documentation
### Getting started
The simplest way to use it is to add one attribute to base class or interface:
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
### Using with Interfaces or Abstract classes
Also, you can use it with interfaces or abstract classes:
#### Interface
```c#
[JsonConverter(typeof(JsonKnownTypesConverter<IInterface>))]
public interface IInterface  { ... }
 
public class ChildClass : IInterface  { ... }
```
Json representation:
```
{ ... "$type":"ChildClass" }
```
#### Abstract class
```c#
[JsonConverter(typeof(JsonKnownTypesConverter<AbstractClass>))]
public abstract class AbstractClass  { ... }
 
public class ChildClass : AbstractClass  { ... }
```
Json representation:
```
{ ... "$type":"ChildClass" }
```
### JsonKnownType
If you need to add a custom discriminator use `JsonKnownType` attribute.  
By default, `"$type"` is used for discriminator property name, if you need to change that use `JsonDiscriminator` attribute.
```c#
[JsonConverter(typeof(JsonKnownTypesConverter<BaseClass>))]
[JsonDiscriminator(Name = "myType")] //add custom discriminator name
[JsonKnownType(typeof(BaseClass1Heir))] //could be deleted if you didn't turn off UseClassNameAsDiscriminator
[JsonKnownType(typeof(BaseClass2Heir), "myDiscriminator")]
public class BaseClass { ... }
  
public class BaseClass1Heir : BaseClass  { ... }
 
public class BaseClass2Heir : BaseClass  { ... }
```
Json representation:
```
{ ... , "myType":"BaseClass" }

{ ... , "myType":"BaseClass1Heir" }

{ ... , "myType":"myDiscriminator" }
```
### JsonKnownThisType
Add a discriminator for type which is used with it:
```c#
[JsonConverter(typeof(JsonKnownTypesConverter<BaseClass>))]
[JsonKnownThisType("do_you_know_that")]
public class BaseClass { ... }

[JsonKnownThisType("html_is_programming_language")]
public class BaseClass1Heir : BaseClass  { ... }
  
[JsonKnownThisType("just_joke=)")]
public class BaseClass2Heir : BaseClass  { ... }
```
Json representation:
```
{ ... , "$type":"do_you_know_that" }

{ ... , "$type":"html_is_programming_language" }

{ ... , "$type":"just_joke=)" }
```
### Configuration
To change default discriminator settings use:
```c#
JsonKnownTypesSettingsManager.DefaultDiscriminatorSettings = new JsonDiscriminatorSettings
{
    DiscriminatorFieldName = "name",
    UseClassNameAsDiscriminator = false
};
```
> `DiscriminatorFieldName` change default `"$type"` name to yours  

> If `UseClassNameAsDiscriminator` is `false` you should add `JsonKnownType` or `JsonKnownThisType` attribute for each relative class manually otherwise it will throw an exception.

If you want to add derived types from another assembly you can set custom type resolver `Func<Type, Type[]>`:
```c#
JsonKnownTypesSettingsManager.GetDerivedByBase = parent => parent.Assembly.GetTypes();
```
### Use manually
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
> You have to pass a converter directly to method if you do not use `JsonConverter` attribute.
### Fallback type deserialization
Normally you will receive an exception during deserialization of models marked with unknown or
unspecified type discriminator. If you need an exception-free way you can use `JsonKnownTypeFallback` attribute.
In that case entities marked with unknown or missing type discriminator will be deserialized to specified fallback type.
See example.

Assume you have a bunch of events coming from webhook/frontend/etc.:
```json5
[
  { id: "abc", opType: "op_start" },
  { id: "bcd", opType: "on_save" },
  { id: "cde", opType: "op_update" },
  { id: "def", opType: "op_end" }
]
```
Then in your application you can do the following to handle only events you are only interested of:
```c#
[JsonConverter(typeof(JsonKnownTypesConverter<OperationBase>))]
[JsonDiscriminator(Name = "opType")]
[JsonKnownType(typeof(OperationStarted), "op_start")]
[JsonKnownType(typeof(OperationEnded), "op_end")]
[JsonKnownTypeFallback(typeof(UnsupportedOperation))]
public abstract class OperationBase
{
    public string Id { get; set; }
}

public class OperationStarted : OperationBase { }
public class OperationEnded : OperationBase { }
public class UnsupportedOperation : OperationBase { }
```

## License

Authored by: Dmitry Kaznacheev (dmitry-bym)

This project is under MIT license. You can obtain the license copy [here](https://github.com/dmitry-bym/JsonKnownTypes/blob/master/LICENSE).

This work using work of James Newton-King, author of Json.NET. https://www.newtonsoft.com/json
