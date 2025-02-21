using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace therapy_app_backend.Migrations
{
    public partial class UpdatedTherapistAndAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoOfRatings",
                table: "Therapists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfRatings",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Appointments");
        }
    }
}
