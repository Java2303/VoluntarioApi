using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerApi.Migrations
{
    /// <inheritdoc />
    public partial class CrearTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    CursoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Dificultad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.CursoID);
                });

            migrationBuilder.CreateTable(
                name: "Donadores",
                columns: table => new
                {
                    DonadorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CI_NIT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumeroReferencia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donadores", x => x.DonadorID);
                });

            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    EspecialidadID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEspecialidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.EspecialidadID);
                });

            migrationBuilder.CreateTable(
                name: "Privilegios",
                columns: table => new
                {
                    PrivilegioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePrivilegio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilegios", x => x.PrivilegioID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolID);
                });

            migrationBuilder.CreateTable(
                name: "RolesPrivilegios",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false),
                    PrivilegioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesPrivilegios", x => new { x.RolID, x.PrivilegioID });
                    table.ForeignKey(
                        name: "FK_RolesPrivilegios_Privilegios_PrivilegioID",
                        column: x => x.PrivilegioID,
                        principalTable: "Privilegios",
                        principalColumn: "PrivilegioID");
                    table.ForeignKey(
                        name: "FK_RolesPrivilegios_Roles_RolID",
                        column: x => x.RolID,
                        principalTable: "Roles",
                        principalColumn: "RolID");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RolID = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioID);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolID",
                        column: x => x.RolID,
                        principalTable: "Roles",
                        principalColumn: "RolID");
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    EventoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEvento = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrganizadorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.EventoID);
                    table.ForeignKey(
                        name: "FK__Eventos__Organiz__66603565",
                        column: x => x.OrganizadorID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "Voluntarios",
                columns: table => new
                {
                    VoluntarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: true),
                    EspecialidadID = table.Column<int>(type: "int", nullable: true),
                    Sexo = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    FechaNac = table.Column<DateOnly>(type: "date", nullable: true),
                    Domicilio = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NumeroCelular = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntarios", x => x.VoluntarioID);
                    table.ForeignKey(
                        name: "FK_Voluntarios_Especialidades_EspecialidadID",
                        column: x => x.EspecialidadID,
                        principalTable: "Especialidades",
                        principalColumn: "EspecialidadID");
                    table.ForeignKey(
                        name: "FK_Voluntarios_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "HistorialCapacitacion",
                columns: table => new
                {
                    HistorialID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoluntarioID = table.Column<int>(type: "int", nullable: true),
                    CursoID = table.Column<int>(type: "int", nullable: true),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: true),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialCapacitacion", x => x.HistorialID);
                    table.ForeignKey(
                        name: "FK_HistorialCapacitacion_Cursos_CursoID",
                        column: x => x.CursoID,
                        principalTable: "Cursos",
                        principalColumn: "CursoID");
                    table.ForeignKey(
                        name: "FK_HistorialCapacitacion_Voluntarios_VoluntarioID",
                        column: x => x.VoluntarioID,
                        principalTable: "Voluntarios",
                        principalColumn: "VoluntarioID");
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaAsignacion = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(getdate())"),
                    FechaLimite = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VoluntarioID = table.Column<int>(type: "int", nullable: true),
                    AdministradorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.TareaID);
                    table.ForeignKey(
                        name: "FK_Tareas_Usuarios_AdministradorID",
                        column: x => x.AdministradorID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                    table.ForeignKey(
                        name: "FK_Tareas_Voluntarios_VoluntarioID",
                        column: x => x.VoluntarioID,
                        principalTable: "Voluntarios",
                        principalColumn: "VoluntarioID");
                });

            migrationBuilder.CreateTable(
                name: "VoluntariosEventos",
                columns: table => new
                {
                    VoluntarioEventoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoluntarioID = table.Column<int>(type: "int", nullable: false),
                    EventoID = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoluntariosEventos", x => x.VoluntarioEventoID);
                    table.ForeignKey(
                        name: "FK_VoluntarioEvento_Evento",
                        column: x => x.EventoID,
                        principalTable: "Eventos",
                        principalColumn: "EventoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoluntarioEvento_Voluntario",
                        column: x => x.VoluntarioID,
                        principalTable: "Voluntarios",
                        principalColumn: "VoluntarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donadores_CI_NIT",
                table: "Donadores",
                column: "CI_NIT",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Especialidades_NombreEspecialidad",
                table: "Especialidades",
                column: "NombreEspecialidad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_OrganizadorID",
                table: "Eventos",
                column: "OrganizadorID");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialCapacitacion_CursoID",
                table: "HistorialCapacitacion",
                column: "CursoID");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialCapacitacion_VoluntarioID",
                table: "HistorialCapacitacion",
                column: "VoluntarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Privilegios_NombrePrivilegio",
                table: "Privilegios",
                column: "NombrePrivilegio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_NombreRol",
                table: "Roles",
                column: "NombreRol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolesPrivilegios_PrivilegioID",
                table: "RolesPrivilegios",
                column: "PrivilegioID");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_AdministradorID",
                table: "Tareas",
                column: "AdministradorID");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_VoluntarioID",
                table: "Tareas",
                column: "VoluntarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolID",
                table: "Usuarios",
                column: "RolID");

            migrationBuilder.CreateIndex(
                name: "IX_Voluntarios_EspecialidadID",
                table: "Voluntarios",
                column: "EspecialidadID");

            migrationBuilder.CreateIndex(
                name: "IX_Voluntarios_UsuarioID",
                table: "Voluntarios",
                column: "UsuarioID",
                unique: true,
                filter: "[UsuarioID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VoluntariosEventos_EventoID",
                table: "VoluntariosEventos",
                column: "EventoID");

            migrationBuilder.CreateIndex(
                name: "IX_VoluntariosEventos_VoluntarioID",
                table: "VoluntariosEventos",
                column: "VoluntarioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donadores");

            migrationBuilder.DropTable(
                name: "HistorialCapacitacion");

            migrationBuilder.DropTable(
                name: "RolesPrivilegios");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "VoluntariosEventos");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Privilegios");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Voluntarios");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
