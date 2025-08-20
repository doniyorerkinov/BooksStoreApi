using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksStoreApi.Data;
using BooksStoreApi.Models;

namespace BooksStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BooksStoreContext _context;

        // The constructor injects the database context, making it available
        // for all the controller's methods.
        public AuthorsController(BooksStoreContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        // This method retrieves a list of all authors from the database.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            // Check if the Authors DbSet is null before attempting to access it.
            if (_context.Authors == null)
            {
                return NotFound();
            }
            return await _context.Authors.ToListAsync();
        }

        // GET: api/Authors/5
        // This method retrieves a single author by their ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }

            // Find the author with the specified ID.
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Authors/5
        // This method updates an existing author's information.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            // Ensure the ID in the URL matches the ID in the request body.
            if (id != author.Id)
            {
                return BadRequest();
            }

            // Tell the context to track the entity as modified.
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // This method creates a new author in the database.
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            if (_context.Authors == null)
            {
                // This indicates a server configuration issue if the DbSet is null.
                return Problem("Entity set 'BooksStoreContext.Authors'  is null.");
            }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            // Returns a 201 CreatedAtAction response with the newly created author.
            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        // This method deletes an author by their ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
