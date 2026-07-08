using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazeCareWebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_medicalRecords_AppointmentId",
                table: "medicalRecords");

            migrationBuilder.DropColumn(
                name: "AppotinmentId",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "City",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "medicalRecords");

            migrationBuilder.DropColumn(
                name: "RecordDate",
                table: "medicalRecords");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Symptoms",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "VisitType",
                table: "patients",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "PatientName",
                table: "patients",
                newName: "ContactNumber");

            migrationBuilder.RenameColumn(
                name: "Prescription",
                table: "medicalRecords",
                newName: "TreatmentPlan");

            migrationBuilder.RenameColumn(
                name: "MedicalTest",
                table: "medicalRecords",
                newName: "RecommendedTests");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Doctors",
                newName: "Speciality");

            migrationBuilder.RenameColumn(
                name: "AppointmentTime",
                table: "Appointments",
                newName: "PreferedTime");

            migrationBuilder.RenameColumn(
                name: "AppointmentStatus",
                table: "Appointments",
                newName: "SymptomsDescription");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Experience",
                table: "Doctors",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "NatureOfVisit",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    MedicineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescriptions_medicalRecords_RecordId",
                        column: x => x.RecordId,
                        principalTable: "medicalRecords",
                        principalColumn: "RecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_medicalRecords_AppointmentId",
                table: "medicalRecords",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_RecordId",
                table: "Prescriptions",
                column: "RecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_medicalRecords_AppointmentId",
                table: "medicalRecords");

            migrationBuilder.DropColumn(
                name: "NatureOfVisit",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "patients",
                newName: "VisitType");

            migrationBuilder.RenameColumn(
                name: "ContactNumber",
                table: "patients",
                newName: "PatientName");

            migrationBuilder.RenameColumn(
                name: "TreatmentPlan",
                table: "medicalRecords",
                newName: "Prescription");

            migrationBuilder.RenameColumn(
                name: "RecommendedTests",
                table: "medicalRecords",
                newName: "MedicalTest");

            migrationBuilder.RenameColumn(
                name: "Speciality",
                table: "Doctors",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "SymptomsDescription",
                table: "Appointments",
                newName: "AppointmentStatus");

            migrationBuilder.RenameColumn(
                name: "PreferedTime",
                table: "Appointments",
                newName: "AppointmentTime");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "patients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AppotinmentId",
                table: "patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "medicalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RecordDate",
                table: "medicalRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Experience",
                table: "Doctors",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Symptoms",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_medicalRecords_AppointmentId",
                table: "medicalRecords",
                column: "AppointmentId");
        }
    }
}
