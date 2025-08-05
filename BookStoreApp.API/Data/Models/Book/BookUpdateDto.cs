using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Data.Models.Book;

public class BookUpdateDto : BaseDto //cip...24
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; }

    [Range(1000, int.MaxValue)]
    public int Year { get; set; }

    [Required]
    public string Isbn { get; set; } = null!;

    [StringLength(250, MinimumLength = 10)]
    public string Summary { get; set; }

    public string Image { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }

    public int AuthorId { get; set; }
}
