using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2f0072d6-e6d9-41d8-8a37-9f6c8f87a772", null, "Administrator", "ADMINISTRATOR" },
                    { "834fd74e-3512-4562-9eef-f274041a9530", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "191e98c9-b6c2-43b9-9c30-0ee3319a8236", 0, "de454f13-6429-4953-887b-18a4b1c4227b", "user2@bookstore.com", false, "User2", "Base", false, null, "USER2@BOOKSTORE.COM", "USER2@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEOUmpjwSEWwd4xLQw814GF73qQEVUiLXdTeZ5pFTXFnY1MxX1ymfAK/BvLc+08fXvA==", null, false, "d3df30d4-24ec-4b13-a469-246b30909b30", false, "user2@bookstore.com" },
                    { "2e29df7d-cb8c-4e6e-8f7c-29a667cb9dd9", 0, "c27af2cd-47cf-4201-83d6-1e36d2ced9bf", "admin1@bookstore.com", false, "Admin1", "Base", false, null, "ADMIN1@BOOKSTORE.COM", "ADMIN1@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEFKNSBCkIgvq3LeZf83GBHhHR1i2lgSJfCCHcwYgAl1GROBHI1NEuZPLlWk97u5wRw==", null, false, "2488e615-820a-4cc2-92b8-94ad1a987df9", false, "admin1@bookstore.com" },
                    { "edf469f6-78dd-4ad1-bb28-9512037a1a49", 0, "dfebaedc-38f9-4693-bae3-2770fff756d8", "user1@bookstore.com", false, "User1", "Base", false, null, "USER1@BOOKSTORE.COM", "USER1@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEMH7MEuLRK+QNFn6NQMzt0rT/f37jc+RgbGYPTDksQVMnctXYbQItqs6IWLyG15emQ==", null, false, "5e374d97-283d-4415-b623-3a7f46bd20fa", false, "user1@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "834fd74e-3512-4562-9eef-f274041a9530", "191e98c9-b6c2-43b9-9c30-0ee3319a8236" },
                    { "2f0072d6-e6d9-41d8-8a37-9f6c8f87a772", "2e29df7d-cb8c-4e6e-8f7c-29a667cb9dd9" },
                    { "834fd74e-3512-4562-9eef-f274041a9530", "edf469f6-78dd-4ad1-bb28-9512037a1a49" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "834fd74e-3512-4562-9eef-f274041a9530", "191e98c9-b6c2-43b9-9c30-0ee3319a8236" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2f0072d6-e6d9-41d8-8a37-9f6c8f87a772", "2e29df7d-cb8c-4e6e-8f7c-29a667cb9dd9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "834fd74e-3512-4562-9eef-f274041a9530", "edf469f6-78dd-4ad1-bb28-9512037a1a49" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f0072d6-e6d9-41d8-8a37-9f6c8f87a772");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "834fd74e-3512-4562-9eef-f274041a9530");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "191e98c9-b6c2-43b9-9c30-0ee3319a8236");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e29df7d-cb8c-4e6e-8f7c-29a667cb9dd9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "edf469f6-78dd-4ad1-bb28-9512037a1a49");
        }
    }
}
