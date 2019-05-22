### README
### Script used to restore and publish nuget packages.
### Usage `startup.sh`

printf "Starting up WebApi and WebApp.\n"

dotnet run --project ./../src/Sciensoft.Samples.Products.Api.Presentation/Sciensoft.Samples.Products.Api.Presentation.csproj --launch-profile Sciensoft.Samples.Products.Api.Presentation &
dotnet run --project ./../src/SuitSuply.Products.Web.Presentation/SuitSuply.Products.Web.Presentation.csproj --launch-profile SuitSuply.Products.Web.Presentation