using Blazored.LocalStorage; //cip...39
using BookStoreApp.Blazor.Server.UI.Configurations; //cip...46
using BookStoreApp.Blazor.Server.UI.Data;
using BookStoreApp.Blazor.Server.UI.Providers;
using BookStoreApp.Blazor.Server.UI.Services;
using BookStoreApp.Blazor.Server.UI.Services.Authentication; //cip...39
using BookStoreApp.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage(); //cip...39
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddHttpClient<IClient, Client>(client =>
{
    // TODO: Replace with config-based base URL
    client.BaseAddress = new Uri("https://localhost:7235");
}); //cip...37
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(); //cip...39
builder.Services.AddScoped<IAuthorService, AuthorService>(); //cip...44
builder.Services.AddAutoMapper(typeof(MapperConfig)); //cip...46 the dependency injection package allows me to add automapper here

builder.Services.AddScoped<ApiAuthenticationStateProvider>(); //cip...40
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<ApiAuthenticationStateProvider>()); //cip...39

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
