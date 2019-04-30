# moisesToolkit.Newtonsoft.FluentAPI

[![Build status](https://ci.appveyor.com/api/projects/status/eb1lp2yq2b1lhh3k?svg=true)](https://ci.appveyor.com/project/thiagomoises/moisestoolkit-newtonsoft-fluentapi) 
[![NuGet](https://img.shields.io/nuget/v/moisesToolkit.Newtonsoft.FluentAPI.svg)](https://www.nuget.org/packages/moisesToolkit.Newtonsoft.FluentAPI/)


## What is moisesToolkit.Newtonsoft.FluentAPI?
Fluent, annotations-less, compile safe, automated, convention-based mappings for Json.NET.

## Where can I get it?

Install using the [moisesToolkit.Newtonsoft.FluentAPI NuGet package](https://www.nuget.org/packages/moisesToolkit.Newtonsoft.FluentAPI):

```
dotnet add package moisesToolkit.Newtonsoft.FluentAPI
```
Obs: compatible with .NetStandard, .NetCore and .NetFramework

## How do I use it?

*First, create your map:

```
public class TestJsonTypeConfiguration : IJsonTypeConfiguration<UserTest>
{
    public void Configure(JsonTypeBuilder<UserTest> jsonTypeBuilder)
    {
        jsonTypeBuilder.Property(x => x.FirstName)
            .HasFieldName("first_name");

        jsonTypeBuilder.Property(x => x.LastName)
            .HasFieldName("last_name");

        jsonTypeBuilder.Property(x => x.Age)
            .HasFieldName("user_age");

        jsonTypeBuilder.Property(x => x.Status)
            .HasFieldName("status")
            .HasConverter(new StringEnumConverter());

        jsonTypeBuilder.Property(x => x.City)
            .IsIgnored();
    }
}
```
Obs: [consider the test model](https://github.com/thiagomoises/moisesToolkit.Newtonsoft.FluentAPI/blob/master/test/Newtonsoft.FluentAPI.Tests/ModelTest.cs)

*After, create your `FluentContractResolver` and set the created map:

```
FluentContractResolver _fcr = new FluentContractResolver();
_fcr.AddConfiguration(new TestJsonTypeConfiguration());
```
*At the end, add your `FluentContractResolver` in `JsonSerializerSettings`:

- Global 
```
JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    ContractResolver = _fcr
};
var model = JsonConvert.DeserializeObject<UserTest>(stringBuilder.ToString());
```

- Individual 
```
var settings = new JsonSerializerSettings
{
    Formatting = Newtonsoft.Json.Formatting.Indented,
    ContractResolver = _fcr
};

var model = JsonConvert.DeserializeObject<UserTest>(stringBuilder.ToString(), settings);

```

- In AspNetCore

```
services.AddMvc()
    .AddJsonOptions(options =>
    {
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
        options.SerializerSettings.Formatting = Formatting.Indented;
        options.SerializerSettings.ContractResolver = new FluentContractResolver();
     });
            
```

## Contributors and references
Code created from [discussion in stackoverflow](https://stackoverflow.com/questions/26801453/fluent-converters-mappers-with-json-net/38155903), being made the necessary improvements and corrections.

