using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services;

public interface IAuthorService //cip...44
{
    Task<Response<List<AuthorReadOnlyDto>>> GetAuthorsAsync();
    
}