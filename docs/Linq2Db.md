# Setup with Docker
To use Linq2DB with a database server hosted via Docker, first create the docker container.
Below are four docker-compose.yml files for four different database providers.

The login information needs to be changed before running the container.

After setting up the container, use your software of choice to connect to the database.

## MariaDB
```
services:
  mariadb:
    image: mariadb:latest
    container_name: mariadb-test
    environment:
      MARIADB_ROOT_PASSWORD: "ROOTPASSWORD"
      MARIADB_DATABASE: "DATABASE_NAME"
      MARIADB_USER: "USERNAME"
      MARIADB_PASSWORD: "USERPASSWORD"
    ports:
      - "3306:3306"
    volumes:
      - mariadb-data:/var/lib/mysql

volumes:
  mariadb-data:
```
## MySQL
```
services:
  mysql:
    image: mysql:latest
    container_name: mysql-test
    environment:
      MYSQL_ROOT_PASSWORD: "ROOTPASSWORD"
      MYSQL_DATABASE: "DATABASE_NAME"
      MYSQL_USER: "USERNAME"
      MYSQL_PASSWORD: "USERPASSWORD"
    ports:
      - "3306:3306"
    volumes:
      - mysql-data:/var/lib/mysql
volumes:
  mysql-data:
```

## MSSQL/ SQL Server
```
services:
  mssql:
    image: mcr.microsoft.com/mssql/server:2025-latest
    container_name: mssql-test
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "PASSWORD"
      MSSQL_PID: "PID"
    ports:
      - "1433:1433"
    volumes:
      - mssql-data:/var/opt/mssql
volumes:
  mssql-data:
```

## PostgreSQL
```
services:
  postgres:
    image: postgres:15.19
    container_name: postgres-test
    environment:
      POSTGRES_DB: "DATABASENAME"
      POSTGRES_USER: "USERNAME"
      POSTGRES_PASSWORD: "USERPASSWORD"
    ports:
      - "5432:5432"
    volumes:
      - postgres-data:/var/lib/postgresql

volumes:
  postgres-data:
```