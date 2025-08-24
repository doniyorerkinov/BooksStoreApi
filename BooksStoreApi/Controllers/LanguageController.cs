using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksStoreApi.Data;
using BooksStoreApi.Models;

namespace BooksStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly BooksStoreContext _context;

        /// <summary>
        /// Constructor that injects the database context for accessing Language data
        /// </summary>
        /// <param name="context">The database context</param>
        public LanguageController(BooksStoreContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Language
        /// Retrieves all Languages from the database
        /// </summary>
        /// <returns>A list of all Languages</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Language>>> GetList()
        {
            if (_context.Languages == null)
            {
                return NotFound("Languages collection is not available.");
            }
            return await _context.Languages.ToListAsync();
        }

        /// <summary>
        /// GET: api/Language/5
        /// Retrieves a specific Language by ID
        /// </summary>
        /// <param name="id">The ID of the Language to retrieve</param>
        /// <returns>The Language with the specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Language>> GetLanguage(int id)
        {
            if (_context.Languages == null)
            {
                return NotFound("Languages collection is not available.");
            }

            var Language = await _context.Languages.FindAsync(id);

            if (Language == null)
            {
                return NotFound($"Language with ID {id} was not found.");
            }

            return Language;
        }

        /// <summary>
        /// PUT: api/Language/5
        /// Updates an existing Language's information
        /// </summary>
        /// <param name="id">The ID of the Language to update</param>
        /// <param name="Language">The updated Language data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLanguage(int id, Language Language)
        {
            if (id != Language.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the request body.");
            }

            _context.Entry(Language).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(id))
                {
                    return NotFound($"Language with ID {id} was not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// POST: api/Language
        /// Creates a new Language in the database
        /// </summary>
        /// <param name="Language">The Language data to create</param>
        /// <returns>The created Language with assigned ID</returns>
        [HttpPost]
        public async Task<ActionResult<Language>> PostLanguage(Language Language)
        {
            if (_context.Languages == null)
            {
                return Problem("Entity set 'BooksStoreContext.Languages' is null.");
            }

            _context.Languages.Add(Language);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLanguage", new { id = Language.Id }, Language);
        }

        /// <summary>
        /// DELETE: api/Language/5
        /// Deletes a Language by ID
        /// Note: This will only work if there are no books associated with the Language
        /// </summary>
        /// <param name="id">The ID of the Language to delete</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            if (_context.Languages == null)
            {
                return NotFound("Languages collection is not available.");
            }
            var Language = await _context.Languages.FindAsync(id);
            if (Language == null)
            {
                return NotFound();
            }

            _context.Languages.Remove(Language);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Helper method to check if a Language exists
        /// </summary>
        /// <param name="id">The ID to check</param>
        /// <returns>True if the Language exists, false otherwise</returns>
        private bool LanguageExists(int id)
        {
            return (_context.Languages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
