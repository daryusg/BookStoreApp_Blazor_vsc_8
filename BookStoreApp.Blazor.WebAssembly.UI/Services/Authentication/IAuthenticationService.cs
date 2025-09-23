using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Authentication;

public interface IAuthenticationService //cip...39
{
    //Task<bool> AuthenticateAsync(LoginUserDto loginModel);
    Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel); //cip...67
    Task LogoutAsync();
}