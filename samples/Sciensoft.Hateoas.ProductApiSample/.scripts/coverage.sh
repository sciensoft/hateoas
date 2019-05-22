### README
### Script used to run unit tests.
### Usage `integrationtests.sh`

# Install coverlet, for more details visit https://github.com/tonerdo/coverlet
#dotnet tool install --global coverlet.console

resultPath=$1
if [ -z "$resultPath" ]
then
    resultPath="./../.coverage/coverage"
fi

# Helper for Integration Tests
dotnet run --project ./../src/Sciensoft.Samples.Products.Api.Presentation/Sciensoft.Samples.Products.Api.Presentation.csproj -v q &
sleep 5 &&

# Code-Coverage for Sciensoft.Samples.Products.Application.Tdd
project=./../tests/Sciensoft.Samples.Products.Application.Tdd/Sciensoft.Samples.Products.Application.Tdd.csproj
dotnet test $project --no-build -p:CollectCoverage=true -p:CoverletOutput="$resultPath-SuitSupply_Products_Application_Tdd" -p:CoverletOutputFormat=opencover -p:Exclude="[xunit*]*"

# Code-Coverage for Sciensoft.Samples.Products.AspNetCore.Tdd
project=./../tests/Sciensoft.Samples.Products.AspNetCore.Tdd/Sciensoft.Samples.Products.AspNetCore.Tdd.csproj
dotnet test $project --no-build -p:CollectCoverage=true -p:CoverletOutput="$resultPath-SuitSupply_Products_AspNetCore_Tdd" -p:CoverletOutputFormat=opencover -p:Exclude="[xunit*]*"

# Code-Coverage for SuitSuply.Products.Web.Presentation.IntegrationTests
project=./../tests/SuitSuply.Products.Web.Presentation.IntegrationTests/SuitSuply.Products.Web.Presentation.IntegrationTests.csproj
dotnet test $project --no-build -p:CollectCoverage=true -p:CoverletOutput="$resultPath-SuitSuply_Products_Web_Presentation_IntegrationTests" -p:CoverletOutputFormat=opencover -p:Exclude="[xunit*]*"

# Code-Coverage for Sciensoft.Samples.Products.Api.Infrastructure.IntegrationTests
project=./../tests/Sciensoft.Samples.Products.Api.Infrastructure.IntegrationTests/Sciensoft.Samples.Products.Api.Infrastructure.IntegrationTests.csproj
dotnet test $project --no-build -p:CollectCoverage=true -p:CoverletOutput="$resultPath-SuitSupply_Products_Api_Infrastructure_IntegrationTests" -p:CoverletOutputFormat=opencover -p:Exclude="[xunit*]*"

printf "\nPress [enter] to exit console."
read