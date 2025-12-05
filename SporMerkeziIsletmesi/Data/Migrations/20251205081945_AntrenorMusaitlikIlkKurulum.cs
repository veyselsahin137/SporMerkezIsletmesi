using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporMerkeziIsletmesi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AntrenorMusaitlikIlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AntrenorMusaitlikler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AntrenorID = table.Column<int>(type: "int", nullable: false),
                    Gun = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BaslangicSaati = table.Column<TimeSpan>(type: "time", nullable: false),
                    BitisSaati = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntrenorMusaitlikler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntrenorMusaitlikler_Antrenorler_AntrenorID",
                        column: x => x.AntrenorID,
                        principalTable: "Antrenorler",
                        principalColumn: "AntrenorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntrenorMusaitlikler_AntrenorID",
                table: "AntrenorMusaitlikler",
                column: "AntrenorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntrenorMusaitlikler");
        }
    }
}
