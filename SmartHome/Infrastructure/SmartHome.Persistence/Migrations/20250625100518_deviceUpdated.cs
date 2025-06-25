using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deviceUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CpuUsage",
                table: "Devices",
                type: "double precision",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EnergyConsumption",
                table: "Devices",
                type: "double precision",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HealthStatus",
                table: "Devices",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RamUsage",
                table: "Devices",
                type: "double precision",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Temperature",
                table: "Devices",
                type: "double precision",
                precision: 5,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CpuUsage",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "EnergyConsumption",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "HealthStatus",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "RamUsage",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "Devices");
        }
    }
}
