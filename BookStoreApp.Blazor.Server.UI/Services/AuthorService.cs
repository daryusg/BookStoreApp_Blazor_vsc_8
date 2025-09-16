using Blazored.LocalStorage;
using BookStoreApp.API.Models;
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
        Response<int> response = new();

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

    public async Task<Response<int>> DeleteAsync(int id) //cip...48
    {
        Response<int> response = new();

        try
        {
            await GetBearerTokenAsync();
            await _client.AuthorsDELETEAsync(id);
        }
        catch (ApiException apiEx)
        {
            response = ConvertApiExceptions<int>(apiEx);
        }
        return response;
    }

    public async Task<Response<int>> EditAsync(int id, AuthorUpdateDto author) //cip...46
    {
        Response<int> response = new();

        try
        {
            await GetBearerTokenAsync();
            await _client.AuthorsPUTAsync(id, author);
        }
        catch (ApiException apiEx)
        {
            response = ConvertApiExceptions<int>(apiEx);
        }
        return response;
    }

    //public async Task<Response<AuthorReadOnlyDto>> GetAuthorAsync(int id) //cip...46
    public async Task<Response<AuthorDetailsDto>> GetAsync(int id) //cip...47
    {
        Response<AuthorDetailsDto> response;

        try
        {
            await GetBearerTokenAsync();
            //var data = await _client.AuthorsGETAsync(id);
            var data = await _client.AuthorsGET2Async(id); //cip...66 (NOTE: 65 changed to 66)
            response = new Response<AuthorDetailsDto>
            {
                Data = data,
                Success = true
            };
        }
        catch (ApiException apiEx)
        {
            response = ConvertApiExceptions<AuthorDetailsDto>(apiEx);
        }
        return response;
    }

    //public async Task<Response<List<AuthorReadOnlyDto>>> GetAsync()
    public async Task<Response<AuthorReadOnlyDtoVirtualiseResponse>> GetAsync(QueryParameters queryParams) //cip...66
    {
        //Response<List<AuthorReadOnlyDto>> response;
        Response<AuthorReadOnlyDtoVirtualiseResponse> response; //cip...66

        try
        {
            await GetBearerTokenAsync();
            //var data = await _client.AuthorsAllAsync();
            //var data = await _client.AuthorsGETAsync(); //cip...65
            var data = await _client.AuthorsGETAsync(queryParams.PageNumber, queryParams.PageSize); //cip...66
            //response = new Response<List<AuthorReadOnlyDto>>
            response = new Response<AuthorReadOnlyDtoVirtualiseResponse> //cip...66
            {
                Data = data,
                Success = true
            };
        }
        catch (ApiException apiEx)
        {
            //response = ConvertApiExceptions<List<AuthorReadOnlyDto>>(apiEx);
            response = ConvertApiExceptions<AuthorReadOnlyDtoVirtualiseResponse>(apiEx); //cip...66
        }
        return response;
    }
}
