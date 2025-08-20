
namespace BooksStoreApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Isbn { get; set; } = string.Empty;

        // Foreign keys for relationships
        public int AuthorId { get; set; }
        public int LibraryId { get; set; }
        public int BookCategoryId { get; set; }
        public int LanguageId { get; set; }

        // Navigation properties
        public Author Author { get; set; } = null!;
        public Library Library { get; set; } = null!;
        public BookCategory BookCategory { get; set; } = null!;
        public Language Language { get; set; } = null!;
    }
}