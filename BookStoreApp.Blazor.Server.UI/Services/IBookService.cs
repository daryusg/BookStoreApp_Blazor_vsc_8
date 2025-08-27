using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services;

public interface IBookService //cip...52
{
    Task<Response<List<BookReadOnlyDto>>> GetAsync();
    Task<Response<BookDetailsDto>> GetAsync(int id);
    Task<Response<int>> CreateAsync(BookCreateDto book);
    Task<Response<int>> EditAsync(int id, BookUpdateDto book);
    Task<Response<int>> DeleteAsync(int id);
}