namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

public partial class Client : IClient //cip...44
{
    public HttpClient HttpClient
    {
        get => _httpClient;
    }
}