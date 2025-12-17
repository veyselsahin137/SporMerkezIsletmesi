using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporMerkeziIsletmesi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRandevuModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Uyeler_UyeId",
                table: "Randevular");

            migrationBuilder.AddColumn<int>(
                name: "AntrenorID",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "BaslangicSaati",
                table: "Randevular",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "BitisSaati",
                table: "Randevular",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "Durum",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HizmetID",
                table: "Randevular",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OlusturmaTarihi",
                table: "Randevular",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "Randevular",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UyeId1",
                table: "Randevular",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Uyeler_IdentityUserId",
                table: "Uyeler",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_AntrenorID",
                table: "Randevular",
                column: "AntrenorID");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_HizmetID",
                table: "Randevular",
                column: "HizmetID");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_UyeId1",
                table: "Randevular",
                column: "UyeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Antrenorler_AntrenorID",
                table: "Randevular",
                column: "AntrenorID",
                principalTable: "Antrenorler",
                principalColumn: "AntrenorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Hizmetler_HizmetID",
                table: "Randevular",
                column: "HizmetID",
                principalTable: "Hizmetler",
                principalColumn: "HizmetID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Uyeler_UyeId",
                table: "Randevular",
                column: "UyeId",
                principalTable: "Uyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Uyeler_UyeId1",
                table: "Randevular",
                column: "UyeId1",
                principalTable: "Uyeler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Uyeler_AspNetUsers_IdentityUserId",
                table: "Uyeler",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Antrenorler_AntrenorID",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Hizmetler_HizmetID",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Uyeler_UyeId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Uyeler_UyeId1",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Uyeler_AspNetUsers_IdentityUserId",
                table: "Uyeler");

            migrationBuilder.DropIndex(
                name: "IX_Uyeler_IdentityUserId",
                table: "Uyeler");

            migrationBuilder.DropIndex(
                name: "IX_Randevular_AntrenorID",
                table: "Randevular");

            migrationBuilder.DropIndex(
                name: "IX_Randevular_HizmetID",
                table: "Randevular");

            migrationBuilder.DropIndex(
                name: "IX_Randevular_UyeId1",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "AntrenorID",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "BaslangicSaati",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "BitisSaati",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "HizmetID",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "OlusturmaTarihi",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "UyeId1",
                table: "Randevular");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Uyeler_UyeId",
                table: "Randevular",
                column: "UyeId",
                principalTable: "Uyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
