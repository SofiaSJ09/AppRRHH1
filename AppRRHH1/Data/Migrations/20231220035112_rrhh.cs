using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppRRHH1.Data.Migrations
{
    /// <inheritdoc />
    public partial class rrhh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    DepartamentoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.DepartamentoId);
                });

            migrationBuilder.CreateTable(
                name: "PuestoTrabajo",
                columns: table => new
                {
                    PuestoTrabajoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PagoHora = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DepartamentoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuestoTrabajo", x => x.PuestoTrabajoId);
                    table.ForeignKey(
                        name: "FK_PuestoTrabajo_Departamento",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamento",
                        principalColumn: "DepartamentoId");
                });

            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    EmpleadoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PuestoTrabajoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado", x => x.EmpleadoId);
                    table.ForeignKey(
                        name: "FK_Empleado_PuestoTrabajo",
                        column: x => x.PuestoTrabajoId,
                        principalTable: "PuestoTrabajo",
                        principalColumn: "PuestoTrabajoId");
                });

            migrationBuilder.CreateTable(
                name: "Jornadas",
                columns: table => new
                {
                    JornadaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    HorasTrabajadas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalarioBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jornadas", x => x.JornadaId);
                    table.ForeignKey(
                        name: "FK_Jornadas_Empleado",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleado",
                        principalColumn: "EmpleadoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_PuestoTrabajoId",
                table: "Empleado",
                column: "PuestoTrabajoId");

            migrationBuilder.CreateIndex(
                name: "IX_Jornadas_EmpleadoId",
                table: "Jornadas",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_PuestoTrabajo_DepartamentoId",
                table: "PuestoTrabajo",
                column: "DepartamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jornadas");

            migrationBuilder.DropTable(
                name: "Empleado");

            migrationBuilder.DropTable(
                name: "PuestoTrabajo");

            migrationBuilder.DropTable(
                name: "Departamento");
        }
    }
}
