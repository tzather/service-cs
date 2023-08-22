clear

# an environemnt value for "sql_password" must be set

# define variables
SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)

build-server() {
  docker container rm --force dev-mssql
  docker run --env "ACCEPT_EULA=Y" --env "SA_PASSWORD=$sql_password" --publish 1433:1433 --name dev-mssql --hostname dev-mssql --detach mcr.microsoft.com/mssql/server:2022-latest
}

build-database() { (
  cd "$SCRIPT_DIR/$1"
  dotnet ef database drop --force
  # dotnet ef database update 0
  rm -rf Migrations
  dotnet ef migrations add Initial
  dotnet build
  dotnet ef database update
  dotnet ef migrations script --idempotent --output Migrations/db_script.sql
); }

build-database-log() {
  docker exec dev-mssql /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $sql_password -d master -Q "
    if exists (select 1 from sys.databases where name = 'Log')
      drop database Log;
    go
    create database Log;
    go
    use Log;
    go
    create table dbo.Log (
        Updated datetime2 null
      , LogLevel nvarchar(100) null
      , Category nvarchar(100) null
      , EventId nvarchar(100) null
      , EventName nvarchar(100) null
      , State nvarchar(max) null
      , Exception nvarchar(max) null
    )"
}

start-api() { (
  cd "$SCRIPT_DIR/$1"
  dotnet run
); }

# build-server
# sleep 5 # Waits 5 seconds.
build-database-log
build-database "Identity.Api"
