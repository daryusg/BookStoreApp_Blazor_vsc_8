using System;

namespace BookStoreApp.API.Models;

public class VirtualiseResponse<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }

}
