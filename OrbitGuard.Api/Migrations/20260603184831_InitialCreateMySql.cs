using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrbitGuard.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateMySql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "monitored_areas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    location = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    latitude = table.Column<decimal>(type: "decimal(10,6)", precision: 10, scale: 6, nullable: false),
                    longitude = table.Column<decimal>(type: "decimal(10,6)", precision: 10, scale: 6, nullable: false),
                    risk_level = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monitored_areas", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "satellite_readings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    monitored_area_id = table.Column<int>(type: "int", nullable: false),
                    temperature = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    humidity = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    rainfall = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    vegetation_index = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    captured_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_satellite_readings", x => x.id);
                    table.ForeignKey(
                        name: "FK_satellite_readings_monitored_areas_monitored_area_id",
                        column: x => x.monitored_area_id,
                        principalTable: "monitored_areas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "climate_alerts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    monitored_area_id = table.Column<int>(type: "int", nullable: false),
                    satellite_reading_id = table.Column<int>(type: "int", nullable: true),
                    alert_type = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    severity = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_climate_alerts", x => x.id);
                    table.ForeignKey(
                        name: "FK_climate_alerts_monitored_areas_monitored_area_id",
                        column: x => x.monitored_area_id,
                        principalTable: "monitored_areas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_climate_alerts_satellite_readings_satellite_reading_id",
                        column: x => x.satellite_reading_id,
                        principalTable: "satellite_readings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_climate_alerts_monitored_area_id",
                table: "climate_alerts",
                column: "monitored_area_id");

            migrationBuilder.CreateIndex(
                name: "IX_climate_alerts_satellite_reading_id",
                table: "climate_alerts",
                column: "satellite_reading_id");

            migrationBuilder.CreateIndex(
                name: "IX_satellite_readings_monitored_area_id",
                table: "satellite_readings",
                column: "monitored_area_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "climate_alerts");

            migrationBuilder.DropTable(
                name: "satellite_readings");

            migrationBuilder.DropTable(
                name: "monitored_areas");
        }
    }
}
