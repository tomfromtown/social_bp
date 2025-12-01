# PostgreSQL Docker Configuration

This directory contains PostgreSQL initialization scripts and configuration for the Docker setup.

## Files

- **`init.sql`**: Database initialization script that runs when the PostgreSQL container starts for the first time
- **`Dockerfile`**: Custom PostgreSQL Dockerfile (optional, currently using official image)

## Docker Compose Configuration

PostgreSQL is configured in `docker-compose.yml` with:

- **Image**: `postgres:16-alpine` (lightweight Alpine-based PostgreSQL 16)
- **Database**: `socialmedia`
- **User**: `postgres`
- **Password**: `postgres` (change in production!)
- **Port**: `5432`
- **Volume**: Persistent data storage in Docker volume `postgres_data`
- **Health Check**: Ensures PostgreSQL is ready before backend starts

## Initialization

The `init.sql` script runs automatically when the container is first created. Entity Framework Core migrations handle the actual table creation.

## Data Persistence

PostgreSQL data is stored in a Docker volume (`postgres_data`), which persists even if the container is removed. To completely remove the database:

```bash
docker-compose down -v
```

## Connection String

The backend connects to PostgreSQL using:
```
Host=postgres;Port=5432;Database=socialmedia;Username=postgres;Password=postgres
```

Note: `postgres` is the service name in docker-compose, which resolves to the container's internal IP.

## Production Considerations

For production, you should:

1. **Change the password**: Use a strong password via environment variables or secrets
2. **Use secrets management**: Store credentials in Docker secrets or environment files
3. **Enable SSL**: Configure SSL connections for security
4. **Backup strategy**: Set up regular database backups
5. **Resource limits**: Configure memory and CPU limits
6. **Network security**: Restrict PostgreSQL port exposure

## Custom Configuration

To add custom PostgreSQL configuration, create a `postgresql.conf` file and mount it:

```yaml
volumes:
  - ./backend/postgres/postgresql.conf:/etc/postgresql/postgresql.conf
```

Then update the command in docker-compose.yml to use the custom config file.

