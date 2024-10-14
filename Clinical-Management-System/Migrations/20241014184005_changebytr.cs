using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinical_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class changebytr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Documents",
                type: "varbinary(max)",
                nullable:true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
