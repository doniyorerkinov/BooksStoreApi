# Development Roadmap - BookStore API

This document provides a comprehensive roadmap for extending the BookStore API with new controllers, models, and features.

## 📋 Table of Contents

- [Phase 1: Core Controllers](#phase-1-core-controllers)
- [Phase 2: Advanced Features](#phase-2-advanced-features)
- [Phase 3: Business Logic](#phase-3-business-logic)
- [Phase 4: Performance & Security](#phase-4-performance--security)
- [Adding New Models](#adding-new-models)
- [Adding New Controllers](#adding-new-controllers)
- [Best Practices](#best-practices)

## 🎯 Phase 1: Core Controllers (Priority: High)

### 1.1 Books Controller

**Timeline: 1-2 days**

Create `BooksController.cs` with full CRUD operations:

```csharp
// Controllers/BooksController.cs
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    // GET: api/Books
    // GET: api/Books/{id}
    // GET: api/Books/search?title={title}&author={author}
    // POST: api/Books
    // PUT: api/Books/{id}
    // DELETE: api/Books/{id}
}
```

**Key Features:**

- Include related data (Author, Library, Category, Language)
- Search functionality by title, author, ISBN
- Filtering by library, category, language
- Validation for ISBN format

### 1.2 BookCategories Controller

**Timeline: 1 day**

```csharp
// Controllers/BookCategoriesController.cs
[Route("api/[controller]")]
[ApiController]
public class BookCategoriesController : ControllerBase
{
    // Standard CRUD operations
    // GET: api/BookCategories/{id}/books - Get books by category
}
```

### 1.3 Languages Controller

**Timeline: 1 day**

```csharp
// Controllers/LanguagesController.cs
[Route("api/[controller]")]
[ApiController]
public class LanguagesController : ControllerBase
{
    // Standard CRUD operations
    // GET: api/Languages/{id}/books - Get books by language
}
```

### Implementation Steps:

1. **Create Controller Files**: Follow the pattern established in `AuthorsController.cs`
2. **Add Proper Documentation**: Use XML comments for Swagger generation
3. **Include Relationship Endpoints**: Add endpoints to get related entities
4. **Add Validation**: Implement model validation attributes
5. **Test Endpoints**: Use Swagger UI or Postman for testing

## 🔧 Phase 2: Advanced Features (Priority: Medium)

### 2.1 Search & Filtering Service

**Timeline: 2-3 days**

```csharp
// Services/ISearchService.cs
public interface ISearchService
{
    Task<PagedResult<Book>> SearchBooksAsync(BookSearchCriteria criteria);
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);
    Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
}

// Models/BookSearchCriteria.cs
public class BookSearchCriteria
{
    public string? Title { get; set; }
    public string? AuthorName { get; set; }
    public string? Isbn { get; set; }
    public int? LibraryId { get; set; }
    public int? CategoryId { get; set; }
    public int? LanguageId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
```

### 2.2 Pagination Support

**Timeline: 1 day**

```csharp
// Models/PagedResult.cs
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
```

### 2.3 Data Transfer Objects (DTOs)

**Timeline: 2 days**

Create DTOs to control API responses and improve performance:

```csharp
// DTOs/BookDto.cs
public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Isbn { get; set; }
    public AuthorDto Author { get; set; }
    public LibraryDto Library { get; set; }
    public string Category { get; set; }
    public string Language { get; set; }
}

// DTOs/CreateBookDto.cs
public class CreateBookDto
{
    [Required]
    public string Title { get; set; }

    [Range(1000, 9999)]
    public int Year { get; set; }

    [Required]
    [RegularExpression(@"^[\d-]{10,17}$")]
    public string Isbn { get; set; }

    public int AuthorId { get; set; }
    public int LibraryId { get; set; }
    public int BookCategoryId { get; set; }
    public int LanguageId { get; set; }
}
```

## 🏢 Phase 3: Business Logic (Priority: Medium-Low)

### 3.1 Library Management Service

**Timeline: 3-4 days**

```csharp
// Services/ILibraryManagementService.cs
public interface ILibraryManagementService
{
    Task<bool> TransferBookAsync(int bookId, int fromLibraryId, int toLibraryId);
    Task<LibraryStatistics> GetLibraryStatisticsAsync(int libraryId);
    Task<IEnumerable<Book>> GetAvailableBooksAsync(int libraryId);
}

// Models/LibraryStatistics.cs
public class LibraryStatistics
{
    public int TotalBooks { get; set; }
    public int UniqueAuthors { get; set; }
    public Dictionary<string, int> BooksByCategory { get; set; }
    public Dictionary<string, int> BooksByLanguage { get; set; }
}
```

### 3.2 Author Management Features

**Timeline: 2 days**

```csharp
// Extend AuthorsController with:
// GET: api/Authors/{id}/books
// GET: api/Authors/{id}/statistics
// GET: api/Authors/search?name={name}
```

### 3.3 Book Recommendation System

**Timeline: 4-5 days**

```csharp
// Services/IRecommendationService.cs
public interface IRecommendationService
{
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);
    Task<IEnumerable<Book>> GetBooksInSameCategoryAsync(int bookId);
    Task<IEnumerable<Book>> GetPopularBooksInLibraryAsync(int libraryId);
}
```

## 🚀 Phase 4: Performance & Security (Priority: Low)

### 4.1 Caching Implementation

**Timeline: 2-3 days**

```csharp
// Add to Program.cs
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Services/ICacheService.cs
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan expiration);
    Task RemoveAsync(string key);
}
```

### 4.2 Authentication & Authorization

**Timeline: 4-5 days**

```csharp
// Models/User.cs
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
}

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* configuration */ });
```

### 4.3 Logging & Monitoring

**Timeline: 1-2 days**

```csharp
// Add Serilog
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// Add Application Insights or similar monitoring
builder.Services.AddApplicationInsightsTelemetry();
```

## 📦 Adding New Models

### Step-by-Step Process:

1. **Create Model Class**

```csharp
// Models/Publisher.cs
public class Publisher
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
```

2. **Update Book Model** (if creating relationships)

```csharp
// Add to Book.cs
public int? PublisherId { get; set; }
public Publisher? Publisher { get; set; }
```

3. **Update DbContext**

```csharp
// Add to BooksStoreContext.cs
public DbSet<Publisher> Publishers { get; set; } = null!;

// Configure relationships in OnModelCreating if needed
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Book>()
        .HasOne(b => b.Publisher)
        .WithMany(p => p.Books)
        .HasForeignKey(b => b.PublisherId)
        .OnDelete(DeleteBehavior.SetNull);
}
```

4. **Create Migration**

```bash
dotnet ef migrations add AddPublisher
dotnet ef database update
```

## 🎮 Adding New Controllers

### Template for New Controller:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksStoreApi.Data;
using BooksStoreApi.Models;

namespace BooksStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class [EntityName]Controller : ControllerBase
    {
        private readonly BooksStoreContext _context;

        public [EntityName]Controller(BooksStoreContext context)
        {
            _context = context;
        }

        // GET: api/[EntityName]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<[EntityName]>>> Get[EntityName]s()
        {
            if (_context.[EntityName]s == null)
            {
                return NotFound();
            }
            return await _context.[EntityName]s.ToListAsync();
        }

        // Additional CRUD operations following the same pattern...
    }
}
```

## 🎯 Best Practices

### 1. Model Design

- Use navigation properties for relationships
- Implement proper validation attributes
- Consider using DTOs for API responses
- Use nullable reference types appropriately

### 2. Controller Design

- Follow RESTful conventions
- Use appropriate HTTP status codes
- Implement proper error handling
- Add comprehensive XML documentation

### 3. Database Design

- Use foreign keys for relationships
- Consider indexing for performance
- Implement soft deletes for important data
- Use migrations for schema changes

### 4. Performance

- Use `Include()` for eager loading relationships
- Implement pagination for large datasets
- Consider caching for frequently accessed data
- Use projection for read-only scenarios

### 5. Testing

- Write unit tests for business logic
- Create integration tests for controllers
- Use test databases for testing
- Mock external dependencies

## 📊 Estimated Timeline Summary

| Phase                           | Duration  | Priority   |
| ------------------------------- | --------- | ---------- |
| Phase 1: Core Controllers       | 4-5 days  | High       |
| Phase 2: Advanced Features      | 5-6 days  | Medium     |
| Phase 3: Business Logic         | 9-11 days | Medium-Low |
| Phase 4: Performance & Security | 7-10 days | Low        |

**Total Estimated Time: 25-32 days**

This roadmap provides a structured approach to extending your BookStore API. Start with Phase 1 to establish the core functionality, then move through subsequent phases based on your project requirements and priorities.
