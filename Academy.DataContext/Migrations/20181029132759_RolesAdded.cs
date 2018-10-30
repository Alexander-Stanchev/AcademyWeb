using Microsoft.EntityFrameworkCore.Migrations;

namespace Academy.DataContext.Migrations
{
    public partial class RolesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6df1168a-753c-40fd-9c57-7cb0d43e2081", "c492c133-cc36-4539-aa9f-f49abacf8a0c", "Administrator", null });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9444bbbf-8ee0-407d-b0d3-d4abe96634ed", "770e87ad-2902-40f9-9499-4e5a73217b6a", "Teacher", null });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a0dbb330-a55f-46a0-821d-c5adb47e8d30", "f1ce3bfd-3e00-4e21-9fe6-a966e1bb7bd8", "Student", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRole");
        }
    }
}
