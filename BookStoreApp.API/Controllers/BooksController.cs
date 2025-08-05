using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.Data.Models.Book;
using AutoMapper.QueryableExtensions;

namespace BookStoreApp.API.Controllers
{
    //cip...23
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(BookStoreDbContext context, IMapper mapper) //cip...24
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: api/Books
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            //return await _context.Books.ToListAsync();
            //var books = await _context.Books.Include(q => q.Author).ToListAsync(); // Include = LEFT (OUTER?) JOIN
            //or
            //var books = await _context.Books
            var booksDto = await _context.Books
                .Include(q => q.Author)
                .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider) // Use AutoMapper to project directly to BookReadOnlyDto
                .ToListAsync(); // Include = LEFT (OUTER?) JOIN
            //var booksDto = _mapper.Map<IEnumerable<BookReadOnlyDto>>(books); //not needed when using ProjectTo
            return Ok(booksDto);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            //var book = await _context.Books.FindAsync(id);
            var bookDto = await _context.Books
                .Include(q => q.Author)
                .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider) // Use AutoMapper to project directly to BookReadOnlyDto
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bookDto == null)
            {
                return NotFound();
            }

            return bookDto;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _mapper.Map(bookDto, book); // Map BookUpdateDto to Book
            _context.Entry(bookDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExistsAsync(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto); // Map BookCreateDto to Book
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> BookExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(e => e.Id == id);
        }
    }
}
