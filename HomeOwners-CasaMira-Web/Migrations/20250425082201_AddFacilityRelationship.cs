using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeOwners_CasaMira_Web.Migrations
{
    /// <inheritdoc />
    public partial class AddFacilityRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FacilityReservations_FacilityId",
                table: "FacilityReservations",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityReservations_Facilities_FacilityId",
                table: "FacilityReservations",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityReservations_Facilities_FacilityId",
                table: "FacilityReservations");

            migrationBuilder.DropIndex(
                name: "IX_FacilityReservations_FacilityId",
                table: "FacilityReservations");
        }
    }
}
