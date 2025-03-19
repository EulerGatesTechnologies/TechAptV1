using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechAptV1.Client.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedIsPrimeToBoolAndSetValueAsPrimaryKeyIndexed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Number",
                table: "Number");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Number");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Number",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrime",
                table: "Number",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Number",
                table: "Number",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_Number_Value",
                table: "Number",
                column: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Number",
                table: "Number");

            migrationBuilder.DropIndex(
                name: "IX_Number_Value",
                table: "Number");

            migrationBuilder.AlterColumn<int>(
                name: "IsPrime",
                table: "Number",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Number",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Number",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Number",
                table: "Number",
                column: "Id");
        }
    }
}
