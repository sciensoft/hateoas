### README
### Script used to run unit tests.
### Usage `integrationtests.sh`

resultPath=$1
if [ -z "$resultPath" ]
then
    resultPath="./../.results"
fi

printf "Running Integration Tests.\n"

# Helper for Integration Tests
dotnet run --project ./../src/Sciensoft.Samples.Products.Api.Presentation/Sciensoft.Samples.Products.Api.Presentation.csproj -v q &
sleep 5 &&

# Tests for Sciensoft.Samples.Products.Api.Infrastructure.IntegrationTests
dotnet test ./../tests/Sciensoft.Samples.Products.Api.Infrastructure.IntegrationTests/Sciensoft.Samples.Products.Api.Infrastructure.IntegrationTests.csproj --logger "trx;logfilename=Sciensoft.Samples.Products.Api.Infrastructure.IntegrationTests.trx" --results-directory $resultPath

# Tests for Sciensoft.Samples.Products.Api.Infrastructure.IntegrationTests
dotnet test ./../tests/SuitSuply.Products.Web.Presentation.IntegrationTests/SuitSuply.Products.Web.Presentation.IntegrationTests.csproj --logger "trx;logfilename=SuitSuply.Products.Web.Presentation.IntegrationTests.trx" --results-directory $resultPath

printf "\nPress [enter] to exit console."
read