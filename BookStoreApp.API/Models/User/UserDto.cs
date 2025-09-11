using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.User;

public class UserDto : LoginUserDto //cip...30
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Role { get; set; }
}
