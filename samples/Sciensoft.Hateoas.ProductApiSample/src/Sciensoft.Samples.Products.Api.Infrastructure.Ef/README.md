# Ef Migrations

Project used to run Entity-Framework migrations using dotnet-ef tools.

## Add

Used to create new migrations

```
dotnet ef migrations add <migrationName> --project <projectToKeepMigrations>
```

## Update

Used to apply migrations to the database

```
dotnet ef database update
```