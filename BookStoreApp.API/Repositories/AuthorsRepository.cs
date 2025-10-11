using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories;

public class AuthorsRepository : GenericRepository<Author>, IAuthorsRepository //cip...64
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthorsRepository> _logger; //cip...72 (chatgpt)

    public AuthorsRepository(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsRepository> logger) : base(context, mapper, logger)  //cip...72 (chatgpt)
    {
        this._context = context;
        this._mapper = mapper;
        this._logger = logger;
    }

    public async Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id)
    {
        var author = await _context.Authors
            .Include(a => a.Books) // Include related books
            .ProjectTo<AuthorDetailsDto>(_mapper.ConfigurationProvider) // Project to AuthorDetailsDto
            .FirstOrDefaultAsync(q => q.Id == id);

        return author;
    }
}