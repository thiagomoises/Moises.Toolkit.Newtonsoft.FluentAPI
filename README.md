# Moises.Toolkit.Newtonsoft.FluentAPI

[![Build status](https://ci.appveyor.com/api/projects/status/76s6v9q5qaav2asu?svg=true)](https://ci.appveyor.com/project/thiagomoises/moises-toolkit-mercadopago-netcore)
[![NuGet](https://img.shields.io/nuget/v/Moises.Toolkit.Newtonsoft.FluentAPI.svg)](https://www.nuget.org/packages/Moises.Toolkit.Newtonsoft.FluentAPI/)
[![CodeFactor](https://www.codefactor.io/repository/github/thiagomoises/moises.toolkit.newtonsoft.fluentapi/badge)](https://www.codefactor.io/repository/github/thiagomoises/moises.toolkit.newtonsoft.fluentapi)


## What is Moises.Toolkit.Newtonsoft.FluentAPI?
Fluent, annotations-less, compile safe, automated, convention-based mappings for Json.NET.

## Where can I get it?

Install using the [Moises.Toolkit.Newtonsoft.FluentAPI NuGet package](https://www.nuget.org/packages/Moises.Toolkit.Newtonsoft.FluentAPI):

```
dotnet add package Moises.Toolkit.Newtonsoft.FluentAPI
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
Obs: [consider the test model](https://github.com/thiagomoises/Moises.Toolkit.Newtonsoft.FluentAPI/blob/master/test/Newtonsoft.FluentAPI.Tests/ModelTest.cs)

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

