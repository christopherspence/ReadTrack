using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadTrack.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTimeToDurationAndAddedDateToSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Sessions",
                newName: "Duration");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Sessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Sessions",
                newName: "Time");
        }
    }
}
