using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivSys.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablasDeSeguridad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carreras",
                columns: table => new
                {
                    IDCarrera = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCarrera = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras", x => x.IDCarrera);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IDRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IDRol);
                });

            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    IDEstudiante = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    Nombre_s = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Semestre = table.Column<int>(type: "int", nullable: false),
                    CarreraID = table.Column<int>(type: "int", nullable: false),
                    EsBecado = table.Column<bool>(type: "bit", nullable: false),
                    EsEgresado = table.Column<bool>(type: "bit", nullable: false),
                    CarreraIDCarrera = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.IDEstudiante);
                    table.ForeignKey(
                        name: "FK_Estudiantes_Carreras_CarreraID",
                        column: x => x.CarreraID,
                        principalTable: "Carreras",
                        principalColumn: "IDCarrera",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estudiantes_Carreras_CarreraIDCarrera",
                        column: x => x.CarreraIDCarrera,
                        principalTable: "Carreras",
                        principalColumn: "IDCarrera");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IDUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IDUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IDRol",
                        column: x => x.IDRol,
                        principalTable: "Roles",
                        principalColumn: "IDRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstudiantesBecados",
                columns: table => new
                {
                    IDEstudiante = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    PorcentajeBeca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudiantesBecados", x => x.IDEstudiante);
                    table.ForeignKey(
                        name: "FK_EstudiantesBecados_Estudiantes_IDEstudiante",
                        column: x => x.IDEstudiante,
                        principalTable: "Estudiantes",
                        principalColumn: "IDEstudiante");
                });

            migrationBuilder.CreateTable(
                name: "EstudiantesEgresados",
                columns: table => new
                {
                    IDEstudiante = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    FechaEgreso = table.Column<DateTime>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudiantesEgresados", x => x.IDEstudiante);
                    table.ForeignKey(
                        name: "FK_EstudiantesEgresados_Estudiantes_IDEstudiante",
                        column: x => x.IDEstudiante,
                        principalTable: "Estudiantes",
                        principalColumn: "IDEstudiante");
                });

            migrationBuilder.CreateTable(
                name: "HistorialAcademico",
                columns: table => new
                {
                    IDRegistro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDEstudiante = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    Materia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Calificacion = table.Column<decimal>(type: "DECIMAL(4,2)", nullable: false),
                    Periodo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialAcademico", x => x.IDRegistro);
                    table.ForeignKey(
                        name: "FK_HistorialAcademico_Estudiantes_IDEstudiante",
                        column: x => x.IDEstudiante,
                        principalTable: "Estudiantes",
                        principalColumn: "IDEstudiante",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_CarreraID",
                table: "Estudiantes",
                column: "CarreraID");

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_CarreraIDCarrera",
                table: "Estudiantes",
                column: "CarreraIDCarrera");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialAcademico_IDEstudiante",
                table: "HistorialAcademico",
                column: "IDEstudiante");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IDRol",
                table: "Usuarios",
                column: "IDRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstudiantesBecados");

            migrationBuilder.DropTable(
                name: "EstudiantesEgresados");

            migrationBuilder.DropTable(
                name: "HistorialAcademico");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Carreras");
        }
    }
}
