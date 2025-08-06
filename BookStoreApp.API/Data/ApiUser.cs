using Microsoft.AspNetCore.Identity;

namespace BookStoreApp.API.Data;

public class ApiUser : IdentityUser //cip...28
{
    /*this wasn't being picked up in the migration so (chatgpt) i had to use the OnModelCreating method in BookStoreDbContext.cs:
        modelBuilder.Entity<ApiUser>(b =>
        {
            b.Property(u => u.FirstName)
                .HasMaxLength(50); // Optional: apply constraints

            b.Property(u => u.LastName)
                .HasMaxLength(50);
        });*/
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
