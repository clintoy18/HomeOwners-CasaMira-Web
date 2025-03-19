using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeOwners_CasaMira_Web.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToFacility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Facilities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Facilities");
        }
    }
}
