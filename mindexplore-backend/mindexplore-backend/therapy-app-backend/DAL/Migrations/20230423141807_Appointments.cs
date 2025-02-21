using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace therapy_app_backend.Migrations
{
    public partial class Appointments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patients_PatientId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Therapists_TherapistId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Patients_PatientId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Therapists_TherapistId",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Match",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.RenameTable(
                name: "Match",
                newName: "Matches");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_Match_TherapistId",
                table: "Matches",
                newName: "IX_Matches_TherapistId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_PatientId",
                table: "Matches",
                newName: "IX_Matches_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_TherapistId",
                table: "Appointments",
                newName: "IX_Appointments_TherapistId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointments",
                newName: "IX_Appointments_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Therapists_TherapistId",
                table: "Appointments",
                column: "TherapistId",
                principalTable: "Therapists",
                principalColumn: "TherapistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Patients_PatientId",
                table: "Matches",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Therapists_TherapistId",
                table: "Matches",
                column: "TherapistId",
                principalTable: "Therapists",
                principalColumn: "TherapistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Therapists_TherapistId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Patients_PatientId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Therapists_TherapistId",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Match");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointment");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_TherapistId",
                table: "Match",
                newName: "IX_Match_TherapistId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_PatientId",
                table: "Match",
                newName: "IX_Match_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_TherapistId",
                table: "Appointment",
                newName: "IX_Appointment_TherapistId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointment",
                newName: "IX_Appointment_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Match",
                table: "Match",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patients_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Therapists_TherapistId",
                table: "Appointment",
                column: "TherapistId",
                principalTable: "Therapists",
                principalColumn: "TherapistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Patients_PatientId",
                table: "Match",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Therapists_TherapistId",
                table: "Match",
                column: "TherapistId",
                principalTable: "Therapists",
                principalColumn: "TherapistId");
        }
    }
}
