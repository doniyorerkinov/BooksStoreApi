using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksStoreApi.Data;
using BooksStoreApi.Models;

namespace BooksStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryCantroller : ControllerBase
    {
        private readonly BooksStoreContext _context;

        /// <summary>
        /// Constructor that injects the database context for accessing Category data
        /// </summary>
        /// <param name="context">The database context</param>
        public CategoryCantroller(BooksStoreContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Category
        /// Retrieves all BookCategories from the database
        /// </summary>
        /// <returns>A list of all BookCategories</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookCategory>>> GetCategories()
        {
            if (_context.BookCategories == null)
            {
                return NotFound("BookCategories collection is not available.");
            }
            return await _context.BookCategories.ToListAsync();
        }

        /// <summary>
        /// GET: api/Category/5
        /// Retrieves a specific BookCategory by ID
        /// </summary>
        /// <param name="id">The ID of the BookCategory to retrieve</param>
        /// <returns>The BookCategory with the specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookCategory>> GetCategory(int id)
        {
            if (_context.BookCategories == null)
            {
                return NotFound("BookCategories collection is not available.");
            }

            var category = await _context.BookCategories.FindAsync(id);

            if (category == null)
            {
                return NotFound($"Category with ID {id} was not found.");
            }

            return category;
        }

        /// <summary>
        /// PUT: api/Category/5
        /// Updates an existing BookCategory's information
        /// </summary>
        /// <param name="id">The ID of the BookCategory to update</param>
        /// <param name="bookCategory">The updated BookCategory data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, BookCategory bookCategory)
        {
            if (id != bookCategory.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the request body.");
            }

            _context.Entry(bookCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound($"Category with ID {id} was not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// POST: api/Category
        /// Creates a new BookCategory in the database
        /// </summary>
        /// <param name="bookCategory">The BookCategory data to create</param>
        /// <returns>The created BookCategory with assigned ID</returns>
        [HttpPost]
        public async Task<ActionResult<BookCategory>> PostCategory(BookCategory bookCategory)
        {
            if (_context.BookCategories == null)
            {
                return Problem("Entity set 'BooksStoreContext.BookCategories' is null.");
            }

            _context.BookCategories.Add(bookCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = bookCategory.Id }, bookCategory);
        }

        /// <summary>
        /// DELETE: api/Category/5
        /// Deletes a BookCategory by ID
        /// Note: This will only work if there are no books associated with the category
        /// </summary>
        /// <param name="id">The ID of the BookCategory to delete</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.BookCategories == null)
            {
                return NotFound("BookCategories collection is not available.");
            }
            var category = await _context.BookCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.BookCategories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Helper method to check if a BookCategory exists
        /// </summary>
        /// <param name="id">The ID to check</param>
        /// <returns>True if the BookCategory exists, false otherwise</returns>
        private bool CategoryExists(int id)
        {
            return (_context.BookCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
