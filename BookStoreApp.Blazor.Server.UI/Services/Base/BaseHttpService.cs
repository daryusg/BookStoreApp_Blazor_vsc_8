using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace BookStoreApp.Blazor.Server.UI.Services.Base;

public class BaseHttpService //cip...44
{
    private readonly IClient _client;
    private readonly ILocalStorageService _localStorage;

    public BaseHttpService(IClient client, ILocalStorageService localStorage)
    {
        this._client = client;
        this._localStorage = localStorage;
    }

    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException apiException)
    {

        switch (apiException.StatusCode)
        {
            case >= 200 and <= 299:
                {
                    return new Response<Guid>
                    {
                        Message = "Operation completed successfully.",
                        Success = true
                    };
                }
            case 400: //bad request
                {
                    return new Response<Guid>
                    {
                        Message = "Validation errors have occurred.",
                        ValidationErrors = apiException.Response,
                        Success = false
                    };
                }
            case 401: //copilot
                {
                    return new Response<Guid>
                    {
                        Message = "You are not authorized to perform this action.",
                        Success = false
                    };
                }
            case 404:
                {
                    return new Response<Guid>
                    {
                        Message = "The requested item could not be found.",
                        Success = false
                    };
                }
            default:
                {
                    return new Response<Guid>
                    {
                        Message = "Something went wrong, please try again.",
                        Success = false
                    };
                }
        }
    }

    protected async Task GetBearerTokenAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
