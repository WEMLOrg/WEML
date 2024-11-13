using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEML.Data.Migrations
{
    /// <inheritdoc />
    public partial class aaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Challanges",
                columns: table => new
                {
                    cId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maxScore = table.Column<int>(type: "int", nullable: false),
                    minScoreToPass = table.Column<int>(type: "int", nullable: false),
                    objective = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challanges", x => x.cId);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    qId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correctAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    incorrectAnswer1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    incorrectAnswer2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    incorrectAnswer3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.qId);
                });

            migrationBuilder.CreateTable(
                name: "ChallangeQuestions",
                columns: table => new
                {
                    cId = table.Column<int>(type: "int", nullable: false),
                    questionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallangeQuestions", x => new { x.cId, x.questionId });
                    table.ForeignKey(
                        name: "FK_ChallangeQuestions_Challanges_cId",
                        column: x => x.cId,
                        principalTable: "Challanges",
                        principalColumn: "cId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallangeQuestions_Questions_questionId",
                        column: x => x.questionId,
                        principalTable: "Questions",
                        principalColumn: "qId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallangeQuestions_questionId",
                table: "ChallangeQuestions",
                column: "questionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallangeQuestions");

            migrationBuilder.DropTable(
                name: "Challanges");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
