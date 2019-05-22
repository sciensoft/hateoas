# Product Restful Api Sample

Sample Restful Api using HATEOAS library. It consists of a WebApp and WebApi in a layered architecture and some other libraries.

## Technologies, Features and Concepts implemented

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

```sh
startup.sh
```

This will launch both the RESTful Api and Frontend App.

## Projects

The solution `Sciensoft.Samples.Products.sln` consists of the following projects:

- **libraries**
  - Sciensoft.Samples.Products.Abstractions
  - Sciensoft.Samples.Products.AspNetCore
  - Sciensoft.Samples.Products.Infrastructure
- **src**
  - Sciensoft.Samples.Products.Web.Application
  - Sciensoft.Samples.Products.Api.Application
  - Sciensoft.Samples.Products.ViewModels
  - Sciensoft.Samples.Products.Api.Infrastructure
  - Sciensoft.Samples.Products.Api.Infrastructure.EfMigrations
  - Sciensoft.Samples.Products.Web.Presentation
  - Sciensoft.Samples.Products.Api.Presentation
- **tests**
  - Sciensoft.Samples.Products.Application.Tdd
  - Sciensoft.Samples.Products.AspNetCore.Tdd
  - Sciensoft.Samples.Products.Web.Presentation.IntegrationTests
  - Sciensoft.Samples.Products.Api.Infrastructure.IntegrationTests

### Nuget Packages

- Sciensoft.Samples.Products.Abstractions
- Sciensoft.Samples.Products.AspNetCore
- Sciensoft.Samples.Products.Infrastructure

For a real-case scenario, generation of nuget packages for some common code would be desirable, based on that I've created a script to generate nuget packages from the *libraries* projects.

To generate nuget packages run `package.sh` with an optional parameter to set **Package Version**. The script is located in *[./scripts](./scripts)* folder.

```sh
package.sh 2.1.3
```

## Code Coverage

Code coverage with [coverlet](https://github.com/tonerdo/coverlet/). To run code coverage, please run `coverage.sh` with an optional parameter to set test results location. The script is located in *[./scripts](./scripts)* folder.

```sh
coverage.sh <[results-path]>
```

## Testing

Some of the layers were tested using `xunit` tool.

### Unit Tests

To run unit tests, please run `unittests.sh` with an optional parameter to set test results location. THe script is located in *[./scripts](./scripts)* folder.

```sh
unittests.sh <[results-path]>
```

### Integration Tests

To run integration tests, please run `integrationtests.sh` with an optional parameter to set test results location. THe script is located in *[./scripts](./scripts)* folder.

```sh
integrationtests.sh <[results-path]>
```