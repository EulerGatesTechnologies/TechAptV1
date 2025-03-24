using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechAptV1.Client.Migrations
{
    /// <inheritdoc />
    public partial class AddPkIdToNumbers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Number",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Number");
        }
    }
}
