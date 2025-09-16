using System;

namespace BookStoreApp.API.Models;

public class QueryParameters //cip...65 (+ chatgpt)
{
    private const int maxPageSize = 50;
    private int _pageSize = 10;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
    }

}
