using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace therapy_app_backend.Migrations
{
    public partial class UpdateTherapistTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Therapists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoHoursPractice",
                table: "Therapists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Quote",
                table: "Therapists",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "NoHoursPractice",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "Quote",
                table: "Therapists");
        }
    }
}
