using BooksStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksStoreApi.Data
{
    public class BooksStoreContext : DbContext
    {
        public BooksStoreContext(DbContextOptions<BooksStoreContext> options) : base(options)
        {
        }

        // Define a DbSet for each entity to represent the database tables
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Library> Libraries { get; set; } = null!;
        public DbSet<BookCategory> BookCategories { get; set; } = null!;
        public DbSet<Language> Languages { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationships between your entities here if needed.
            // EF Core will handle most relationships by convention, but you can
            // customize them here.
        }
    }
}
