using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechAptV1.Client.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Number",
                columns: table => new
                {
                    Value = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsPrime = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Number", x => x.Value);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Number_Value",
                table: "Number",
                column: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Number");
        }
    }
}
