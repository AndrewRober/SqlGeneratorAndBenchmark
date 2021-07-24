using Microsoft.EntityFrameworkCore.Migrations;

namespace SqlGeneratorAndBenchmark.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeNormalizedObjs",
                columns: table => new
                {
                    DeNormalizedObjId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentObjName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildObjName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildObjName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildObjName3 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeNormalizedObjs", x => x.DeNormalizedObjId);
                });

            migrationBuilder.CreateTable(
                name: "ParentObjs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentObjName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentObjs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChildObjs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentObjId = table.Column<int>(type: "int", nullable: false),
                    ChildObjName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildObjs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildObjs_ParentObjs_ParentObjId",
                        column: x => x.ParentObjId,
                        principalTable: "ParentObjs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildObjs_ParentObjId",
                table: "ChildObjs",
                column: "ParentObjId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildObjs");

            migrationBuilder.DropTable(
                name: "DeNormalizedObjs");

            migrationBuilder.DropTable(
                name: "ParentObjs");
        }
    }
}
