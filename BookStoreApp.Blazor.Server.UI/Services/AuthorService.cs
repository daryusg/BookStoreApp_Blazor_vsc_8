using System.Runtime.CompilerServices;
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

    public async Task<Response<int>> CreateAsync(AuthorCreateDto author) //cip...45
    {
        Response<int> response = new() { Success = true };

        try
        {
            await GetBearerTokenAsync();
            await _client.AuthorsPOSTAsync(author);
        }
        catch (ApiException apiEx)
        {
            response = ConvertApiExceptions<int>(apiEx);
        }
        return response;
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
        catch (ApiException apiEx)
        {
            response = ConvertApiExceptions<List<AuthorReadOnlyDto>>(apiEx);
        }
        return response;
    }
}
