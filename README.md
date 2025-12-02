# The Social Media Site

A full-stack social media application built with React and .NET Core, following a modular-monolithic architecture with CQRS pattern.

##  Architecture

This project follows a **modular-monolithic architecture** with **CQRS (Command Query Responsibility Segregation)** pattern:

- **Frontend**: React.js with Vite
- **Backend**: .NET 8 Web API (C#)
- **Database**: PostgreSQL
- **Architecture Pattern**: CQRS with MediatR
- **Authentication**: JWT Bearer Tokens

### Project Structure

```
social/
 frontend/                 # React frontend application
    src/
       components/      # React components
       services/        # API service layer
       utils/           # Utility functions
    Dockerfile

 backend/                 # .NET backend application
    SocialMedia.Api/     # API layer (Controllers, DTOs)
    SocialMedia.Application/  # Application layer (CQRS: Commands, Queries)
    SocialMedia.Domain/  # Domain layer (Entities)
    SocialMedia.Infrastructure/  # Infrastructure layer (Data access)
    postgres/            # PostgreSQL initialization scripts
    Dockerfile

 docker-compose.yml       # Root Docker Compose file (orchestrates all services)
```

##  Quick Start

### Prerequisites

- [Docker](https://www.docker.com/get-started) and Docker Compose

### Running the Application

Run the entire application using Docker Compose:

```bash
# From the project root directory
docker-compose up --build
```

This will start:
- **PostgreSQL** database on port `5433`
- **Backend API** on port `5000`
- **Frontend** on port `5173`

Access the application:
- Frontend: http://localhost:5173
- Backend API: http://localhost:5000/api

### Docker Commands

```bash
# Build and start all services
docker-compose up --build

# Start in detached mode (background)
docker-compose up -d --build

# Stop all services
docker-compose down

# Stop and remove volumes (clean slate)
docker-compose down -v

# View logs
docker-compose logs -f

# View logs for specific service
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f postgres

# Restart a specific service
docker-compose restart backend
docker-compose restart frontend
```

##  Authentication

The application uses JWT (JSON Web Tokens) for authentication.

### Test Credentials

- **Username**: `test`
- **Password**: `test`

### Authentication Flow

1. User logs in with username/password
2. Backend validates credentials and returns a JWT token
3. Frontend stores the token in localStorage
4. All subsequent API requests include the token in the `Authorization` header
5. Backend validates the token for protected endpoints

##  API Endpoints

All endpoints are prefixed with `/api`

### Authentication

- `POST /api/auth/login` - User login (public)
  ```json
  {
    "username": "test",
    "password": "test"
  }
  ```
  Returns:
  ```json
  {
    "token": "jwt-token-here",
    "username": "test"
  }
  ```

### Posts

- `GET /api/posts` - Get all posts (authenticated)
- `POST /api/posts` - Create a new post (authenticated)
  ```json
  {
    "content": "Your post content here"
  }
  ```

### Comments

- `POST /api/posts/{postId}/comments` - Add a comment to a post (authenticated)
  ```json
  {
    "content": "Your comment here"
  }
  ```

### Likes

- `POST /api/posts/{postId}/likes` - Toggle like on a post (authenticated)
  Returns:
  ```json
  {
    "isLiked": true,
    "message": "Post liked"
  }
  ```

##  Database

The application uses **PostgreSQL** with Entity Framework Core (Code-First approach).

### Database Schema

- **Users**: User accounts with hashed passwords
- **Posts**: User posts with content and timestamps
- **Comments**: Comments on posts with author information
- **Likes**: Post likes with unique constraint (PostId, UserId)

### Migrations

Migrations are automatically applied when the backend container starts. Migrations are located in `backend/SocialMedia.Api/Migrations/`

To create a new migration (inside Docker container):
```bash
# Access the backend container
docker-compose exec backend bash

# Create migration
dotnet ef migrations add MigrationName --project SocialMedia.Api

# Exit container
exit

# Restart backend to apply migration
docker-compose restart backend
```

##  Backend Architecture

The backend follows **Clean Architecture** principles with CQRS:

### Layers

1. **SocialMedia.Api** (Presentation Layer)
   - Controllers
   - DTOs/Request models
   - API configuration

2. **SocialMedia.Application** (Application Layer)
   - Commands (CreatePost, AddComment, ToggleLike, Login)
   - Queries (GetPosts)
   - Command/Query Handlers (using MediatR)
   - DTOs

3. **SocialMedia.Domain** (Domain Layer)
   - Entities (User, Post, Comment, Like)
   - Domain models

4. **SocialMedia.Infrastructure** (Infrastructure Layer)
   - DbContext implementation
   - Data access
   - Database seeding

### CQRS Pattern

- **Commands**: Write operations (CreatePostCommand, AddCommentCommand, ToggleLikeCommand, LoginCommand)
- **Queries**: Read operations (GetPostsQuery)
- **MediatR**: Mediates between controllers and handlers

##  Frontend Architecture

The frontend is built with React and follows a component-based architecture:

### Key Components

- **Login**: User authentication
- **PostList**: Displays all posts with create post functionality
- **Post**: Individual post display with likes and comments
- **CommentSection**: Comment display and creation
- **CreatePost**: Post creation form
- **ErrorBoundary**: Error handling component

### API Service Layer

All API calls are centralized in `src/services/api.js`:
- `authAPI.login()` - Authentication
- `postsAPI.getAll()` - Get all posts
- `postsAPI.create()` - Create a post
- `commentsAPI.add()` - Add a comment
- `likesAPI.toggle()` - Toggle like on a post

##  Docker Configuration

The project uses Docker Compose for orchestration. The root `docker-compose.yml` file manages all services:

- **PostgreSQL**: Database service
- **Backend**: .NET Core API service
- **Frontend**: React application served via Nginx

All services are defined and orchestrated through the root `docker-compose.yml` file.

##  Testing

### Test User

The database is seeded with a test user:
- Username: `test`
- Password: `test`

Additional seed data includes sample posts, comments, and likes.

##  Environment Variables

All environment variables are configured in the root `docker-compose.yml` file:

**Backend:**
- `ConnectionStrings__DefaultConnection`: PostgreSQL connection string
- `JwtSettings__SecretKey`: JWT signing key
- `JwtSettings__Issuer`: JWT issuer
- `JwtSettings__Audience`: JWT audience
- `JwtSettings__ExpirationInMinutes`: Token expiration time

**Frontend:**
- `VITE_API_URL`: Backend API URL (default: `http://localhost:5000/api`)

To modify these values, edit the `docker-compose.yml` file and restart the services.

##  Development

This project is designed to run entirely in Docker. All development should be done using Docker Compose.

### Making Changes

1. Make your code changes
2. Rebuild and restart services:
   ```bash
   docker-compose up --build
   ```

### Database Migrations

Migrations are automatically applied when the backend container starts. If you need to create new migrations:

1. Access the backend container:
   ```bash
   docker-compose exec backend bash
   ```

2. Create migration:
   ```bash
   dotnet ef migrations add MigrationName --project SocialMedia.Api
   ```

3. Exit the container and restart:
   ```bash
   exit
   docker-compose restart backend
   ```

##  Technologies Used

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- MediatR (CQRS)
- JWT Authentication
- BCrypt.Net (Password hashing)

### Frontend
- React 18
- Vite
- Modern JavaScript (ES6+)

### Infrastructure
- Docker & Docker Compose
- Nginx (for frontend production)
- PostgreSQL 16

##  License

This project is for educational/demonstration purposes.

##  Contributing

This is a demonstration project. Feel free to fork and modify as needed.

##  Support

For issues or questions, please check the individual README files in:
- `backend/README.md`
- `frontend/README.md`

---

**Happy Coding! **