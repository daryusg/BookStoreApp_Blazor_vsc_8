using System.Security.Authentication;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Providers;
using BookStoreApp.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication;

public class AuthenticationService : IAuthenticationService //cip...39
{
    private readonly IClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    //private readonly AuthenticationStateProvider _authenticationStateProvider; cip...55
    private readonly ApiAuthenticationStateProvider _authenticationStateProvider;

    //public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider) cip...55
    public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, ApiAuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        this._localStorage = localStorage;
        this._authenticationStateProvider = authenticationStateProvider;
        Console.WriteLine($"[DEBUG] AuthenticationStateProvider injected type: {_authenticationStateProvider.GetType().FullName}"); //cip...55 chatgpt debug
    }

    public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
    {
        var response = await _httpClient.LoginAsync(loginModel);
        if (response != null)
        {
            // Handle successful authentication, e.g., store token, navigate to home page
            //store token
            await _localStorage.SetItemAsync("authToken", response.Token);

            //change auth state of app
            //await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedInAsync(); cip...55
            await _authenticationStateProvider.LoggedInAsync();

            return true;
        }
        else
        {
            // Handle authentication failure
            throw new Exception("Authentication failed");
        }
    }

    public async Task LogoutAsync() //cip...40
    {
        //await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOutAsync();  cip...55
        await _authenticationStateProvider.LoggedOutAsync();
    }
}