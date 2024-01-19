using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kindergarten.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KActivities",
                columns: table => new
                {
                    KActivityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KActivityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KActivities", x => x.KActivityID);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    TeacherID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.TeacherID);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherID = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassID);
                    table.ForeignKey(
                        name: "FK_Classes_Teachers_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "TeacherID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Children",
                columns: table => new
                {
                    ChildID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.ChildID);
                    table.ForeignKey(
                        name: "FK_Children_Classes_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ClassID");
                });

            migrationBuilder.CreateTable(
                name: "ClassesActivities",
                columns: table => new
                {
                    ClassID = table.Column<int>(type: "int", nullable: false),
                    KActivityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassesActivities", x => new { x.ClassID, x.KActivityID });
                    table.ForeignKey(
                        name: "FK_ClassesActivities_Classes_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ClassID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassesActivities_KActivities_KActivityID",
                        column: x => x.KActivityID,
                        principalTable: "KActivities",
                        principalColumn: "KActivityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PresentStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_Attendances_Children_ChildID",
                        column: x => x.ChildID,
                        principalTable: "Children",
                        principalColumn: "ChildID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ChildID",
                table: "Attendances",
                column: "ChildID");

            migrationBuilder.CreateIndex(
                name: "IX_Children_ClassID",
                table: "Children",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherID",
                table: "Classes",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_ClassesActivities_KActivityID",
                table: "ClassesActivities",
                column: "KActivityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "ClassesActivities");

            migrationBuilder.DropTable(
                name: "Children");

            migrationBuilder.DropTable(
                name: "KActivities");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
