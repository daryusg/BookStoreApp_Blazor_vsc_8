using BookStoreApp.API.Data.Models.Book;

namespace BookStoreApp.API.Data.Models.Author;

public class AuthorDetailsDto : AuthorReadOnlyDto //cip...47
{
    public List<BookReadOnlyDto> Books { get; set; }
}
