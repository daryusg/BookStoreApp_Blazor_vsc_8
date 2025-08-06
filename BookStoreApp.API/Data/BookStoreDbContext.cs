using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookStoreApp.API.Data;

//public partial class BookStoreDbContext : DbContext
public partial class BookStoreDbContext : IdentityDbContext<ApiUser> //cip...27,28
{
    public BookStoreDbContext()
    {
    }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    /*
    cip...14 removed as set up/configured in Program.cs
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Data Source=localhost,1449;Initial Catalog=BookStoreDb;Encrypt=False;User ID=sa;Password=Str0ngPa$$w0rd;");
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); //cip...27

        //cip...28. chatgpt fix/hack for inclusion in migration. 👇 This tells EF to include FirstName and LastName in the AspNetUsers table
        modelBuilder.Entity<ApiUser>(b =>
        {
            b.Property(u => u.FirstName)
                .HasMaxLength(50); // Optional: apply constraints

            b.Property(u => u.LastName)
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.Bio).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasIndex(e => e.Isbn, "UQ_Books_ISBN").IsUnique();

            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Summary).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Authors_Id");
        });

        modelBuilder.Entity<IdentityRole>().HasData( //cip...29
            new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                Id = "2f0072d6-e6d9-41d8-8a37-9f6c8f87a772"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER",
                Id = "834fd74e-3512-4562-9eef-f274041a9530"
            }
        );

        var hasher = new PasswordHasher<ApiUser>(); //cip...29
        // Seed initial users
        modelBuilder.Entity<ApiUser>().HasData( //cip...29
            new ApiUser
            {
                Id = "edf469f6-78dd-4ad1-bb28-9512037a1a49",
                Email = "user1@bookstore.com",
                NormalizedEmail = "USER1@BOOKSTORE.COM",
                UserName = "user1@bookstore.com",
                NormalizedUserName = "USER1@BOOKSTORE.COM",
                FirstName = "User1",
                LastName = "Base",
                PasswordHash = "AQAAAAIAAYagAAAAEMH7MEuLRK+QNFn6NQMzt0rT/f37jc+RgbGYPTDksQVMnctXYbQItqs6IWLyG15emQ==", /*.net9+ issue. copied from migration. hasher.HashPassword(null, "P@ssw0rd1"),*/
                ConcurrencyStamp = "dfebaedc-38f9-4693-bae3-2770fff756d8", /*.net9+ issue. copied from migration*/
                SecurityStamp = "5e374d97-283d-4415-b623-3a7f46bd20fa" /*.net9+ issue. copied from migration*/
            },
            new ApiUser
            {
                Id = "191e98c9-b6c2-43b9-9c30-0ee3319a8236",
                Email = "user2@bookstore.com",
                NormalizedEmail = "USER2@BOOKSTORE.COM",
                UserName = "user2@bookstore.com",
                NormalizedUserName = "USER2@BOOKSTORE.COM",
                FirstName = "User2",
                LastName = "Base",
                PasswordHash = "AQAAAAIAAYagAAAAEOUmpjwSEWwd4xLQw814GF73qQEVUiLXdTeZ5pFTXFnY1MxX1ymfAK/BvLc+08fXvA==", /*.net9+ issue. copied from migration. hasher.HashPassword(null, "P@ssw0rd1"),*/
                ConcurrencyStamp = "de454f13-6429-4953-887b-18a4b1c4227b", /*.net9+ issue. copied from migration*/
                SecurityStamp = "d3df30d4-24ec-4b13-a469-246b30909b30" /*.net9+ issue. copied from migration*/
            }
            ,
            new ApiUser
            {
                Id = "2e29df7d-cb8c-4e6e-8f7c-29a667cb9dd9",
                Email = "admin1@bookstore.com",
                NormalizedEmail = "ADMIN1@BOOKSTORE.COM",
                UserName = "admin1@bookstore.com",
                NormalizedUserName = "ADMIN1@BOOKSTORE.COM",
                FirstName = "Admin1",
                LastName = "Base",
                PasswordHash = "AQAAAAIAAYagAAAAEFKNSBCkIgvq3LeZf83GBHhHR1i2lgSJfCCHcwYgAl1GROBHI1NEuZPLlWk97u5wRw==", /*.net9 issue. copied from migration. hasher.HashPassword(null, "P@ssw0rd1"),*/
                ConcurrencyStamp = "c27af2cd-47cf-4201-83d6-1e36d2ced9bf", /*.net9+ issue. copied from migration*/
                SecurityStamp = "2488e615-820a-4cc2-92b8-94ad1a987df9" /*.net9+ issue. copied from migration*/
            }
        );

        modelBuilder.Entity<IdentityUserRole<string>>().HasData( //cip...29
            new IdentityUserRole<string>
            {
                RoleId = "834fd74e-3512-4562-9eef-f274041a9530", // User role
                UserId = "edf469f6-78dd-4ad1-bb28-9512037a1a49" // user1
            },
            new IdentityUserRole<string>
            {
                RoleId = "834fd74e-3512-4562-9eef-f274041a9530", // User role
                UserId = "191e98c9-b6c2-43b9-9c30-0ee3319a8236" // user2
            },
            new IdentityUserRole<string>
            {
                RoleId = "2f0072d6-e6d9-41d8-8a37-9f6c8f87a772", // Administrator role
                UserId = "2e29df7d-cb8c-4e6e-8f7c-29a667cb9dd9" // admin1
            }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    public class BookStoreDbContextFactory : IDesignTimeDbContextFactory<BookStoreDbContext> //chatgpt. for controller scaffolding in cip...18
    {
        public BookStoreDbContext CreateDbContext(string[] args)
        {
            // This is how it finds your appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BookStoreDbContext>();
            var connectionString = configuration.GetConnectionString("BookStoreDbConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new BookStoreDbContext(optionsBuilder.Options);
        }
    }
}
