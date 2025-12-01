# Entity Framework Code-First Migrations

This project uses **Code-First** approach with Entity Framework Core. The database schema is defined by the C# model classes, and migrations are used to create and update the database.

## Entities

The application has three main entities:

### 1. User
- **Id**: Primary key (auto-increment)
- **Username**: Unique, required, max 50 characters
- **PasswordHash**: Required

### 2. Post
- **Id**: Primary key (auto-increment)
- **AuthorId**: Foreign key to User
- **Content**: Required, max 2000 characters
- **CreatedAt**: Required timestamp
- **Navigation Properties**: 
  - `Author` (User)
  - `Comments` (Collection)
  - `Likes` (Collection)

### 3. Comment
- **Id**: Primary key (auto-increment)
- **PostId**: Foreign key to Post (Cascade delete)
- **AuthorId**: Foreign key to User
- **Content**: Required, max 500 characters
- **CreatedAt**: Required timestamp
- **Navigation Properties**:
  - `Post` (Post)
  - `Author` (User)

### 4. Like (Supporting Entity)
- **Id**: Primary key (auto-increment)
- **PostId**: Foreign key to Post (Cascade delete)
- **UserId**: Foreign key to User
- **Unique Constraint**: (PostId, UserId) - prevents duplicate likes
- **Navigation Properties**:
  - `Post` (Post)
  - `User` (User)

## Database Schema

The database schema is defined in `ApplicationDbContext.cs` using Fluent API configuration:

- **Users** table with unique username index
- **Posts** table with foreign key to Users
- **Comments** table with foreign keys to Posts and Users
- **Likes** table with composite unique index on (PostId, UserId)

## Migrations

### Current Migration

- **InitialCreate**: Creates all tables, indexes, and relationships

### Creating New Migrations

When you modify the model classes (User, Post, Comment, Like), create a new migration:

```bash
cd backend/SocialMedia.Api
dotnet ef migrations add MigrationName
```

### Applying Migrations

Migrations are automatically applied when the application starts (see `Program.cs`). To manually apply migrations:

```bash
dotnet ef database update
```

### Rolling Back Migrations

To rollback to a previous migration:

```bash
dotnet ef database update PreviousMigrationName
```

To remove the last migration (before applying):

```bash
dotnet ef migrations remove
```

## Code-First Workflow

1. **Define Models**: Create or modify C# classes in `Models/` folder
2. **Configure Relationships**: Update `ApplicationDbContext.OnModelCreating()` if needed
3. **Create Migration**: Run `dotnet ef migrations add MigrationName`
4. **Review Migration**: Check the generated migration file in `Migrations/` folder
5. **Apply Migration**: Migration is auto-applied on startup, or run `dotnet ef database update`

## Database Configuration

The database connection string is configured in:
- `appsettings.json` for local development
- Environment variables in Docker (via `docker-compose.yml`)

The application uses PostgreSQL with Entity Framework Core.

## Notes

- Migrations are automatically applied on application startup
- The database is seeded with initial data after migrations are applied
- All foreign key relationships use appropriate delete behaviors:
  - Posts → Users: Restrict (cannot delete user with posts)
  - Comments → Posts: Cascade (delete comments when post is deleted)
  - Comments → Users: Restrict (cannot delete user with comments)
  - Likes → Posts: Cascade (delete likes when post is deleted)
  - Likes → Users: Restrict (cannot delete user with likes)

