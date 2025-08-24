# BooksStore API

A comprehensive RESTful API for managing a bookstore system with libraries, books, authors, categories, and languages.

## Overview

The BooksStore API provides endpoints to manage:

- **Authors** - Book authors with first and last names
- **Libraries** - Physical locations that house books
- **Books** - Individual book items with metadata
- **Book Categories** - Genre classifications for books
- **Languages** - Language information for books

## Architecture

This API is built using:

- **ASP.NET Core 6.0** - Web framework
- **Entity Framework Core** - ORM for database operations
- **SQLite** - Database (with PostgreSQL support)
- **Swagger/OpenAPI** - API documentation

## Database Schema

### Core Entities

1. **Author**

   - Id (Primary Key)
   - FirstName
   - LastName

2. **Library**

   - Id (Primary Key)
   - Name
   - Address

3. **Book**

   - Id (Primary Key)
   - Title
   - Year
   - ISBN
   - AuthorId (Foreign Key)
   - LibraryId (Foreign Key)
   - BookCategoryId (Foreign Key)
   - LanguageId (Foreign Key)

4. **BookCategory**

   - Id (Primary Key)
   - Name

5. **Language**
   - Id (Primary Key)
   - Name

### Relationships

- **Book → Author**: Many-to-One (One author can write many books)
- **Book → Library**: Many-to-One (One library can house many books)
- **Book → BookCategory**: Many-to-One (One category can have many books)
- **Book → Language**: Many-to-One (One language can have many books)

## API Endpoints

### Authors

- `GET /api/Authors` - Get all authors
- `GET /api/Authors/{id}` - Get author by ID
- `POST /api/Authors` - Create new author
- `PUT /api/Authors/{id}` - Update author
- `DELETE /api/Authors/{id}` - Delete author

### Libraries

- `GET /api/Libraries` - Get all libraries
- `GET /api/Libraries/{id}` - Get library by ID
- `GET /api/Libraries/{id}/books` - Get all books in a library
- `POST /api/Libraries` - Create new library
- `PUT /api/Libraries/{id}` - Update library
- `DELETE /api/Libraries/{id}` - Delete library

## Getting Started

### Prerequisites

- .NET 6.0 SDK
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository

```bash
git clone <repository-url>
cd BooksStoreApi
```

2. Restore packages

```bash
dotnet restore
```

3. Update database

```bash
dotnet ef database update
```

4. Run the application

```bash
dotnet run
```

The API will be available at `https://localhost:7xxx` and `http://localhost:5xxx`

### API Documentation

Once running, visit:

- Swagger UI: `https://localhost:7xxx/swagger`
- OpenAPI specification: `https://localhost:7xxx/swagger/v1/swagger.json`

## Development

### Project Structure

```
BooksStoreApi/
├── Controllers/          # API controllers
├── Models/              # Entity models
├── Data/                # Database context
├── Migrations/          # EF migrations
├── Properties/          # Launch settings
├── Program.cs           # Application entry point
└── appsettings.json     # Configuration
```

### Entity Framework

The project uses Entity Framework Core with migrations. To add a new migration:

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Configuration

### Database Connection

The application supports both SQLite (default) and PostgreSQL. Configure in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=bookstore.db",
    "PostgreSQLConnection": "Host=localhost;Database=bookstore;Username=user;Password=pass"
  }
}
```

### Development Environment

Development-specific settings are in `appsettings.Development.json` with detailed logging enabled.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License.
