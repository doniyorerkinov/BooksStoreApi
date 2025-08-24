# Models Development Roadmap

This document outlines the roadmap for developing and expanding the data models in the BooksStore API project.

## Current Models Status

### ✅ Existing Models

1. **Author** (`Author.cs`)

   ```csharp
   - Id (int, Primary Key)
   - FirstName (string)
   - LastName (string)
   ```

   **Status**: ✅ Complete - Basic implementation
   **Navigation Properties**: None (one-to-many with Book)

2. **Library** (`Library.cs`)

   ```csharp
   - Id (int, Primary Key)
   - Name (string)
   - Address (string)
   - Books (ICollection<Book>) // Navigation property
   ```

   **Status**: ✅ Complete - With navigation properties

3. **Book** (`Book.cs`)

   ```csharp
   - Id (int, Primary Key)
   - Title (string)
   - Year (int)
   - Isbn (string)
   - AuthorId (int, Foreign Key)
   - LibraryId (int, Foreign Key)
   - BookCategoryId (int, Foreign Key)
   - LanguageId (int, Foreign Key)
   - Author, Library, BookCategory, Language (Navigation properties)
   ```

   **Status**: ✅ Complete - Central entity with all relationships

4. **BookCategory** (`BookCategory.cs`)

   ```csharp
   - Id (int, Primary Key)
   - Name (string)
   - Books (ICollection<Book>) // Navigation property
   ```

   **Status**: ✅ Complete - With navigation properties

5. **Language** (`Language.cs`)
   ```csharp
   - Id (int, Primary Key)
   - Name (string)
   - Books (ICollection<Book>) // Navigation property
   ```
   **Status**: ✅ Complete - With navigation properties

## 🔄 Model Enhancement Opportunities

### 1. Author Model Enhancements (High Priority)

**File**: `Author.cs`

**Missing Properties**:

```csharp
public DateTime? BirthDate { get; set; }
public DateTime? DeathDate { get; set; }
public string Biography { get; set; } = string.Empty;
public string Nationality { get; set; } = string.Empty;
public string Email { get; set; } = string.Empty;
public string Website { get; set; } = string.Empty;

// Navigation Properties
public ICollection<Book> Books { get; set; } = new List<Book>();
```

**Benefits**:

- Rich author profiles
- Better search capabilities
- Enhanced user experience

### 2. Book Model Enhancements (High Priority)

**File**: `Book.cs`

**Missing Properties**:

```csharp
public string Description { get; set; } = string.Empty;
public int PageCount { get; set; }
public decimal Price { get; set; }
public DateTime PublishedDate { get; set; }
public string Publisher { get; set; } = string.Empty;
public string CoverImageUrl { get; set; } = string.Empty;
public bool IsAvailable { get; set; } = true;
public int CopiesTotal { get; set; } = 1;
public int CopiesAvailable { get; set; } = 1;
public DateTime CreatedAt { get; set; }
public DateTime UpdatedAt { get; set; }
```

**Benefits**:

- Inventory management
- Better book information
- E-commerce capabilities

### 3. Library Model Enhancements (Medium Priority)

**File**: `Library.cs`

**Missing Properties**:

```csharp
public string Phone { get; set; } = string.Empty;
public string Email { get; set; } = string.Empty;
public string Website { get; set; } = string.Empty;
public TimeSpan OpeningTime { get; set; }
public TimeSpan ClosingTime { get; set; }
public string City { get; set; } = string.Empty;
public string State { get; set; } = string.Empty;
public string ZipCode { get; set; } = string.Empty;
public string Country { get; set; } = string.Empty;
public int Capacity { get; set; }
public bool IsActive { get; set; } = true;
```

**Benefits**:

- Better library management
- Location-based features
- Operating hours tracking

### 4. BookCategory Model Enhancements (Low Priority)

**File**: `BookCategory.cs`

**Missing Properties**:

```csharp
public string Description { get; set; } = string.Empty;
public string ColorCode { get; set; } = string.Empty; // For UI theming
public int SortOrder { get; set; } = 0;
public bool IsActive { get; set; } = true;
public int? ParentCategoryId { get; set; } // For hierarchical categories

// Navigation Properties
public BookCategory? ParentCategory { get; set; }
public ICollection<BookCategory> SubCategories { get; set; } = new List<BookCategory>();
```

**Benefits**:

- Hierarchical category structure
- Better organization
- UI customization

### 5. Language Model Enhancements (Low Priority)

**File**: `Language.cs`

**Missing Properties**:

```csharp
public string Code { get; set; } = string.Empty; // ISO 639-1 code (e.g., "en", "es")
public string NativeName { get; set; } = string.Empty; // Language name in its own script
public bool IsRightToLeft { get; set; } = false;
public bool IsActive { get; set; } = true;
```

**Benefits**:

- Internationalization support
- Better language handling
- Standardized language codes

## 🆕 New Models to Consider

### 1. Publisher Model (Medium Priority)

**File**: `Publisher.cs`

```csharp
public class Publisher
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime EstablishedYear { get; set; }

    // Navigation Properties
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
```

**Impact**: Update Book model to include PublisherId

### 2. User Model (High Priority for future)

**File**: `User.cs`

```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public UserRole Role { get; set; } = UserRole.Member;

    // Navigation Properties
    public ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

public enum UserRole
{
    Member,
    Librarian,
    Admin
}
```

### 3. BookLoan Model (High Priority for future)

**File**: `BookLoan.cs`

```csharp
public class BookLoan
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public int LibraryId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned { get; set; } = false;
    public decimal? FineAmount { get; set; }

    // Navigation Properties
    public Book Book { get; set; } = null!;
    public User User { get; set; } = null!;
    public Library Library { get; set; } = null!;
}
```

### 4. Reservation Model (Medium Priority for future)

**File**: `Reservation.cs`

```csharp
public class Reservation
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public int LibraryId { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFulfilled { get; set; } = false;

    // Navigation Properties
    public Book Book { get; set; } = null!;
    public User User { get; set; } = null!;
    public Library Library { get; set; } = null!;
}
```

### 5. Review Model (Low Priority)

**File**: `Review.cs`

```csharp
public class Review
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; } // 1-5 stars
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Properties
    public Book Book { get; set; } = null!;
    public User User { get; set; } = null!;
}
```

## 🏗️ Implementation Guidelines

### 1. Data Annotations

Use appropriate data annotations for validation:

```csharp
[Required]
[StringLength(100)]
public string Name { get; set; } = string.Empty;

[EmailAddress]
public string Email { get; set; } = string.Empty;

[Phone]
public string Phone { get; set; } = string.Empty;

[Range(1, 5)]
public int Rating { get; set; }

[DataType(DataType.Date)]
public DateTime PublishedDate { get; set; }
```

### 2. Fluent API Configuration

Configure complex relationships in `BooksStoreContext.OnModelCreating()`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Configure decimal precision
    modelBuilder.Entity<Book>()
        .Property(b => b.Price)
        .HasColumnType("decimal(18,2)");

    // Configure cascade delete
    modelBuilder.Entity<BookLoan>()
        .HasOne(bl => bl.Book)
        .WithMany()
        .OnDelete(DeleteBehavior.Restrict);

    // Configure unique constraints
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();
}
```

### 3. Audit Properties

Consider implementing a base entity for common audit fields:

```csharp
public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
}

public class Book : AuditableEntity
{
    // ... other properties
}
```

## 📅 Implementation Timeline

### Phase 1: Core Enhancements (Week 1)

- [ ] Enhance Author model with navigation properties
- [ ] Add basic Book enhancements (price, description)
- [ ] Improve Library model with contact info

### Phase 2: Advanced Features (Week 2)

- [ ] Add Publisher model
- [ ] Implement hierarchical BookCategory
- [ ] Enhance Language with ISO codes

### Phase 3: User Management (Week 3)

- [ ] Implement User model
- [ ] Add authentication/authorization
- [ ] Create user-related controllers

### Phase 4: Library Operations (Week 4)

- [ ] Add BookLoan model
- [ ] Implement Reservation system
- [ ] Add Review system

### Phase 5: Advanced Features (Week 5)

- [ ] Implement audit trail
- [ ] Add soft delete functionality
- [ ] Performance optimizations

## 🔧 Migration Strategy

### 1. Incremental Changes

- Add new properties to existing models gradually
- Use nullable properties for new required fields
- Provide default values where appropriate

### 2. Data Migration

```csharp
// Example migration for adding new properties
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<string>(
        name: "Biography",
        table: "Authors",
        type: "nvarchar(max)",
        nullable: false,
        defaultValue: "");
}
```

### 3. Backward Compatibility

- Don't remove existing properties without deprecation
- Use versioning for breaking changes
- Provide migration guides

## 📊 Success Metrics

- All models properly normalized (3NF)
- Comprehensive validation attributes
- Proper navigation properties
- Clean migration history
- No circular references
- Efficient query performance
- Comprehensive test coverage

## ⚠️ Important Considerations

### 1. Performance

- Use lazy loading carefully
- Implement projection for read-only scenarios
- Consider database indexes for frequently queried fields

### 2. Validation

- Implement both client-side and server-side validation
- Use custom validation attributes for complex rules
- Provide meaningful error messages

### 3. Security

- Never store passwords in plain text
- Implement proper authorization checks
- Validate all user inputs

### 4. Scalability

- Consider partitioning for large tables
- Implement caching strategies
- Use appropriate data types for performance

## Notes

- Always create migrations after model changes
- Test relationships thoroughly
- Consider using DTOs for API responses
- Document any breaking changes
- Keep business logic separate from data models
