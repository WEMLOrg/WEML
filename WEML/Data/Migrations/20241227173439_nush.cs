using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEML.Data.Migrations
{
    /// <inheritdoc />
    public partial class nush : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactDoctorEmail",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FeelingUsers",
                columns: table => new
                {
                    FeelingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeelingUsers", x => new { x.FeelingId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FeelingUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeelingUsers_Feelings_FeelingId",
                        column: x => x.FeelingId,
                        principalTable: "Feelings",
                        principalColumn: "FeelingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SymptomUsers",
                columns: table => new
                {
                    SymptomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomUsers", x => new { x.SymptomId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SymptomUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SymptomUsers_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "SymptomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeelingUsers_UserId",
                table: "FeelingUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomUsers_UserId",
                table: "SymptomUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeelingUsers");

            migrationBuilder.DropTable(
                name: "SymptomUsers");

            migrationBuilder.DropColumn(
                name: "ContactDoctorEmail",
                table: "AspNetUsers");
        }
    }
}
