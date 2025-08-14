using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.UI.Providers;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider //cip...39
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
    private readonly IAuthenticationService _authenticationService;

    public ApiAuthenticationStateProvider(ILocalStorageService localStorage, IAuthenticationService authenticationService)
    {
        _localStorage = localStorage;
        _authenticationService = authenticationService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity()); //empty claims principal

        var token = await _localStorage.GetItemAsync<string>("authToken");
        //var user = string.IsNullOrEmpty(token) ? null : new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt")); copilot
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(user);
        }

        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(token);
        if (tokenContent.ValidTo < DateTime.Now)
        {
            return new AuthenticationState(user);
        }

        //var claims = tokenContent.Claims;
        var claims = await GetClaimsAsync(); //cip...40

        user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        return new AuthenticationState(user);
    }

    public async Task LoggedInAsync()
    {
        var claims = await GetClaimsAsync(); //cip...40
        var user = await GetAuthenticationStateAsync();
        var authState = Task.FromResult(user);
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task LoggedOutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    private async Task<List<Claim>> GetClaimsAsync() //cip...40
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");
        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
        var claims = tokenContent.Claims.ToList();
        claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
        return claims.ToList();
    }
}
