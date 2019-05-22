# SuitSupply Assessment

This assessment for *Senior .NET Developer* position consists of a WebApp and WebApi in a layered architecture and some other libraries.

**Technologies, Features and Concepts implemented**

*Below are some of the technologies, features and concepts implemented fully or partially for the sake of acknowledging my knowledge.*

 - CQS - Command Quert Separation
 - Concurrency Checks for Data consistency
 - Entity-Framework with SQLite
   - EF Code-First Migrations
 - Protective Programming
 - OOP Features
   - Encapsulation and protection of invariants
   - Polymorphism
   - And all the rest like composition, inheritance and some others...
 - Repository
 - Dependency Inversion
 - SOLID
 - Testing with xUnit
   - Unit tests
   - Integration tests
   - Mocks
   - FluentAssertions
 - Bash Scripts for CI/CD pipeline
 - Semantic Logging
 - Async/Await and TPL (Task Parallel Library)
 - Layered Architecture (DDD stratigic reference)
 - RESTful API
 - HATEOAS from Rest Uniform Interface constraint (to achieve Richardson maturity model)
   - Using my own open-source project
 - CRUD Application
 - Exception Handling
 - Content Negotiation **[WIP]**
 - Docker support **[WIP]**
 - And other things
 

## Startup

To start both applications, please run `startup.sh` located in *[./scripts](./scripts)* folder.

```
startup.sh
```

This will launch both the RESTful Api and Frontend App.


## Projects

The solution `SuitSupply.Products.sln` consists of the following projects:

 - **libraries**
   - SuitSupply.Products.Abstractions
   - SuitSupply.Products.AspNetCore
   - SuitSupply.Products.Infrastructure
 - **src**
   - SuitSuply.Products.Web.Application
   - SuitSupply.Products.Api.Application
   - SuitSupply.Products.ViewModels
   - SuitSupply.Products.Api.Infrastructure
   - SuitSupply.Products.Api.Infrastructure.EfMigrations
   - SuitSuply.Products.Web.Presentation
   - SuitSupply.Products.Api.Presentation
 - **tests**
   - SuitSupply.Products.Application.Tdd
   - SuitSupply.Products.AspNetCore.Tdd
   - SuitSuply.Products.Web.Presentation.IntegrationTests
   - SuitSupply.Products.Api.Infrastructure.IntegrationTests

### Nuget Packages

 - SuitSupply.Products.Abstractions
 - SuitSupply.Products.AspNetCore
 - SuitSupply.Products.Infrastructure

For a real-case scenario, generation of nuget packages for some common code would be desirable, based on that I've created a script to generate nuget packages from the *libraries* projects.

To generate nuget packages run `package.sh` with an optional parameter to set **Package Version**. The script is located in *[./scripts](./scripts)* folder.

```
package.sh 2.1.3
```


## Code Coverage

Code coverage with [coverlet](https://github.com/tonerdo/coverlet/). To run code coverage, please run `coverage.sh` with an optional parameter to set test results location. The script is located in *[./scripts](./scripts)* folder.

```
coverage.sh <[results-path]>
```


## Testing

Some of the layers were tested using `xunit` tool.

### Unit Tests

To run unit tests, please run `unittests.sh` with an optional parameter to set test results location. THe script is located in *[./scripts](./scripts)* folder.

```
unittests.sh <[results-path]>
```

### Integration Tests

To run integration tests, please run `integrationtests.sh` with an optional parameter to set test results location. THe script is located in *[./scripts](./scripts)* folder.

``` 
integrationtests.sh <[results-path]>
```