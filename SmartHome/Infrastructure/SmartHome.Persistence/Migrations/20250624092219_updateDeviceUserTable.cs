using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateDeviceUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "IsDeleted",
                table: "DeviceUsers",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DeviceUsers");
        }
    }
}
