using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadTrack.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserIdToSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sessions");
        }
    }
}
