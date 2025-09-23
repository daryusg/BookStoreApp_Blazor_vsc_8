using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Providers;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Authentication;

public class AuthenticationService : BaseHttpService, IAuthenticationService //cip...39, 67
{
    private readonly IClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    //private readonly AuthenticationStateProvider _authenticationStateProvider; cip...55
    private readonly ApiAuthenticationStateProvider _authenticationStateProvider;

    //public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider) cip...55
    public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, ApiAuthenticationStateProvider authenticationStateProvider) : base(httpClient, localStorage) //cip...67
    {
        _httpClient = httpClient;
        this._localStorage = localStorage;
        this._authenticationStateProvider = authenticationStateProvider;
        Console.WriteLine($"[DEBUG] AuthenticationStateProvider injected type: {_authenticationStateProvider.GetType().FullName}"); //cip...55 chatgpt debug
    }

    //public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
    public async Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel) //cip...67
    {
        //var response = await _httpClient.LoginAsync(loginModel);
        Response<AuthResponse> response; //cip...67
        try
        {
            //response = await _httpClient.LoginAsync(loginModel);
            var result = await _httpClient.LoginAsync(loginModel); //cip...67
            response = new Response<AuthResponse> { Data = result, Success = true }; //cip...67
            await _localStorage.SetItemAsync("authToken", result.Token);

            //change auth state of app
            //await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedInAsync(); cip...55
            await _authenticationStateProvider.LoggedInAsync();
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<AuthResponse>(ex); //cip...67
        }
        return response;
    }

    public async Task LogoutAsync() //cip...40
    {
        //await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOutAsync();  cip...55
        await _authenticationStateProvider.LoggedOutAsync();
    }
}