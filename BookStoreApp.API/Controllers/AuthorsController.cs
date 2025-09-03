using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Data.Models.Author;
using AutoMapper;
using BookStoreApp.API.Static; //cip...49. my extra implementation
using Microsoft.AspNetCore.Authorization;
using AutoMapper.QueryableExtensions;

namespace BookStoreApp.API.Controllers
{
    //cip...18
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure that only authenticated users can access this controller
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper; //cip...19
        private readonly ILogger _logger; //cip...20

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger) //cip...19, 20
        {
            _context = context;
            this._mapper = mapper;
            this._logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors() //cip...19
        {
            _logger.LogInformation($"Request to {nameof(GetAuthors)}"); //cip...20
            try //cip...20
            {
                //return await _context.Authors.ToListAsync();
                var authors = await _context.Authors.ToListAsync();
                var authorsDto = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
                return Ok(authorsDto); // Return the mapped list of authorsDto
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        //public async Task<ActionResult<Author>> GetAuthor(int id)
        //public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id) //cip...19
        public async Task<ActionResult<AuthorDetailsDto>> GetAuthor(int id) //cip...47
        {
            try //cip...20
            {
                var author = await _context.Authors
                    .Include(a => a.Books) // Include related books
                    .ProjectTo<AuthorDetailsDto>(_mapper.ConfigurationProvider) // Project to AuthorDetailsDto
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (author == null)
                {
                    _logger.LogWarning($"Author (id = {id}) not found in {nameof(GetAuthor)}"); //cip...20
                    return NotFound();
                }

                //return author;
                //var authorDto = _mapper.Map<AuthorReadOnlyDto>(author); // Map Author to AuthorReadOnlyDto //cip...47 now not needed as we are using ProjectTo above
                //return Ok(authorDto); // Return the mapped authorDto
                return Ok(author); // Return the .ProjectTo<AuthorDetailsDto> result //cip...47
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in {nameof(GetAuthor)} with id = {id}");
                return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Administrator)]

        //public async Task<IActionResult> PutAuthor(int id, Author author)
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)//cip...19
        {
            try //cip...20
            {
                if (id != authorDto.Id)
                {
                    return BadRequest();
                }

                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)} with id = {id}.");
                    return NotFound();
                }

                _mapper.Map(authorDto, author); // Map AuthorUpdateDto to Author (ie copy data from authorDto to author)

                _context.Entry(author).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!AuthorExists(id))
                    if (!await AuthorExistsAsync(id)) //tw update
                    {
                        return NotFound();
                    }
                    else
                    {
                        //throw;
                        return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
                    }
                }

                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in {nameof(PutAuthor)} with id = {id}.");
                return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
            }
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = Roles.Administrator)]
        //public async Task<ActionResult<Author>> PostAuthor(Author author)
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto) //cip...19
        {
            try //cip...20
            {
                //i can, either, map here (see below) or use AutoMapper here to map authorDto (AuthorCreateDto) to author (Author)
                // var author = new Author
                // {
                //     FirstName = authorDto.FirstName,
                //     LastName = authorDto.LastName,
                //     Bio = authorDto.Bio
                // };
                var author = _mapper.Map<Author>(authorDto);
                //_context.Authors.Add(author);
                await _context.Authors.AddAsync(author); //tw update
                await _context.SaveChangesAsync();

                //return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author); //tw update (to use nameof)
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in {nameof(PostAuthor)}.");
                return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try //cip...20
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"{nameof(Author)} record not found in {nameof(DeleteAuthor)} with id = {id}.");
                    return NotFound();
                }

                try //cip...48 added by me to handle deletion failure eg fk violation
                {
                    _context.Authors.Remove(author);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var errMsg = $"An error occurred while deleting ({nameof(DeleteAuthor)} with id = {id}).{Environment.NewLine}{ex.Message}";
                    _logger.LogError(ex, errMsg);
                    return StatusCode(500, errMsg /*Messages.Error500Message*/); // Return a 500 Internal Server Error response. NOTE: currently handled in BaseHttpService.ConvertApiExceptions as "Something went wrong, please try again."
                }

                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in {nameof(DeleteAuthor)} with id = {id}.");
                return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
            }
        }

        /*
        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
        */
        private async Task<bool> AuthorExistsAsync(int id)  //tw update
        {
            return await _context.Authors.AnyAsync(e => e.Id == id);
        }
    }
}
