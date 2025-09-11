using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Data.Models.Author;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories;

public class AuthorsRepository : GenericRepository<Author>, IAuthorsRepository //cip...64
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public AuthorsRepository(BookStoreDbContext context, IMapper mapper) : base(context)
    {
        this._context = context;
        this._mapper = mapper;
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