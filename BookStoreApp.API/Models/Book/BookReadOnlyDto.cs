namespace BookStoreApp.API.Models.Book;

public class BookReadOnlyDto : BaseDto //cip...24
{
    public string Title { get; set; }

    public string Image { get; set; }

    public decimal Price { get; set; }

    public int AuthorId { get; set; }

    public string AuthorName { get; set; }
}
