namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

public class Response<T> //cip...44
{
    public string Message { get; set; } 
    public string ValidationErrors { get; set; }
    public bool Success { get; set; }
    public T Data { get; set; }
}
