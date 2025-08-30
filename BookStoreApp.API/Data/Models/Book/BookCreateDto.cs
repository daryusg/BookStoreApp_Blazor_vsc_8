using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Data.Models.Book;

public class BookCreateDto //cip...24
{
    [Required]
    public int AuthorId { get; set; }

    [Required]
    [StringLength(50)]
    public string Title { get; set; }

    [Required]
    [Range(1000, int.MaxValue)]
    public int Year { get; set; }

    [Required]
    public string Isbn { get; set; } = null!;

    [StringLength(250, MinimumLength = 10)]
    public string Summary { get; set; }

    public string? ImageData { get; set; } //cip...55. rename (note:regen nswag)
    public string? OriginalImageName { get; set; } //cip...55. (note:regen nswag)

    [Required]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }
}
