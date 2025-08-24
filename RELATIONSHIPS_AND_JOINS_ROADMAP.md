# Relationships and Joins Roadmap

This document provides a comprehensive guide for understanding, implementing, and expanding entity relationships and database joins in the BooksStore API.

## Current Relationship Architecture

### 📊 Entity Relationship Diagram

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   Author    │────▶│    Book     │◀────│   Library   │
│             │ 1:M │             │ M:1 │             │
│ - Id        │     │ - Id        │     │ - Id        │
│ - FirstName │     │ - Title     │     │ - Name      │
│ - LastName  │     │ - Year      │     │ - Address   │
└─────────────┘     │ - Isbn      │     └─────────────┘
                    │ - AuthorId  │
                    │ - LibraryId │     ┌─────────────┐
                    │ - CategoryId│◀────│BookCategory │
                    │ - LanguageId│ M:1 │             │
                    └─────────────┘     │ - Id        │
                           ▲            │ - Name      │
                           │ M:1        └─────────────┘
                           │
                    ┌─────────────┐
                    │  Language   │
                    │             │
                    │ - Id        │
                    │ - Name      │
                    └─────────────┘
```

### 🔗 Current Relationships

1. **Author → Book** (One-to-Many)

   - One author can write multiple books
   - Each book has exactly one primary author
   - Foreign Key: `Book.AuthorId`

2. **Library → Book** (One-to-Many)

   - One library can house multiple books
   - Each book belongs to exactly one library
   - Foreign Key: `Book.LibraryId`

3. **BookCategory → Book** (One-to-Many)

   - One category can contain multiple books
   - Each book belongs to exactly one category
   - Foreign Key: `Book.BookCategoryId`

4. **Language → Book** (One-to-Many)
   - One language can have multiple books
   - Each book is written in exactly one language
   - Foreign Key: `Book.LanguageId`

## 🎯 Current Join Implementations

### 1. Basic Entity Loading

```csharp
// Simple entity retrieval without related data
var book = await _context.Books.FindAsync(id);
```

### 2. Eager Loading with Include

```csharp
// Load book with all related entities
var book = await _context.Books
    .Include(b => b.Author)
    .Include(b => b.Library)
    .Include(b => b.BookCategory)
    .Include(b => b.Language)
    .FirstOrDefaultAsync(b => b.Id == id);
```

### 3. Nested Includes (LibrariesController example)

```csharp
// Load library with books and their related data
var library = await _context.Libraries
    .Include(l => l.Books)
    .ThenInclude(b => b.Author)
    .Include(l => l.Books)
    .ThenInclude(b => b.BookCategory)
    .Include(l => l.Books)
    .ThenInclude(b => b.Language)
    .FirstOrDefaultAsync(l => l.Id == id);
```

## 🚀 Advanced Relationship Patterns

### 1. Many-to-Many Relationships

#### A. Authors and Books (Enhanced)

**Current**: One-to-Many
**Enhanced**: Many-to-Many (for co-authored books)

```csharp
// Junction table approach
public class BookAuthor
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public bool IsPrimaryAuthor { get; set; } = false;
    public int AuthorOrder { get; set; } = 1;

    // Navigation properties
    public Book Book { get; set; } = null!;
    public Author Author { get; set; } = null!;
}

// Updated models
public class Book
{
    // ... existing properties
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}

public class Author
{
    // ... existing properties
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
```

**Configuration in DbContext**:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<BookAuthor>()
        .HasKey(ba => new { ba.BookId, ba.AuthorId });

    modelBuilder.Entity<BookAuthor>()
        .HasOne(ba => ba.Book)
        .WithMany(b => b.BookAuthors)
        .HasForeignKey(ba => ba.BookId);

    modelBuilder.Entity<BookAuthor>()
        .HasOne(ba => ba.Author)
        .WithMany(a => a.BookAuthors)
        .HasForeignKey(ba => ba.AuthorId);
}
```

#### B. Books and Categories (Hierarchical & Multiple)

**Current**: One-to-Many
**Enhanced**: Many-to-Many with hierarchical categories

```csharp
public class BookCategoryAssignment
{
    public int BookId { get; set; }
    public int BookCategoryId { get; set; }
    public bool IsPrimaryCategory { get; set; } = false;

    // Navigation properties
    public Book Book { get; set; } = null!;
    public BookCategory BookCategory { get; set; } = null!;
}

// Enhanced BookCategory with hierarchy
public class BookCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }

    // Navigation properties
    public BookCategory? ParentCategory { get; set; }
    public ICollection<BookCategory> SubCategories { get; set; } = new List<BookCategory>();
    public ICollection<BookCategoryAssignment> BookAssignments { get; set; } = new List<BookCategoryAssignment>();
}
```

### 2. Self-Referencing Relationships

#### Book Series Relationship

```csharp
public class Book
{
    // ... existing properties
    public int? SeriesId { get; set; }
    public int? SeriesOrder { get; set; }

    // Self-referencing navigation
    public Book? SeriesParent { get; set; }
    public ICollection<Book> SeriesBooks { get; set; } = new List<Book>();
}
```

### 3. Polymorphic Relationships

#### Address System (for Libraries and potentially Users/Publishers)

```csharp
public class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public AddressType Type { get; set; } = AddressType.Primary;

    // Polymorphic foreign keys
    public int EntityId { get; set; }
    public string EntityType { get; set; } = string.Empty; // "Library", "User", etc.
}

public enum AddressType
{
    Primary,
    Secondary,
    Billing,
    Shipping
}
```

## 📋 Query Patterns and Best Practices

### 1. Projection Queries (Performance Optimization)

```csharp
// Instead of loading full entities, project only needed data
public class BookSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string LibraryName { get; set; } = string.Empty;
}

public async Task<IEnumerable<BookSummaryDto>> GetBookSummariesAsync()
{
    return await _context.Books
        .Select(b => new BookSummaryDto
        {
            Id = b.Id,
            Title = b.Title,
            AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
            LibraryName = b.Library.Name
        })
        .ToListAsync();
}
```

### 2. Conditional Includes

```csharp
public async Task<Book?> GetBookAsync(int id, bool includeAuthor = false, bool includeLibrary = false)
{
    var query = _context.Books.AsQueryable();

    if (includeAuthor)
        query = query.Include(b => b.Author);

    if (includeLibrary)
        query = query.Include(b => b.Library);

    return await query.FirstOrDefaultAsync(b => b.Id == id);
}
```

### 3. Split Queries (for multiple collections)

```csharp
// Use AsSplitQuery() for better performance with multiple includes
var library = await _context.Libraries
    .AsSplitQuery()
    .Include(l => l.Books)
    .ThenInclude(b => b.Author)
    .Include(l => l.Books)
    .ThenInclude(b => b.BookCategory)
    .FirstOrDefaultAsync(l => l.Id == id);
```

### 4. Filtered Includes

```csharp
// Include only available books in a library
var library = await _context.Libraries
    .Include(l => l.Books.Where(b => b.IsAvailable))
    .ThenInclude(b => b.Author)
    .FirstOrDefaultAsync(l => l.Id == id);
```

## 🛠️ Advanced Join Scenarios

### 1. Complex Search Queries

```csharp
public async Task<IEnumerable<Book>> SearchBooksAsync(BookSearchDto searchDto)
{
    var query = _context.Books
        .Include(b => b.Author)
        .Include(b => b.Library)
        .Include(b => b.BookCategory)
        .AsQueryable();

    if (!string.IsNullOrEmpty(searchDto.Title))
    {
        query = query.Where(b => b.Title.Contains(searchDto.Title));
    }

    if (!string.IsNullOrEmpty(searchDto.AuthorName))
    {
        query = query.Where(b =>
            (b.Author.FirstName + " " + b.Author.LastName).Contains(searchDto.AuthorName));
    }

    if (searchDto.CategoryIds?.Any() == true)
    {
        query = query.Where(b => searchDto.CategoryIds.Contains(b.BookCategoryId));
    }

    if (searchDto.LibraryIds?.Any() == true)
    {
        query = query.Where(b => searchDto.LibraryIds.Contains(b.LibraryId));
    }

    if (searchDto.YearFrom.HasValue)
    {
        query = query.Where(b => b.Year >= searchDto.YearFrom.Value);
    }

    if (searchDto.YearTo.HasValue)
    {
        query = query.Where(b => b.Year <= searchDto.YearTo.Value);
    }

    return await query.ToListAsync();
}
```

### 2. Aggregation Queries

```csharp
// Books count by category
public async Task<IEnumerable<CategoryStatsDto>> GetCategoryStatsAsync()
{
    return await _context.BookCategories
        .Select(c => new CategoryStatsDto
        {
            CategoryName = c.Name,
            BookCount = c.Books.Count(),
            AvailableCount = c.Books.Count(b => b.IsAvailable),
            LibraryCount = c.Books.Select(b => b.LibraryId).Distinct().Count()
        })
        .ToListAsync();
}

// Authors with most books
public async Task<IEnumerable<AuthorStatsDto>> GetTopAuthorsAsync(int topCount = 10)
{
    return await _context.Authors
        .Select(a => new AuthorStatsDto
        {
            AuthorName = $"{a.FirstName} {a.LastName}",
            BookCount = a.Books.Count(),
            LibraryCount = a.Books.Select(b => b.LibraryId).Distinct().Count(),
            LatestBookYear = a.Books.Max(b => b.Year)
        })
        .OrderByDescending(a => a.BookCount)
        .Take(topCount)
        .ToListAsync();
}
```

### 3. Cross-Entity Reporting

```csharp
public async Task<LibraryReportDto> GetLibraryReportAsync(int libraryId)
{
    var library = await _context.Libraries
        .Include(l => l.Books)
        .ThenInclude(b => b.Author)
        .Include(l => l.Books)
        .ThenInclude(b => b.BookCategory)
        .FirstOrDefaultAsync(l => l.Id == libraryId);

    if (library == null) return null;

    return new LibraryReportDto
    {
        LibraryName = library.Name,
        TotalBooks = library.Books.Count,
        AvailableBooks = library.Books.Count(b => b.IsAvailable),
        AuthorCount = library.Books.Select(b => b.AuthorId).Distinct().Count(),
        CategoryDistribution = library.Books
            .GroupBy(b => b.BookCategory.Name)
            .Select(g => new CategoryDistributionDto
            {
                Category = g.Key,
                Count = g.Count(),
                Percentage = (double)g.Count() / library.Books.Count * 100
            })
            .ToList()
    };
}
```

## 🔄 Migration Roadmap for Enhanced Relationships

### Phase 1: Enhanced Current Relationships

1. **Add Navigation Properties to Author**

   ```csharp
   public ICollection<Book> Books { get; set; } = new List<Book>();
   ```

2. **Implement Soft Deletes**
   ```csharp
   public bool IsDeleted { get; set; } = false;
   public DateTime? DeletedAt { get; set; }
   public string DeletedBy { get; set; } = string.Empty;
   ```

### Phase 2: Many-to-Many Relationships

1. **BookAuthor Junction Table**
2. **BookCategoryAssignment Junction Table**
3. **Update Controllers to handle complex relationships**

### Phase 3: Advanced Features

1. **Book Series Relationships**
2. **Hierarchical Categories**
3. **Polymorphic Address System**

### Phase 4: Performance Optimization

1. **Database Indexes on Foreign Keys**
2. **Composite Indexes for Common Queries**
3. **Query Optimization and Monitoring**

## 📊 Database Design Best Practices

### 1. Indexing Strategy

```sql
-- Foreign key indexes (automatically created in most databases)
CREATE INDEX IX_Books_AuthorId ON Books(AuthorId);
CREATE INDEX IX_Books_LibraryId ON Books(LibraryId);
CREATE INDEX IX_Books_BookCategoryId ON Books(BookCategoryId);
CREATE INDEX IX_Books_LanguageId ON Books(LanguageId);

-- Composite indexes for common queries
CREATE INDEX IX_Books_Library_Category ON Books(LibraryId, BookCategoryId);
CREATE INDEX IX_Books_Author_Year ON Books(AuthorId, Year);

-- Text search indexes
CREATE INDEX IX_Books_Title ON Books(Title);
CREATE INDEX IX_Authors_Name ON Authors(FirstName, LastName);
```

### 2. Referential Integrity

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Cascade delete configuration
    modelBuilder.Entity<Book>()
        .HasOne(b => b.Author)
        .WithMany(a => a.Books)
        .OnDelete(DeleteBehavior.Restrict); // Prevent author deletion if books exist

    modelBuilder.Entity<Book>()
        .HasOne(b => b.Library)
        .WithMany(l => l.Books)
        .OnDelete(DeleteBehavior.Cascade); // Delete books when library is deleted

    // Check constraints
    modelBuilder.Entity<Book>()
        .HasCheckConstraint("CK_Book_Year", "Year > 0 AND Year <= YEAR(GETDATE())");
}
```

### 3. Data Seeding

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Seed basic data
    modelBuilder.Entity<Language>().HasData(
        new Language { Id = 1, Name = "English", Code = "en" },
        new Language { Id = 2, Name = "Spanish", Code = "es" },
        new Language { Id = 3, Name = "French", Code = "fr" }
    );

    modelBuilder.Entity<BookCategory>().HasData(
        new BookCategory { Id = 1, Name = "Fiction" },
        new BookCategory { Id = 2, Name = "Non-Fiction" },
        new BookCategory { Id = 3, Name = "Science" },
        new BookCategory { Id = 4, Name = "Technology" }
    );
}
```

## 🎯 Testing Strategy for Relationships

### 1. Unit Tests for Navigation Properties

```csharp
[Test]
public async Task GetLibrary_WithBooks_ReturnsLibraryWithBooks()
{
    // Arrange
    var library = new Library { Name = "Test Library", Address = "Test Address" };
    var book = new Book { Title = "Test Book", Library = library };

    // Act & Assert
    Assert.That(library.Books, Contains.Item(book));
    Assert.That(book.Library, Is.EqualTo(library));
}
```

### 2. Integration Tests for Joins

```csharp
[Test]
public async Task GetLibraryBooks_WithIncludes_ReturnsCompleteData()
{
    // Arrange
    using var context = CreateTestContext();
    var controller = new LibrariesController(context);

    // Act
    var result = await controller.GetLibraryBooks(1);

    // Assert
    var books = GetOkResult<IEnumerable<Book>>(result);
    Assert.That(books.First().Author, Is.Not.Null);
    Assert.That(books.First().BookCategory, Is.Not.Null);
}
```

## 📈 Performance Monitoring

### 1. Query Analysis

```csharp
// Log slow queries in Program.cs
builder.Services.AddDbContext<BooksStoreContext>(options =>
{
    options.UseSqlServer(connectionString)
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging() // Only in development
           .EnableDetailedErrors();
});
```

### 2. N+1 Query Detection

```csharp
// Use explicit loading to avoid N+1 problems
var books = await _context.Books.ToListAsync();
await _context.Entry(books.First())
    .Reference(b => b.Author)
    .LoadAsync();
```

## 🚧 Common Pitfalls and Solutions

### 1. Circular References in JSON Serialization

**Problem**: JSON serialization fails with navigation properties
**Solution**: Use DTOs or configure JSON options

```csharp
// In Program.cs
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
```

### 2. Performance Issues with Large Datasets

**Problem**: Loading too much data with includes
**Solution**: Use projection and pagination

```csharp
public async Task<PagedResult<BookSummaryDto>> GetBooksPagedAsync(int page, int pageSize)
{
    var query = _context.Books
        .Select(b => new BookSummaryDto
        {
            Id = b.Id,
            Title = b.Title,
            AuthorName = $"{b.Author.FirstName} {b.Author.LastName}"
        });

    var totalCount = await query.CountAsync();
    var items = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new PagedResult<BookSummaryDto>
    {
        Items = items,
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize
    };
}
```

### 3. Concurrency Issues

**Problem**: Multiple users updating related entities
**Solution**: Use optimistic concurrency with timestamps

```csharp
public class Book
{
    // ... other properties
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
}
```

This comprehensive roadmap provides the foundation for building robust, scalable relationships in your BooksStore API. Start with the current implementations and gradually enhance them based on your specific requirements and use cases.
