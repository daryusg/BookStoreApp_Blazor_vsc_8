namespace BookStoreApp.Blazor.Server.UI.Models;

public class QueryParameters //cip...65 (+ chatgpt)
{
    private readonly int _maxPageSize;

    public QueryParameters(IConfiguration configuration)
    {
        _maxPageSize = configuration.GetValue<int>("Paging:PageSize", 50);
    }

    private int _pageSize = 10;
    public int StartIndex { get; set; } = 0;
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = (value > _maxPageSize) ? _maxPageSize : value; }
    }
}
