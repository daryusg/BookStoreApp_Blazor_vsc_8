using BookStoreApp.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IClient _httpClient;
    private readonly NavigationManager _navManager;

    public AuthenticationService(IClient httpClient, NavigationManager navManager)
    {
        _httpClient = httpClient;
        _navManager = navManager;
    }

    public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
    {
        var response = await _httpClient.LoginAsync(loginModel);
        if (response != null)
        {
            // Handle successful authentication, e.g., store token, navigate to home page
            //store token
            //change auth state of app
            return response != null;
        }
        else
        {
            // Handle authentication failure
            throw new Exception("Authentication failed");
        }
    }
}
