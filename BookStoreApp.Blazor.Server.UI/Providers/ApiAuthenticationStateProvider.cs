using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.UI.Providers;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider //cip...39
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity()); //empty claims principal

        var token = await _localStorage.GetItemAsync<string>("authToken");
        //var user = string.IsNullOrEmpty(token) ? null : new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt")); copilot
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(anonymousUser);
        }

        try
        {
            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(token);
            if (tokenContent.ValidTo < DateTime.UtcNow)
                return new AuthenticationState(anonymousUser);

            //var claims = tokenContent.Claims;
            var claims = await GetClaimsAsync(); //cip...40
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            return new AuthenticationState(user);
        }
        catch
        {
            // fallback to anonymous user
            await _localStorage.RemoveItemAsync("authToken");
            return new AuthenticationState(anonymousUser);
        }
        
    }

    public async Task LoggedInAsync()
    {
        var authState = await GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
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
        return claims;
    }
}
