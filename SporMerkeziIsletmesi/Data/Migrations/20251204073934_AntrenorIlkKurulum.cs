using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporMerkeziIsletmesi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AntrenorIlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Antrenorler",
                columns: table => new
                {
                    AntrenorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UzmanlikAlani = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Antrenorler", x => x.AntrenorID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Antrenorler");
        }
    }
}
