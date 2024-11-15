using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEML.Data.Migrations
{
    /// <inheritdoc />
    public partial class Feelings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feelings",
                columns: table => new
                {
                    FeelingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeelingName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeelingDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeelingSeverity = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feelings", x => x.FeelingId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feelings");
        }
    }
}
