using BookStoreApp.API.Data;
using BookStoreApp.API.Data.Models.Book;

namespace BookStoreApp.API.Repositories;

public interface IBooksRepository : IGenericRepository<Book> //cip...64
{
    Task<List<BookReadOnlyDto>> GetAllBooksAsync();
    Task<BookDetailsDto> GetBookAsync(int id);

}
