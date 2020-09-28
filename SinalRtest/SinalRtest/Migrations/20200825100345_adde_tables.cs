using Microsoft.EntityFrameworkCore.Migrations;

namespace SinalRtest.Migrations
{
    public partial class adde_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Catergory",
                table: "TMS");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "TMS",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "TMS");

            migrationBuilder.AddColumn<string>(
                name: "Catergory",
                table: "TMS",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
