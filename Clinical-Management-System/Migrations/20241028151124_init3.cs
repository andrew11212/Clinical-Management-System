using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinical_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Prescriptions_PrescriptionId",
                table: "Documents");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Prescriptions_PrescriptionId",
                table: "Documents",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Prescriptions_PrescriptionId",
                table: "Documents");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Prescriptions_PrescriptionId",
                table: "Documents",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
