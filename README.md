# Sciensoft.Hateoas

A library to help you achieve HATEOAS using an fluent language and lambda expression for configuring your ASP.NET Core WebApi apps. It's based on REST application architecture style **Uniform Interface** constraint _hypermedia as the engine of application state (HATEOAS)_.

**The good thing is, there is no need to inheritance or additional code in your models or addition of extra result filters to support its functionality. It all come beautifully out of the box with `Sciensoft.Hateoas`**.

Sciensoft.Hateoas threats lambda as first-class citizen, so your configuration starts with a lambda expression. This library DO NOT inforce [REST constraints][REST-Constraints] or [Richardson Maturity Level][Richardson-Maturity-Level] and this has to be done by you, Sciensoft.Hateoas helps you only with the implementation of HATEOAS in your resource.

Learn more about RESTful API [here][Learn-RestfulApi] and Lambda Expressions [here][Lambda-Expressions].

## Get Started

Sciensoft.Hateoas can be installed using Nuget package manager or `dotnet` CLI.

```:
Install Sciensoft.Hateoas
```


### Configuration

Using a fluent language, it can be configured easily adding the service to .NET dependency inversion pipeline.

```:
public void ConfigureServices(IServiceCollection services)
{
  services
    .AddMvc()
    .AddLinks(policy =>
    {
      policy
        .AddPolicy<SampleViewModel>(model =>
        {
          model
            .AddSelf(m => m.Id, "This is a GET self link.")
            .AddCustom(m => m.Id, "Edit", method: HttpMethods.Put)
            .AddCustom(m => $"/move/resource/state/?id={m.Id}", "MoveResourceState", method: HttpMethods.Get, message: "Any operation in your resource.")
            .AddRoute(m => m.Id, SampleController.GetWithId);
        });
    });
}
```

**Json Result Sample**

```:
{
    "id": "571be3ce-7ac5-4d99-9872-c6bc868db092",
    "name": "Hello Sample View",
    "samples": [
        "A",
        "B",
        "C"
    ],
    "links": [
        {
            "method": "GET",
            "uri": "http://localhost:52350/api/samples/571be3ce-7ac5-4d99-9872-c6bc868db092",
            "relation": "Self"
        },
        {
            "method": "PUT",
            "uri": "http://localhost:52350/api/samples/571be3ce-7ac5-4d99-9872-c6bc868db092",
            "relation": "Edit"
        },
        {
            "method": "GET",
            "uri": "http://localhost:52350/api/samples/move/resource/state/?id=571be3ce-7ac5-4d99-9872-c6bc868db092",
            "relation": "MoveResourceState",
            "message": "Any operation in your resource."
        },
        {
            "uri": "http://localhost:52350/api/samples/api/samples/571be3ce-7ac5-4d99-9872-c6bc868db092",
            "relation": "GetWithId"
        }
    ]
}
```


## Features

 - Hateoas configuration with Lambda Expression

### Roadmap

 - Add support for Absolute or Relative links configuration
 - Add support for .NET Authorization
 - Set repository to public on Git
 - Create project website


## Constributions

You are welcome to contribute to this project, please clone this repository and go ahead working in your feature branch, after just submit a Pull Request.

Before doing a major change, have a look to the existing Pull Requests, one of them may be tackling the same thing.

A good advice to starting contributing is helping me out with existing issues.


## Issues

To open an issue or even suggest a feature, please use the [Issues](./issues) tab.


## License

Copyright 2019 Sciensoft

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at [http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)


## TODOs

It's a better implementation and expectation of '[RiskFirst.Hateoas][RiskFirst.Hateoas]' functionality.


[RiskFirst.Hateoas]:https://github.com/riskfirst/riskfirst.hateoas/blob/master/README.md


[Learn-RestfulApi]:https://restfulapi.net/
[Lambda-Expressions]:https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions
[Richardson-Maturity-Level]:https://martinfowler.com/articles/richardsonMaturityModel.html
[REST-Constraints]:https://medium.com/extend/what-is-rest-a-simple-explanation-for-beginners-part-2-rest-constraints-129a4b69a582