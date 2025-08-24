# Controllers Development Roadmap

This document outlines the roadmap for developing and expanding the API controllers in the BooksStore API project.

## Current Controllers Status

### ✅ Completed Controllers

1. **AuthorsController** (`AuthorsController.cs`)

   - Full CRUD operations (GET, POST, PUT, DELETE)
   - Single author retrieval by ID
   - All authors listing
   - Proper error handling and validation

2. **LibrariesController** (`LibrariesController.cs`)
   - Full CRUD operations (GET, POST, PUT, DELETE)
   - Single library retrieval by ID
   - All libraries listing
   - Library books retrieval with related data
   - Proper error handling and validation

## 📋 Pending Controllers

### 1. BooksController (High Priority)

**File**: `BooksController.cs`

**Endpoints to implement**:

- `GET /api/Books` - Get all books with pagination
- `GET /api/Books/{id}` - Get book by ID with related data
- `GET /api/Books/search?title={title}&author={author}&category={category}` - Search books
- `POST /api/Books` - Create new book
- `PUT /api/Books/{id}` - Update book
- `DELETE /api/Books/{id}` - Delete book

**Special considerations**:

- Include related data (Author, Library, Category, Language) in responses
- Implement search functionality
- Add pagination for large datasets
- Validate foreign key relationships

**Dependencies**: All other models (Author, Library, BookCategory, Language)

### 2. BookCategoriesController (Medium Priority)

**File**: `BookCategoriesController.cs`

**Endpoints to implement**:

- `GET /api/BookCategories` - Get all categories
- `GET /api/BookCategories/{id}` - Get category by ID
- `GET /api/BookCategories/{id}/books` - Get books in category
- `POST /api/BookCategories` - Create new category
- `PUT /api/BookCategories/{id}` - Update category
- `DELETE /api/BookCategories/{id}` - Delete category

**Special considerations**:

- Handle cascade deletion (what happens to books when category is deleted?)
- Prevent deletion if books exist in category

### 3. LanguagesController (Medium Priority)

**File**: `LanguagesController.cs`

**Endpoints to implement**:

- `GET /api/Languages` - Get all languages
- `GET /api/Languages/{id}` - Get language by ID
- `GET /api/Languages/{id}/books` - Get books in language
- `POST /api/Languages` - Create new language
- `PUT /api/Languages/{id}` - Update language
- `DELETE /api/Languages/{id}` - Delete language

**Special considerations**:

- Similar to BookCategories - handle cascade deletion
- Consider adding language codes (ISO 639-1)

## 🔄 Enhancement Opportunities

### Advanced Search Controller

**File**: `SearchController.cs`

**Purpose**: Centralized search functionality across all entities

**Endpoints**:

- `GET /api/Search/books?q={query}&filters={filters}` - Advanced book search
- `GET /api/Search/authors?q={query}` - Author search
- `GET /api/Search/libraries?location={location}` - Library search by location

### Statistics Controller

**File**: `StatisticsController.cs`

**Purpose**: Provide analytical data

**Endpoints**:

- `GET /api/Statistics/books/count` - Total books count
- `GET /api/Statistics/books/by-category` - Books distribution by category
- `GET /api/Statistics/books/by-year` - Books by publication year
- `GET /api/Statistics/libraries/capacity` - Library capacity analysis

### Recommendations Controller

**File**: `RecommendationsController.cs`

**Purpose**: Book recommendation system

**Endpoints**:

- `GET /api/Recommendations/books/by-author/{authorId}` - Books by same author
- `GET /api/Recommendations/books/by-category/{categoryId}` - Similar books
- `GET /api/Recommendations/books/popular` - Popular books

## 🏗️ Implementation Guidelines

### 1. Controller Structure Template

```csharp
[Route("api/[controller]")]
[ApiController]
public class {EntityName}Controller : ControllerBase
{
    private readonly BooksStoreContext _context;

    public {EntityName}Controller(BooksStoreContext context)
    {
        _context = context;
    }

    // Standard CRUD operations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<{Entity}>>> Get{EntityName}s()

    [HttpGet("{id}")]
    public async Task<ActionResult<{Entity}>> Get{Entity}(int id)

    [HttpPost]
    public async Task<ActionResult<{Entity}>> Post{Entity}({Entity} entity)

    [HttpPut("{id}")]
    public async Task<IActionResult> Put{Entity}(int id, {Entity} entity)

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete{Entity}(int id)

    private bool {Entity}Exists(int id)
}
```

### 2. Best Practices

#### Error Handling

- Always check for null DbSets
- Handle `DbUpdateConcurrencyException`
- Return appropriate HTTP status codes
- Use consistent error messages

#### Validation

- Validate model state with `[ApiController]` attribute
- Check foreign key constraints
- Validate business rules

#### Performance

- Use `async/await` for all database operations
- Include related data with `Include()` when needed
- Implement pagination for large datasets
- Use projections for read-only operations

#### Documentation

- Add XML comments for API documentation
- Use descriptive action names
- Include example requests/responses

### 3. Testing Strategy

#### Unit Tests

- Test each controller action
- Mock the database context
- Test error scenarios
- Validate return types and status codes

#### Integration Tests

- Test full request/response cycle
- Use in-memory database
- Test authorization and authentication
- Validate API contracts

## 📅 Implementation Timeline

### Phase 1 (Week 1)

- [ ] BooksController (core functionality)
- [ ] Unit tests for BooksController

### Phase 2 (Week 2)

- [ ] BookCategoriesController
- [ ] LanguagesController
- [ ] Integration tests

### Phase 3 (Week 3)

- [ ] Advanced search functionality
- [ ] Statistics controller
- [ ] Performance optimization

### Phase 4 (Week 4)

- [ ] Recommendations controller
- [ ] API documentation enhancement
- [ ] Security improvements

## 🔧 Development Tools

### Code Generation

Consider using tools to generate boilerplate controller code:

- Visual Studio scaffolding
- Custom T4 templates
- Code snippets

### API Testing

- Swagger UI for manual testing
- Postman collections
- Automated API tests

### Performance Monitoring

- Application Insights
- Custom metrics
- Database query profiling

## 📊 Success Metrics

- All CRUD operations working correctly
- Proper error handling and validation
- Comprehensive test coverage (>80%)
- API response times < 200ms
- Consistent API design patterns
- Complete API documentation

## Notes

- Always update the database context when adding new entities
- Run migrations after model changes
- Keep controllers focused on HTTP concerns
- Move business logic to service layers as the application grows
- Consider adding authentication and authorization as the API matures
