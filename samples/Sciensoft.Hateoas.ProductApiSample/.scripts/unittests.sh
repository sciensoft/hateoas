### README
### Script used to run unit tests.
### Usage `unittests.sh`

resultPath=$1
if [ -z "$resultPath" ]
then
    resultPath="./../.results"
fi

printf "Running Unit Tests.\n"

# Tests for Sciensoft.Samples.Products.Abstractions
dotnet test ./../tests/Sciensoft.Samples.Products.AspNetCore.Tdd/Sciensoft.Samples.Products.AspNetCore.Tdd.csproj --logger "trx;logfilename=Sciensoft.Samples.Products.AspNetCore.Tdd.trx" --results-directory $resultPath

# Tests for Sciensoft.Samples.Products.Application.Tdd
dotnet test ./../tests/Sciensoft.Samples.Products.Application.Tdd/Sciensoft.Samples.Products.Application.Tdd.csproj --logger "trx;logfilename=Sciensoft.Samples.Products.Application.Tdd.trx" --results-directory $resultPath

printf "\nPress [enter] to exit console."
read