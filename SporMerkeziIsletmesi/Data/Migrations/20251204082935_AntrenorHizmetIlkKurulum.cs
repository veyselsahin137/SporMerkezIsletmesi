using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporMerkeziIsletmesi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AntrenorHizmetIlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AntrenorHizmetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AntrenorID = table.Column<int>(type: "int", nullable: false),
                    HizmetID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntrenorHizmetler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntrenorHizmetler_Antrenorler_AntrenorID",
                        column: x => x.AntrenorID,
                        principalTable: "Antrenorler",
                        principalColumn: "AntrenorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AntrenorHizmetler_Hizmetler_HizmetID",
                        column: x => x.HizmetID,
                        principalTable: "Hizmetler",
                        principalColumn: "HizmetID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntrenorHizmetler_AntrenorID",
                table: "AntrenorHizmetler",
                column: "AntrenorID");

            migrationBuilder.CreateIndex(
                name: "IX_AntrenorHizmetler_HizmetID",
                table: "AntrenorHizmetler",
                column: "HizmetID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntrenorHizmetler");
        }
    }
}
