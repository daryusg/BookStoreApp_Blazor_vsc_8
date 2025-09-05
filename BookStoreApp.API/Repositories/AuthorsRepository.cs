using BookStoreApp.API.Data;

namespace BookStoreApp.API.Repositories;

public class AuthorsRepository : GenericRepository<Author>, IAuthorsRepository //cip...64
{
    public AuthorsRepository(BookStoreDbContext context) : base(context)
    {
    }
}