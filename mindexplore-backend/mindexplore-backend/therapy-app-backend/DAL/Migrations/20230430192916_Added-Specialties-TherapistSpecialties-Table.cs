using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace therapy_app_backend.Migrations
{
    public partial class AddedSpecialtiesTherapistSpecialtiesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TherapistSpecialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TherapistId = table.Column<int>(type: "int", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TherapistSpecialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TherapistSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TherapistSpecialties_Therapists_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapists",
                        principalColumn: "TherapistId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TherapistSpecialties_SpecialtyId",
                table: "TherapistSpecialties",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistSpecialties_TherapistId",
                table: "TherapistSpecialties",
                column: "TherapistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TherapistSpecialties");

            migrationBuilder.DropTable(
                name: "Specialties");
        }
    }
}
