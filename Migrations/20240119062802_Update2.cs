using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kindergarten.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Teachers_TeacherID",
                table: "Classes");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherID",
                table: "Classes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ClassName",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Teachers_TeacherID",
                table: "Classes",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Teachers_TeacherID",
                table: "Classes");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherID",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClassName",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Teachers_TeacherID",
                table: "Classes",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
