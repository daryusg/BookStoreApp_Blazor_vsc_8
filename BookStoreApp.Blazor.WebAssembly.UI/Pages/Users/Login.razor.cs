using BookStoreApp.Blazor.WebAssembly.UI.Services.Authentication;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.WebAssembly.UI.Pages.Users;

public partial class Login
{
    [Inject] public IAuthenticationService authService { get; set; }
    [Inject] public NavigationManager navManager { get; set; }

    LoginUserDto LoginModel = new();
    string message = string.Empty;

    public async Task HandleLoginAsync()
    {
        // @* try
        // { *@
        var response = await authService.AuthenticateAsync(LoginModel);
        //if (response)
        if (response.Success) //cip...67
        {
            // Handle successful login, e.g., redirect to home page
            navManager.NavigateTo("/");
        }
        // Handle login failure, e.g., display error message
        message = response.Message; //cip...67
        // @* }
        // catch (ApiException ex)
        // {
        //     if (ex.StatusCode >= 200 && ex.StatusCode <= 299)
        //     {
        //         // Registration successful, redirect home page
        //         navManager.NavigateTo("/users/login");
        //         //return;
        //     }
        //     message = ex.Message;
        // } *@
    }
}
