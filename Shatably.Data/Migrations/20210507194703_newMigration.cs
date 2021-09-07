using Microsoft.EntityFrameworkCore.Migrations;

namespace Shatably.Data.Migrations
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviewss_AspNetUsers_CompanyIDId",
                table: "Reviewss");

            migrationBuilder.RenameColumn(
                name: "CompanyIDId",
                table: "Reviewss",
                newName: "CompanyIDUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviewss_CompanyIDId",
                table: "Reviewss",
                newName: "IX_Reviewss_CompanyIDUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviewss_AspNetUsers_CompanyIDUserId",
                table: "Reviewss",
                column: "CompanyIDUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviewss_AspNetUsers_CompanyIDUserId",
                table: "Reviewss");

            migrationBuilder.RenameColumn(
                name: "CompanyIDUserId",
                table: "Reviewss",
                newName: "CompanyIDId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviewss_CompanyIDUserId",
                table: "Reviewss",
                newName: "IX_Reviewss_CompanyIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviewss_AspNetUsers_CompanyIDId",
                table: "Reviewss",
                column: "CompanyIDId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
