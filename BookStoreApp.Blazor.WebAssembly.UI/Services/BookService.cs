using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services;

public class BookService : BaseHttpService, IBookService //cip...52
{
    private readonly IClient _client;

    public BookService(IClient client, ILocalStorageService localStorage) : base(client, localStorage)
    {
        this._client = client;
    }

    public async Task<Response<int>> CreateAsync(BookCreateDto book)
    {
        Response<int> response = new();

        try
        {
            await GetBearerTokenAsync();
            await _client.BooksPOSTAsync(book);
        }
        catch (ApiException apiEx)
        {
            response = ConvertApiExceptions<int>(apiEx);
        }
        return response;
    }

    public async Task<Response<int>> DeleteAsync(int id)
    {
        Response<int> response = new();

        try
        {
            await GetBearerTokenAsync();
            await _client.BooksDELETEAsync(id);
        }
        catch (ApiException apiEx)
        {
            response = ConvertApiExceptions<int>(apiEx);
        }
        return response;
    }

    public async Task<Response<int>> EditAsync(int id, BookUpdateDto book)
    {
        Response<int> response = new();

        try
        {
            await GetBearerTokenAsync();
            await _client.BooksPUTAsync(id, book);
        }
        catch (ApiException apiEx)
        {
            response = ConvertApiExceptions<int>(apiEx);
        }
        return response;
    }

    public async Task<Response<BookDetailsDto>> GetAsync(int id)
    {
        Response<BookDetailsDto> response;

        try
        {
            await GetBearerTokenAsync();
            var data = await _client.BooksGETAsync(id);
            response = new Response<BookDetailsDto>
            {
                Data = data,
                Success = true
            };
        }
        catch (ApiException apiEx)
        {
            response = ConvertApiExceptions<BookDetailsDto>(apiEx);
        }
        return response;
    }

    public async Task<Response<List<BookReadOnlyDto>>> GetAsync()
    {
        Response<List<BookReadOnlyDto>> response;

        try
        {
            await GetBearerTokenAsync();
            var data = await _client.BooksAllAsync();
            response = new Response<List<BookReadOnlyDto>>
            {
                Data = data.ToList(),
                Success = true
            };
        }
        catch (ApiException apiEx)
        {
            response = ConvertApiExceptions<List<BookReadOnlyDto>>(apiEx);
        }
        return response;
    }
}
