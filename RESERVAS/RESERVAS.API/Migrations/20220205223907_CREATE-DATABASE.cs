using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESERVAS.API.Migrations
{
    public partial class CREATEDATABASE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HOTELES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PAIS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LATITUD = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LONGITUD = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DESCRIPCION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACTIVO = table.Column<bool>(type: "bit", nullable: false),
                    HABITACIONES = table.Column<int>(type: "int", nullable: false),
                    CREATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DELETED = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOTELES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    APELLIDOS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DIRECCION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DELETED = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RESERVAS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_USUARIOID = table.Column<int>(type: "int", nullable: false),
                    ID_HOTELID = table.Column<int>(type: "int", nullable: false),
                    ID_HABITACION = table.Column<int>(type: "int", nullable: false),
                    CHECKIN = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CHECKOUT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ESTADO = table.Column<bool>(type: "bit", nullable: false),
                    CREATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DELETED = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESERVAS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RESERVAS_HOTELES_ID_HOTELID",
                        column: x => x.ID_HOTELID,
                        principalTable: "HOTELES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RESERVAS_USUARIOS_ID_USUARIOID",
                        column: x => x.ID_USUARIOID,
                        principalTable: "USUARIOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HOTELES_NOMBRE",
                table: "HOTELES",
                column: "NOMBRE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RESERVAS_ID_HOTELID",
                table: "RESERVAS",
                column: "ID_HOTELID");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVAS_ID_USUARIOID",
                table: "RESERVAS",
                column: "ID_USUARIOID");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_EMAIL",
                table: "USUARIOS",
                column: "EMAIL",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RESERVAS");

            migrationBuilder.DropTable(
                name: "HOTELES");

            migrationBuilder.DropTable(
                name: "USUARIOS");
        }
    }
}
