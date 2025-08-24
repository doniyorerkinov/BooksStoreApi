using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksStoreApi.Data;
using BooksStoreApi.Models;

namespace BooksStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly BooksStoreContext _context;

        /// <summary>
        /// Constructor that injects the database context for accessing library data
        /// </summary>
        /// <param name="context">The database context</param>
        public LibraryController(BooksStoreContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Library
        /// Retrieves all libraries from the database
        /// </summary>
        /// <returns>A list of all libraries</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
        {
            if (_context.Libraries == null)
            {
                return NotFound("Libraries collection is not available.");
            }
            return await _context.Libraries.ToListAsync();
        }

        /// <summary>
        /// GET: api/Library/5
        /// Retrieves a specific library by ID
        /// </summary>
        /// <param name="id">The ID of the library to retrieve</param>
        /// <returns>The library with the specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Library>> GetLibrary(int id)
        {
            if (_context.Libraries == null)
            {
                return NotFound("Libraries collection is not available.");
            }

            var library = await _context.Libraries.FindAsync(id);

            if (library == null)
            {
                return NotFound($"Library with ID {id} was not found.");
            }

            return library;
        }

        /// <summary>
        /// PUT: api/Library/5
        /// Updates an existing library's information
        /// </summary>
        /// <param name="id">The ID of the library to update</param>
        /// <param name="library">The updated library data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibrary(int id, Library library)
        {
            if (id != library.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the request body.");
            }

            _context.Entry(library).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryExists(id))
                {
                    return NotFound($"Library with ID {id} was not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// POST: api/Library
        /// Creates a new library in the database
        /// </summary>
        /// <param name="library">The library data to create</param>
        /// <returns>The created library with assigned ID</returns>
        [HttpPost]
        public async Task<ActionResult<Library>> PostLibrary(Library library)
        {
            if (_context.Libraries == null)
            {
                return Problem("Entity set 'BooksStoreContext.Libraries' is null.");
            }

            _context.Libraries.Add(library);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLibrary", new { id = library.Id }, library);
        }

        /// <summary>
        /// DELETE: api/Library/5
        /// Deletes a library by ID
        /// Note: This will only work if there are no books associated with the library
        /// </summary>
        /// <param name="id">The ID of the library to delete</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibrary(int id)
        {
            if (_context.Libraries == null)
            {
                return NotFound("Libraries collection is not available.");
            }
            var library = await _context.Libraries.FindAsync(id);
            if (library == null)
            {
                return NotFound();
            }

            _context.Libraries.Remove(library);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Helper method to check if a library exists
        /// </summary>
        /// <param name="id">The ID to check</param>
        /// <returns>True if the library exists, false otherwise</returns>
        private bool LibraryExists(int id)
        {
            return (_context.Libraries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
