namespace BookStoreApp.Blazor.WebAssembly.UI.Models;

public class QueryParameters //cip...65
{
    private int _pageSize = 15;
    public int StartIndex { get; set; }
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value; }
    }
}