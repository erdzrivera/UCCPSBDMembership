# Production Database Migration Script
# This script runs database migrations against your production PostgreSQL database

param(
    [Parameter(Mandatory=$true)]
    [string]$ConnectionString
)

Write-Host "Running database migrations..." -ForegroundColor Green
Write-Host "Target: Production PostgreSQL Database" -ForegroundColor Yellow

# Set environment variable for connection string
$env:ConnectionStrings__Default = $ConnectionString
$env:ASPNETCORE_ENVIRONMENT = "Production"

# Navigate to DbMigrator project
Set-Location -Path "src\UCCP.SBD.Membership.DbMigrator"

# Run migrations
dotnet run

Write-Host "`nMigrations completed!" -ForegroundColor Green
Write-Host "Your production database schema is now up to date." -ForegroundColor Cyan
