using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManagement.DataAccess.Migrations
{
    public partial class AddingServiceToPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "service",
                table: "person",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_user_person_id",
                table: "user",
                column: "person_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_person_person_id",
                table: "user",
                column: "person_id",
                principalTable: "person",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_person_person_id",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_person_id",
                table: "user");

            migrationBuilder.DropColumn(
                name: "service",
                table: "person");
        }
    }
}
