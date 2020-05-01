# Sciensoft.Hateoas

![Sciensoft.Hateoas Build Status](https://dev.azure.com/Sciensoft/Sciensoft/_apis/build/status/Sciensoft.Hateoas?branchName=master)

A library to help you achieve HATEOAS using a fluent language and lambda expression for configuring your ASP.NET Core WebApi apps. It's based on REST application architecture style **Uniform Interface** constraint _hypermedia as the engine of application state (HATEOAS)_.

**The good thing is, there is no need to inheritance or additional code in your models or addition of extra result filters to support its functionality. It all come beautifully out of the box with `Sciensoft.Hateoas`**.

Sciensoft.Hateoas threats lambda as first-class citizen, so your configuration starts with a lambda expression. This library DO NOT inforce <a href="https://medium.com/extend/what-is-rest-a-simple-explanation-for-beginners-part-2-rest-constraints-129a4b69a582" target="_blank">REST constraints</a> or <a href="https://martinfowler.com/articles/richardsonMaturityModel.html" target="_blank">Richardson Maturity Level</a> and this has to be done by you, Sciensoft.Hateoas helps you only with the implementation of HATEOAS in your resource.

Learn more about RESTful API <a href="https://restfulapi.net/" target="_blank">here</a> and Lambda Expressions <a href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions" target="_blank">here</a>.

## Get Started

Sciensoft.Hateoas can be installed using <a href="https://www.nuget.org/packages/Sciensoft.Hateoas/" target="_blank">Nuget</a> package manager or `dotnet` CLI.

```bash
Install-Package Sciensoft.Hateoas
```

### Configuration

Using a fluent language, it can be configured easily adding the service to .NET dependency inversion pipeline.

```csharp
public void ConfigureServices(IServiceCollection services)
{
  services
    .AddMvc()
    .AddLinks(policy =>
    {
      policy
        .AddPolicy<BookViewModel>(model =>
        {
          model
            .AddSelf(m => m.Id, "This is a GET self link.")
            .AddRoute(m => m.Id, BookController.UpdateBookById)
            .AddRoute(m => m.Id, BookController.DeleteBookById)
            .AddCustomPath(m => m.Id, "Edit", method: HttpMethods.Post, message: "Edits resource")
            .AddCustomPath(m => $"/change/other/path/?id={m.Id}", "CustomLink1", method: HttpMethods.Post, message: "Any operation in your resource.")
            .AddCustomPath(m => $"other/path/?author={m.Author}", "CustomLink2", method: HttpMethods.Put, message: "Any operation in your resource.");
        });
    });
}
```

Here is how your constroller looks like, no additional injection or attribute decoration is required. Please check our [Sample Project](./samples/Sciensoft.Hateoas.WebSample) out!

```csharp
[Route("api/books")]
public class BookController : ControllerBase
{
    public const string UpdateBookById = nameof(UpdateBookById);
    public const string DeleteBookById = nameof(DeleteBookById);

    [HttpGet]
    public ActionResult<IEnumerable<BookViewModel>> Get()
    { /* Code omitted for simplicity */ }

    [HttpGet("{id:guid}")]
    public ActionResult<BookViewModel> Get(Guid id)
    { /* Code omitted for simplicity */ }

    [HttpPost]
    public IActionResult Post([FromBody] BookViewModel book)
    { /* Code omitted for simplicity */ }

    [HttpPut("{id:guid}", Name = UpdateBookById)]
    public IActionResult Put(Guid id, [FromBody] BookViewModel book)
    { /* Code omitted for simplicity */ }

    [HttpDelete("{id:guid}", Name = DeleteBookById)]
    public IActionResult Delte(Guid id)
    { /* Code omitted for simplicity */ }
}
```

**Json Result:**

```json
{
    "Id": "8f46d29e-6c0d-4511-85e7-b1d7ae42934a",
    "Title": "The Girl Who Lived: A Thrilling Suspense Novel",
    "Author": "Christopher Greyson",
    "Tags": [
        "Fiction",
        "Crime",
        "Murder",
        "Thriller"
    ],
    "Reference": null,
    "links": [
        {
            "method": "GET",
            "uri": "http://your-domain.io/api/books/8f46d29e-6c0d-4511-85e7-b1d7ae42934a",
            "relation": "Self"
        },
        {
            "method": "PUT",
            "uri": "http://your-domain.io/api/books/8f46d29e-6c0d-4511-85e7-b1d7ae42934a",
            "relation": "UpdateBookById"
        },
        {
            "method": "DELETE",
            "uri": "http://your-domain.io/api/books/8f46d29e-6c0d-4511-85e7-b1d7ae42934a",
            "relation": "DeleteBookById"
        },
        {
            "method": "POST",
            "uri": "http://your-domain.io/api/books/8f46d29e-6c0d-4511-85e7-b1d7ae42934a",
            "relation": "Edit"
        },
        {
            "method": "POST",
            "uri": "http://your-domain.io/change/other/path/%3fid=8f46d29e-6c0d-4511-85e7-b1d7ae42934a",
            "relation": "CustomLink1"
        },
        {
            "method": "PUT",
            "uri": "http://your-domain.io/api/books/8f46d29e-6c0d-4511-85e7-b1d7ae42934a/other/path/%3fauthor=Christopher Greyson",
            "relation": "CustomLink2"
        }
    ]
}
```

## Features

- Self link link generation
- Named Route link generation
- Custom link generation with support to path override
- Configuration with Lambda Expression
- Attribute Routing support
- Conventional Routing support

### Roadmap

- Add support to extending link generation
- Add support to external links configuration
- Add support to bypass model link generation
- Add support to .NET Authorization
- Add support to Content Negotiation type in the read-model

## Contributions

Before start contributing, check our [CONTRIBUTING] guideline out, also, before doing a major change, have a look to the existing Issues and Pull Requests, one of them may be tackling the same thing.

## Issues

To open an issue or even suggest a feature, please use the [Issues] tab.

## License

Copyright 2019 Sciensoft

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at [http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

[## LINKS ##]: ----------------------------------------------------------

[Learn-RestfulApi]:https://restfulapi.net/
[Lambda-Expressions]:https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions
[Richardson-Maturity-Level]:https://martinfowler.com/articles/richardsonMaturityModel.html
[REST-Constraints]:https://medium.com/extend/what-is-rest-a-simple-explanation-for-beginners-part-2-rest-constraints-129a4b69a582
[CONTRIBUTING]: ./CONTRIBUTING.md
[Issues]: ./../../issues
