namespace BookStoreApp.API.Data.Models.Author;

public class AuthorReadOnlyDto : BaseDto //cip...19
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Bio { get; set; }
}
