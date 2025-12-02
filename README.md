# The Social Media Site

A full-stack social media application built with React and .NET Core, following a modular-monolithic architecture with CQRS pattern. Please take a look the development summary at the end as well.

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

##  Development Summary

### Framework Choice

I created the application for **.NET Core framework**, choosing **.NET 8** due to its long-term support (LTS).

### Design Pattern: CQRS

The main design pattern is based on **CQRS (Command Query Responsibility Segregation)**.

**Why CQRS?**
I needed to create an application that is scalable. CQRS enables splitting functionality for horizontal scalability when needed.

**Implementation:**
- Used **MediatR** for the inbox/outbox pattern
- Currently implemented in-memory, but can be replaced with messaging systems like Kafka, ServiceBus, RabbitMQ, etc.
- This allows multiple instances to pick up handled items as needed

**Scalability Options:**
- Can be load-balanced on-premises as well as in the cloud
- Horizontal scaling is straightforward with the CQRS architecture

### Development Approach

**Time Investment:**
Approximately 2 workdays were spent on this project.

**AI Assistance:**
- Used AI assistance iteratively, not "vibe coding"
- Made sure the AI assistant worked for me, not just accepting changes without review
- All code was reviewed and validated

**Development Process:**
1. Started with a skeleton UI application (requested from AI)
2. Created .NET Core skeleton manually
3. Planned domain objects (prefer Domain-Driven Development, but not in all cases)
4. Iterative development with AI assistance for:
   - Building skeletons
   - Adding test data
   - Fine-tuning request/response classes
   - Repetitive and replaceable tasks

**Tools Used:**
- JetBrains Rider
- Cursor
- SonarLint

**AI Assistance Philosophy:**
- AI is for assistance, not for "vibe coding" in a professional environment
- Full code review is essential
- Knowing principles and reviewing is crucial
- Did not provide instruction/architecture files as guidelines to Cursor
- Used basic instructions and manual creation to guide development

### Database & Entity Framework

**PostgreSQL Choice:**
- Chose PostgreSQL instead of SQL Server to demonstrate that Npgsql is a viable option for Entity Framework
- (Note: If only technical approach matters, SQL Server would be preferred, but PostgreSQL was chosen for demonstration purposes)

**Database Setup:**
- Seeding test data happens at project startup
- Migrations happen automatically within Docker

### Future Considerations

If continuing work on this project, I would consider:

- **MediatR Distribution**: Implementing MediatR distribution in a generic way (different implementation; needs business needs and costs analysis)
- **Integration Tests**: Adding integration tests for end-to-end scenarios
- **Docker SSL Configuration**: Configuring Docker for proper SSL environment (not done due to uncertainty about running environment and certificate trust configuration)
- **Secrets Management**: Moving all secrets to secure storage (SCV or other safe storage). Currently stored in code only for local running and demonstration purposes

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