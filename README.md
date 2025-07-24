# Moneyfy.Persistence
Persistence layer for Moneyfy Solution

## Create and publish package
```powershell
$version="1.0.2"
$owner="dotnetmoondodev"
$gh_pat=""

dotnet pack .\persistence --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/Moneyfy.Persistence -o packages

dotnet nuget push packages\Moneyfy.Persistence.$version.nupkg --api-key $gh_pat --source "github"
```