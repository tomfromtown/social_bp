# Social Media API

A .NET Core Web API backend for a social media application following **CQRS (Command Query Responsibility Segregation)** pattern with clean architecture.

## Architecture

The project follows **Clean Architecture** with **CQRS** pattern:

### Project Structure

```
backend/
├── SocialMedia.Domain/          # Domain Layer (Entities)
│   └── Entities/
│       ├── User.cs
│       ├── Post.cs
│       ├── Comment.cs
│       └── Like.cs
│
├── SocialMedia.Application/      # Application Layer (CQRS)
│   ├── Commands/                 # Write operations
│   │   ├── Login/
│   │   ├── CreatePost/
│   │   └── AddComment/
│   ├── Queries/                  # Read operations
│   │   └── GetPosts/
│   ├── DTOs/                     # Data Transfer Objects
│   └── Interfaces/              # Application interfaces
│
├── SocialMedia.Infrastructure/   # Infrastructure Layer
│   └── Data/
│       ├── ApplicationDbContext.cs
│       └── SeedData.cs
│
└── SocialMedia.Api/              # Presentation Layer (API)
    ├── Controllers/
    └── Program.cs
```

## CQRS Pattern

### Commands (Write Operations)
- **LoginCommand**: Authenticate user and return JWT token
- **CreatePostCommand**: Create a new post
- **AddCommentCommand**: Add a comment to a post

### Queries (Read Operations)
- **GetPostsQuery**: Retrieve all posts with comments and likes

### How It Works

1. **Controllers** receive HTTP requests
2. **Controllers** create Commands/Queries and send them via MediatR
3. **Handlers** process Commands/Queries and interact with Infrastructure
4. **Infrastructure** handles data access via Entity Framework Core
5. **Domain** contains pure business entities

## Features

- **JWT Authentication**: Secure token-based authentication
- **CQRS Pattern**: Separation of read and write operations
- **Clean Architecture**: Layered architecture with clear separation of concerns
- **PostgreSQL Database**: Persistent data storage
- **Code-First Migrations**: Database schema managed via EF Core migrations
- **MediatR**: Mediator pattern for CQRS implementation

## Prerequisites

- .NET 8.0 SDK or later
- PostgreSQL (or use Docker Compose)

## Getting Started

### With Docker Compose (Recommended)

```bash
cd ../..
docker-compose up --build
```

### Local Development

1. **Navigate to the API project:**
```bash
cd SocialMedia.Api
```

2. **Restore dependencies:**
```bash
dotnet restore
```

3. **Run the application:**
```bash
dotnet run
```

## API Endpoints

### Authentication

#### POST `/api/auth/login`
Login and receive a JWT token.

**Request:**
```json
{
  "username": "test",
  "password": "test"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "test"
}
```

### Posts (Requires Authentication)

#### GET `/api/posts`
Get all posts with comments and like counts.

**Headers:**
```
Authorization: Bearer <your-jwt-token>
```

#### POST `/api/posts`
Create a new post.

**Request:**
```json
{
  "content": "This is my new post!"
}
```

### Comments (Requires Authentication)

#### POST `/api/posts/{postId}/comments`
Add a comment to a post.

**Request:**
```json
{
  "content": "This is a comment!"
}
```

## Test Credentials

- Username: `test`
- Password: `test`

## Technologies Used

- .NET 8.0
- Entity Framework Core with PostgreSQL
- MediatR (CQRS implementation)
- JWT Bearer Authentication
- BCrypt for password hashing
- Swagger/OpenAPI for API documentation

## CQRS Benefits

- **Separation of Concerns**: Read and write operations are separated
- **Scalability**: Can scale read and write sides independently
- **Maintainability**: Clear structure makes code easier to maintain
- **Testability**: Commands and queries can be tested independently
- **Flexibility**: Easy to add new features without affecting existing code
