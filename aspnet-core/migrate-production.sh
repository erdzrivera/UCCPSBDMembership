#!/bin/bash
# Production Database Migration Script
# This script runs database migrations against your production PostgreSQL database

if [ -z "$1" ]; then
    echo "Error: Connection string required"
    echo "Usage: ./migrate-production.sh \"<connection-string>\""
    exit 1
fi

CONNECTION_STRING="$1"

echo -e "\033[0;32mRunning database migrations...\033[0m"
echo -e "\033[0;33mTarget: Production PostgreSQL Database\033[0m"

# Set environment variables
export ConnectionStrings__Default="$CONNECTION_STRING"
export ASPNETCORE_ENVIRONMENT="Production"

# Navigate to DbMigrator project
cd src/UCCP.SBD.Membership.DbMigrator

# Run migrations
dotnet run

echo -e "\n\033[0;32mMigrations completed!\033[0m"
echo -e "\033[0;36mYour production database schema is now up to date.\033[0m"
