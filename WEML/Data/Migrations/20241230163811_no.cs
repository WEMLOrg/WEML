using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEML.Data.Migrations
{
    /// <inheritdoc />
    public partial class no : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactPersonPhone",
                table: "AspNetUsers",
                newName: "ContactPersonEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactPersonEmail",
                table: "AspNetUsers",
                newName: "ContactPersonPhone");
        }
    }
}
