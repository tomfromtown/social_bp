# Social Media Application Tests

Unit tests for the Social Media Application layer using xUnit, Moq, and Entity Framework InMemory database.

## Test Structure

Tests are organized to match the application structure:

- **Commands**: Tests for command handlers (Login, CreatePost, AddComment, ToggleLike)
- **Queries**: Tests for query handlers (GetPosts)
- **Helpers**: Test utilities and helpers (TestDbContext, TestDbContextFactory)

## Running Tests

### Using .NET CLI

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run tests for a specific class
dotnet test --filter "FullyQualifiedName~LoginCommandHandlerTests"

# Run tests with code coverage
dotnet test /p:CollectCoverage=true
```

### Using Visual Studio

1. Open Test Explorer (Test → Test Explorer)
2. Build the solution
3. Run all tests or select specific tests

### Using Rider

1. Right-click on the test project
2. Select "Run Tests" or use the Unit Tests tool window

## Test Coverage

### LoginCommandHandler
- ✅ Valid credentials returns LoginResponse
- ✅ Invalid username returns null
- ✅ Invalid password returns null
- ✅ Valid credentials generates valid JWT token

### CreatePostCommandHandler
- ✅ Valid request creates post
- ✅ Invalid user ID returns null
- ✅ Sets CreatedAt timestamp correctly

### AddCommentCommandHandler
- ✅ Valid request creates comment
- ✅ Invalid post ID returns null
- ✅ Invalid user ID returns null

### ToggleLikeCommandHandler
- ✅ No existing like adds like and returns true
- ✅ Existing like removes like and returns false
- ✅ Invalid post ID throws InvalidOperationException
- ✅ Invalid user ID throws InvalidOperationException

### GetPostsQueryHandler
- ✅ No posts returns empty list
- ✅ With posts returns all posts
- ✅ With posts and comments returns posts with comments
- ✅ With likes returns posts with like count
- ✅ With current user ID sets IsLiked correctly
- ✅ Without current user ID sets IsLiked to false

## Test Dependencies

- **xUnit**: Testing framework
- **Moq**: Mocking framework (for future use with external dependencies)
- **FluentAssertions**: Fluent assertion library for readable test assertions
- **EntityFrameworkCore.InMemory**: In-memory database for testing

## Test Database

Tests use an in-memory database (`TestDbContext`) that is created fresh for each test, ensuring test isolation and no side effects between tests.

