using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;
using BookStoreApp.Blazor.WebAssembly.UI.Services;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Authentication; //cip...39
using BookStoreApp.Blazor.WebAssembly.UI.Configurations;
using BookStoreApp.Blazor.WebAssembly.UI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseAddress = "https://localhost:7235"; //cip...73
if (builder.HostEnvironment.IsProduction())
{
    baseAddress = "https://bookstoreappkevapi.azurewebsites.net"; //cip...73
}
builder.Services.AddScoped(sp => new HttpClient{ BaseAddress = new Uri(baseAddress) }); //cip...61

builder.Services.AddBlazoredLocalStorage(); //cip...39

builder.Services.AddScoped<ApiAuthenticationStateProvider>(); //cip...40
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<ApiAuthenticationStateProvider>()); //cip...39

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IClient, Client>(); //cip...61

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(); //cip...39
builder.Services.AddScoped<IAuthorService, AuthorService>(); //cip...44
builder.Services.AddScoped<IBookService, BookService>(); //cip...52
builder.Services.AddAutoMapper(typeof(MapperConfig)); //cip...46 the dependency injection package allows me to add automapper here

await builder.Build().RunAsync();
