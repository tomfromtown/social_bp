# PostgreSQL Dockerfile with custom configuration
FROM postgres:16-alpine

# Copy initialization scripts
COPY init.sql /docker-entrypoint-initdb.d/

# Copy custom PostgreSQL configuration (optional)
# COPY postgresql.conf /etc/postgresql/postgresql.conf

# Set environment variables
ENV POSTGRES_DB=socialmedia
ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=postgres

# Expose PostgreSQL port
EXPOSE 5432

# Use default entrypoint from postgres image
# The postgres image automatically runs scripts in /docker-entrypoint-initdb.d/

