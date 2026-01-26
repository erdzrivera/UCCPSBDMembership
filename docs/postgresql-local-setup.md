# PostgreSQL Local Setup Script

## Prerequisites
- Docker Desktop running
- PostgreSQL container already created (postgres-membership)

## Step 1: Verify PostgreSQL is Running
```powershell
docker ps | Select-String "postgres-membership"
```

If not running, start it:
```powershell
docker start postgres-membership
```

## Step 2: Create Database Schema

The easiest way is to let the application create the schema on first run.

### Option A: Let AuthServer Create Schema (Recommended)

1. Stop your current AuthServer (Ctrl+C)
2. Run it again:
```powershell
cd aspnet-core\src\UCCP.SBD.Membership.AuthServer
dotnet run
```

ABP will automatically create all tables on first connection!

### Option B: Use DbMigrator (Alternative)

If you want to manually run migrations:

```powershell
cd aspnet-core\src\UCCP.SBD.Membership.DbMigrator
dotnet run
```

**Note**: This might fail if migrations don't exist yet. Option A is easier.

## Step 3: Verify Database

Connect to PostgreSQL to verify tables were created:

```powershell
docker exec -it postgres-membership psql -U postgres -d membership -c "\dt"
```

You should see ABP framework tables (AbpUsers, AbpRoles, etc.) and your custom tables (AppMembers, AppMembershipTypes, AppOrganizations).

## Step 4: Restart Your Services

Now restart all services to use PostgreSQL:

```powershell
# Terminal 1 - AuthServer
cd aspnet-core\src\UCCP.SBD.Membership.AuthServer
dotnet run

# Terminal 2 - API
cd aspnet-core\src\UCCP.SBD.Membership.HttpApi.Host
dotnet run

# Terminal 3 - Angular
cd angular
npm start
```

## Troubleshooting

### If you get "database does not exist":
```powershell
docker exec -it postgres-membership psql -U postgres -c "CREATE DATABASE membership;"
```

### If you want to reset and start fresh:
```powershell
# Drop and recreate database
docker exec -it postgres-membership psql -U postgres -c "DROP DATABASE IF EXISTS membership;"
docker exec -it postgres-membership psql -U postgres -c "CREATE DATABASE membership;"
```

### To stop PostgreSQL container:
```powershell
docker stop postgres-membership
```

### To remove PostgreSQL container completely:
```powershell
docker stop postgres-membership
docker rm postgres-membership
```

## Summary

✅ PostgreSQL connection strings already updated in `appsettings.Development.json`  
✅ Docker container running  
✅ Just run AuthServer - it will create the schema automatically!

That's it! ABP handles the database initialization for you.
