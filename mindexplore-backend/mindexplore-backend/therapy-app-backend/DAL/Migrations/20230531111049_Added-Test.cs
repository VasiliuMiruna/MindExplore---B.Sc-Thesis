using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace therapy_app_backend.Migrations
{
    public partial class AddedTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHungarian",
                table: "Therapists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMemberOfLGBT",
                table: "Therapists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotReligious",
                table: "Therapists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRRoma",
                table: "Therapists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OffersOnlineSessions",
                table: "Therapists",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Questions = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropColumn(
                name: "IsHungarian",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "IsMemberOfLGBT",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "IsNotReligious",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "IsRRoma",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "OffersOnlineSessions",
                table: "Therapists");
        }
    }
}
