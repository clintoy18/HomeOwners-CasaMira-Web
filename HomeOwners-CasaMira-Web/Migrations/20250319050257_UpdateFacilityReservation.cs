using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeOwners_CasaMira_Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFacilityReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FacilityName",
                table: "FacilityReservation",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FacilityReservation",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "FacilityReservation",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "FacilityId",
                table: "FacilityReservation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "FacilityReservation",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilityReservation_FacilityId",
                table: "FacilityReservation",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityReservation_UserId",
                table: "FacilityReservation",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityReservation_AspNetUsers_UserId",
                table: "FacilityReservation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityReservation_Facility_FacilityId",
                table: "FacilityReservation",
                column: "FacilityId",
                principalTable: "Facility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityReservation_AspNetUsers_UserId",
                table: "FacilityReservation");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityReservation_Facility_FacilityId",
                table: "FacilityReservation");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropIndex(
                name: "IX_FacilityReservation_FacilityId",
                table: "FacilityReservation");

            migrationBuilder.DropIndex(
                name: "IX_FacilityReservation_UserId",
                table: "FacilityReservation");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "FacilityReservation");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "FacilityReservation");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "FacilityReservation");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "FacilityReservation",
                newName: "FacilityName");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FacilityReservation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
