# Social Media Backend

A .NET Core Web API backend implementing **CQRS (Command Query Responsibility Segregation)** pattern with Clean Architecture.

## Project Structure

The backend is organized into multiple projects following Clean Architecture principles:

### SocialMedia.Domain
**Purpose**: Contains core business entities and domain logic.

- `Entities/`: Domain entities (User, Post, Comment, Like)
- No dependencies on other projects
- Pure business logic

### SocialMedia.Application
**Purpose**: Contains application logic, CQRS commands/queries, and DTOs.

- `Commands/`: Write operations (Login, CreatePost, AddComment)
- `Queries/`: Read operations (GetPosts)
- `DTOs/`: Data Transfer Objects
- `Interfaces/`: Application layer interfaces
- Uses MediatR for CQRS implementation
- References: Domain

### SocialMedia.Infrastructure
**Purpose**: Contains data access and external service implementations.

- `Data/`: Entity Framework Core DbContext and configurations
- `SeedData.cs`: Database seeding logic
- References: Application, Domain

### SocialMedia.Api
**Purpose**: Presentation layer - HTTP API controllers and configuration.

- `Controllers/`: API endpoints
- `Program.cs`: Application startup and configuration
- References: Application, Infrastructure

## CQRS Pattern

### Commands (Write Operations)
Commands represent operations that modify state:

- **LoginCommand**: Authenticate user → Returns JWT token
- **CreatePostCommand**: Create new post → Returns PostDto
- **AddCommentCommand**: Add comment to post → Returns CommentDto

### Queries (Read Operations)
Queries represent operations that read data:

- **GetPostsQuery**: Retrieve all posts with comments and likes → Returns List<PostDto>

### Flow
1. Controller receives HTTP request
2. Controller creates Command/Query
3. Controller sends Command/Query via MediatR
4. Handler processes Command/Query
5. Handler uses IApplicationDbContext to access data
6. Handler returns result
7. Controller returns HTTP response

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL (or Docker)

### Running Locally

1. **Restore dependencies:**
```bash
cd SocialMedia.Api
dotnet restore
```

2. **Update database:**
```bash
dotnet ef database update
```

3. **Run the application:**
```bash
dotnet run
```

### Running with Docker

From the project root:
```bash
docker-compose up --build
```

## Database Migrations

Migrations are located in `SocialMedia.Api/Migrations/`.

To create a new migration:
```bash
cd SocialMedia.Api
dotnet ef migrations add MigrationName --startup-project . --project ../SocialMedia.Infrastructure
```

To apply migrations:
```bash
dotnet ef database update --startup-project . --project ../SocialMedia.Infrastructure
```

## Architecture Benefits

- **Separation of Concerns**: Each layer has a single responsibility
- **Testability**: Easy to unit test commands, queries, and handlers
- **Maintainability**: Clear structure makes code easier to understand and modify
- **Scalability**: CQRS allows scaling read and write operations independently
- **Flexibility**: Easy to add new features without affecting existing code

## API Documentation

Once running, visit:
- Swagger UI: `http://localhost:5000/swagger`
- API Base: `http://localhost:5000/api`

## Test Credentials

- Username: `test`
- Password: `test`
