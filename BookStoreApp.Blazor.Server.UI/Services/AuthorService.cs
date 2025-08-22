using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services;

public class AuthorService : BaseHttpService, IAuthorService //cip...44
{
    private readonly IClient _client;

    public AuthorService(IClient client, ILocalStorageService localStorage) : base(client, localStorage)
    {
        this._client = client;
    }

    public async Task<Response<List<AuthorReadOnlyDto>>> GetAuthorsAsync()
    {
        Response<List<AuthorReadOnlyDto>> response;

        try
        {
            await GetBearerTokenAsync();
            var data = await _client.AuthorsAllAsync();
            response = new Response<List<AuthorReadOnlyDto>>
            {
                Data = data.ToList(),
                Success = true
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<List<AuthorReadOnlyDto>>(ex);
        }
        return response;
    }
}
