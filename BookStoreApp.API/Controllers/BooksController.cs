using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using BookStoreApp.API.Static;
using BookStoreApp.API.Repositories;
using BookStoreApp.API.Models.Book;

namespace BookStoreApp.API.Controllers
{
    //cip...23
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure that only authenticated users can access this controller
    public class BooksController : ControllerBase
    {
        //private readonly BookStoreDbContext _context;
        private readonly IBooksRepository _booksRepository; //cip...64
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment; //cip...55

        public BooksController(IBooksRepository booksRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment) //cip...24, 55, 64
        {
            _booksRepository = booksRepository;
            this._mapper = mapper;
            this._webHostEnvironment = webHostEnvironment;
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
            // var booksDto = await _booksRepository.Books
            //     .Include(q => q.Author)
            //     .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider) // Use AutoMapper to project directly to BookReadOnlyDto
            //     .ToListAsync(); // Include = LEFT (OUTER?) JOIN
            var booksDto = await _booksRepository.GetAllBooksAsync(); //cip...64
            //var booksDto = _mapper.Map<IEnumerable<BookReadOnlyDto>>(books); //not needed when using ProjectTo
            return Ok(booksDto);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            //var book = await _context.Books.FindAsync(id);
            // var bookDto = await _booksRepository.Books
            //     .Include(q => q.Author)
            //     .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider) // Use AutoMapper to project directly to BookDetailsDto
            //     .FirstOrDefaultAsync(b => b.Id == id);
            var bookDto = await _booksRepository.GetBookAsync(id); //cip...64
            if (bookDto == null)
            {
                return NotFound();
            }

            return bookDto;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Administrator)] // Ensure that only users with the Administrator role can update books
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            //var book = await _booksRepository.Books.FindAsync(id);
            var book = await _booksRepository.GetAsync(id); //cip...64
            if (book == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(bookDto.ImageData)) //cip...57
            {
                //create the new image file
                bookDto.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName);

                //delete the new image file
                var picName = Path.GetFileName(book.Image);
                var path = $"{_webHostEnvironment.WebRootPath}\\images\\bookcovers\\{picName}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            _mapper.Map(bookDto, book); // Map BookUpdateDto to Book
            //_booksRepository.Entry(book).State = EntityState.Modified; cip...64 not needed

            try
            {
                //await _booksRepository.SaveChangesAsync();
                await _booksRepository.UpdateAsync(book); //cip...64
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
        [Authorize(Roles = Roles.Administrator)]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto); // Map BookCreateDto to Book
            book.Image = CreateFile(bookDto.ImageData, bookDto.OriginalImageName); //cip...55
            //_booksRepository.Books.Add(book);
            await _booksRepository.AddAsync(book); //cip...64
            //await _booksRepository.SaveChangesAsync(); cip...64 not needed

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            //var book = await _booksRepository.Books.FindAsync(id);
            var book = await _booksRepository.GetAsync(id); //cip...64
            if (book == null)
            {
                return NotFound();
            }

            //_booksRepository.Books.Remove(book);
            await _booksRepository.DeleteAsync(id); //cip...64
            //await _booksRepository.SaveChangesAsync(); cip...64 not needed

            return NoContent();
        }

        private string CreateFile(string imageBase64, string imageName) //cip...55
        {
            var url = HttpContext.Request.Host.Value;
            var ext = Path.GetExtension(imageName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var path = $"{_webHostEnvironment.WebRootPath}\\images\\bookcovers\\{fileName}";

            byte[] image = Convert.FromBase64String(imageBase64);

            var fileStream = System.IO.File.Create(path);
            fileStream.Write(image, 0, image.Length);
            fileStream.Close();

            return $"https://{url}//images//bookcovers//{fileName}";
        }

        private async Task<bool> BookExistsAsync(int id)
        {
            //return await _booksRepository.Books.AnyAsync(e => e.Id == id);
            return await _booksRepository.Exists(id); //cip...64
        }
    }
}
