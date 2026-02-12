param(
    [Parameter(Mandatory=$true)]
    [string]$Name
)

Write-Host "Restoring local tools..."

dotnet tool restore | Out-Null

$tools = dotnet tool list

if ($tools -notmatch "dotnet-ef") {
    Write-Host "Installing dotnet-ef..."
    dotnet tool install dotnet-ef
}

dotnet tool run dotnet-ef migrations add $Name `
 --project Shared `
 --startup-project Api `
 --output-dir Context/Migrations/HumanResource
