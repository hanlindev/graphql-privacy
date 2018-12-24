using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphQL.Privacy.Sample.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true),
                    IsHidden = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1L, "User1" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2L, "User2" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3L, "User3" });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "IsHidden", "Title", "UserId" },
                values: new object[] { 1L, false, "Album 1", 1L });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "IsHidden", "Title", "UserId" },
                values: new object[] { 2L, false, "Album 2", 1L });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "IsHidden", "Title", "UserId" },
                values: new object[] { 3L, false, "Album 3", 1L });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "IsHidden", "Title", "UserId" },
                values: new object[] { 4L, false, "Album 4", 2L });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "IsHidden", "Title", "UserId" },
                values: new object[] { 5L, true, "Album 5", 2L });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "IsHidden", "Title", "UserId" },
                values: new object[] { 6L, false, "Album 6", 2L });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_UserId",
                table: "Albums",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
