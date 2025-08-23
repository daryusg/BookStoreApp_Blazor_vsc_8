using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services;

public interface IAuthorService //cip...44
{
    Task<Response<List<AuthorReadOnlyDto>>> GetAuthorsAsync();
    Task<Response<AuthorReadOnlyDto>> GetAuthorAsync(int id); //cip...46
    Task<Response<int>> CreateAsync(AuthorCreateDto author); //cip...45
    Task<Response<int>> EditAsync(int id, AuthorUpdateDto author); //cip...46
}