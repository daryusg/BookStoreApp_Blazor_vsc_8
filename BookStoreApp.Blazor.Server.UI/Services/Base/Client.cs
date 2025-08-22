namespace BookStoreApp.Blazor.Server.UI.Services.Base;

public partial class Client : IClient //cip...44
{
    public HttpClient HttpClient
    {
        get => _httpClient;
    }
}