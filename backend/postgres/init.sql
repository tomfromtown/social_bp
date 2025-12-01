-- Initialize database schema
-- This script runs automatically when PostgreSQL container starts for the first time

-- Create database if it doesn't exist (usually created by POSTGRES_DB env var)
-- SELECT 'CREATE DATABASE socialmedia'
-- WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'socialmedia')\gexec

-- Grant privileges
GRANT ALL PRIVILEGES ON DATABASE socialmedia TO postgres;

-- Note: Entity Framework migrations will create the actual tables
-- This file is for any custom initialization if needed

