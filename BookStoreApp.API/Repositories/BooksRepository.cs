using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Data.Models.Book;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories;

public class BooksRepository : GenericRepository<Book>, IBooksRepository //cip...64
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public BooksRepository(BookStoreDbContext context, IMapper mapper) : base(context)
    {
        this._context = context;
        this._mapper = mapper;
    }

    public async Task<List<BookReadOnlyDto>> GetAllBooksAsync()
    {
        var booksDto = await _context.Books
            .Include(q => q.Author)
            .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider) // Use AutoMapper to project directly to BookReadOnlyDto
            .ToListAsync(); // Include = LEFT (OUTER?) JOIN
        return booksDto;
    }

    public async Task<BookDetailsDto> GetBookAsync(int id)
    {
        var bookDto = await _context.Books
            .Include(q => q.Author)
            .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider) // Use AutoMapper to project directly to BookDetailsDto
            .FirstOrDefaultAsync(b => b.Id == id);
        return bookDto;
    }
}
