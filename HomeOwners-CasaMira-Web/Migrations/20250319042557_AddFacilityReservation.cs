using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeOwners_CasaMira_Web.Migrations
{
    /// <inheritdoc />
    public partial class AddFacilityReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FacilityReservations",
                table: "FacilityReservations");

            migrationBuilder.RenameTable(
                name: "FacilityReservations",
                newName: "FacilityReservation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FacilityReservation",
                table: "FacilityReservation",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FacilityReservation",
                table: "FacilityReservation");

            migrationBuilder.RenameTable(
                name: "FacilityReservation",
                newName: "FacilityReservations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FacilityReservations",
                table: "FacilityReservations",
                column: "Id");
        }
    }
}
