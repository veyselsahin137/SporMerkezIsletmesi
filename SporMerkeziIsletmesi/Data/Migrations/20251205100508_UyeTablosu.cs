using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporMerkeziIsletmesi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UyeTablosu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uyeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: false),
                    Yas = table.Column<int>(type: "int", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Boy = table.Column<int>(type: "int", nullable: false),
                    Kilo = table.Column<int>(type: "int", nullable: false),
                    Hedef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AktiviteSeviyesi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YagOrani = table.Column<float>(type: "real", nullable: true),
                    TercihEdilenZaman = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uyeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uyeler_Salonlar_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salonlar",
                        principalColumn: "SalonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Uyeler_SalonId",
                table: "Uyeler",
                column: "SalonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Uyeler");
        }
    }
}
