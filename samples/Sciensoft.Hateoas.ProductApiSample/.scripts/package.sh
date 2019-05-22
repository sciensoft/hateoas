### README
### Script used to restore and publish nuget packages.
### Usage `package.sh 2.0.1`

version=$1
if [ -z "$version" ]
then
    version="1.0.0"
fi

printf "Creating nuget packages with version '$version'.\n"

# Nuget package for Sciensoft.Samples.Products.Abstractions
dotnet pack ./../libraries/Sciensoft.Samples.Products.Abstractions/Sciensoft.Samples.Products.Abstractions.csproj -p:PackageVersion=$version -o ./../.packages/ -c RELEASE --include-symbols --include-source

# Nuget package for Sciensoft.Samples.Products.AspNetCore
dotnet pack ./../libraries/Sciensoft.Samples.Products.AspNetCore/Sciensoft.Samples.Products.AspNetCore.csproj -p:PackageVersion=$version -o ./../.packages/ -c RELEASE --include-symbols --include-source

# Nuget package for Sciensoft.Samples.Products.AspNetCore
dotnet pack ./../libraries/Sciensoft.Samples.Products.Infrastructure/Sciensoft.Samples.Products.Infrastructure.csproj -p:PackageVersion=$version -o ./../.packages/ -c RELEASE --include-symbols --include-source

printf "\nPress [enter] to exit console."
read