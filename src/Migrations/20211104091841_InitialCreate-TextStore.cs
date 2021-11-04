using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Refor.Migrations
{
    public partial class InitialCreateTextStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "texts",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    uploaded = table.Column<Instant>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_texts", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_texts_id",
                table: "texts",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "texts");
        }
    }
}
