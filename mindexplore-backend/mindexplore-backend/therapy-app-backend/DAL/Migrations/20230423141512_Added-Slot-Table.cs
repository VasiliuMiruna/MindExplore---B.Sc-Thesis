using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace therapy_app_backend.Migrations
{
    public partial class AddedSlotTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTheraists_Patients_PatientId",
                table: "PatientTheraists");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientTheraists_Therapists_TherapistId",
                table: "PatientTheraists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientTheraists",
                table: "PatientTheraists");

            migrationBuilder.RenameTable(
                name: "PatientTheraists",
                newName: "PatientTherapists");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTheraists_TherapistId",
                table: "PatientTherapists",
                newName: "IX_PatientTherapists_TherapistId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTheraists_PatientId",
                table: "PatientTherapists",
                newName: "IX_PatientTherapists_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientTherapists",
                table: "PatientTherapists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTherapists_Patients_PatientId",
                table: "PatientTherapists",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTherapists_Therapists_TherapistId",
                table: "PatientTherapists",
                column: "TherapistId",
                principalTable: "Therapists",
                principalColumn: "TherapistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTherapists_Patients_PatientId",
                table: "PatientTherapists");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientTherapists_Therapists_TherapistId",
                table: "PatientTherapists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientTherapists",
                table: "PatientTherapists");

            migrationBuilder.RenameTable(
                name: "PatientTherapists",
                newName: "PatientTheraists");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTherapists_TherapistId",
                table: "PatientTheraists",
                newName: "IX_PatientTheraists_TherapistId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTherapists_PatientId",
                table: "PatientTheraists",
                newName: "IX_PatientTheraists_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientTheraists",
                table: "PatientTheraists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTheraists_Patients_PatientId",
                table: "PatientTheraists",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTheraists_Therapists_TherapistId",
                table: "PatientTheraists",
                column: "TherapistId",
                principalTable: "Therapists",
                principalColumn: "TherapistId");
        }
    }
}
