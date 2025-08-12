using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication;

public interface IAuthenticationService //cip...39
{
    Task<bool> AuthenticateAsync(LoginUserDto loginModel);
}
