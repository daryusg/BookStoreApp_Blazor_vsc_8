using BookStoreApp.API.Data;
using BookStoreApp.API.Data.Models.Author;

namespace BookStoreApp.API.Repositories;

public interface IAuthorsRepository : IGenericRepository<Author> //cip...64
{
    Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id);
}
