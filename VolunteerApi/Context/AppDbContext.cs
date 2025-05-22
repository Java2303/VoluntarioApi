using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VolunteerApi.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<Curso> Cursos { get; set; }
    public virtual DbSet<Donadore> Donadores { get; set; }
    public virtual DbSet<Especialidade> Especialidades { get; set; }
    public virtual DbSet<Evento> Eventos { get; set; }
    public virtual DbSet<HistorialCapacitacion> HistorialCapacitacions { get; set; }
    public virtual DbSet<Privilegio> Privilegios { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Tarea> Tareas { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Voluntario> Voluntarios { get; set; }
    public virtual DbSet<VoluntarioEvento> VoluntarioEventos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.CursoId);
            entity.Property(e => e.CursoId).HasColumnName("CursoID");
            entity.Property(e => e.Categoria).HasMaxLength(100);
            entity.Property(e => e.Dificultad).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<Donadore>(entity =>
        {
            entity.HasKey(e => e.DonadorId);
            entity.HasIndex(e => e.CiNit).IsUnique();
            entity.Property(e => e.DonadorId).HasColumnName("DonadorID");
            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.CiNit).HasMaxLength(50).HasColumnName("CI_NIT");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.NumeroReferencia).HasMaxLength(20);
        });

        modelBuilder.Entity<Especialidade>(entity =>
        {
            entity.HasKey(e => e.EspecialidadId);
            entity.HasIndex(e => e.NombreEspecialidad).IsUnique();
            entity.Property(e => e.EspecialidadId).HasColumnName("EspecialidadID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.NombreEspecialidad).HasMaxLength(100);
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.EventoId);
            entity.Property(e => e.EventoId).HasColumnName("EventoID");
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.NombreEvento).HasMaxLength(255);
            entity.Property(e => e.OrganizadorId).HasColumnName("OrganizadorID");
            entity.Property(e => e.Ubicacion).HasMaxLength(255);

            entity.HasOne(d => d.Organizador).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.OrganizadorId)
                .HasConstraintName("FK__Eventos__Organiz__66603565");

            // Relación muchos a muchos usando VoluntarioEvento como tabla intermedia
            entity.HasMany(e => e.Voluntarios)
                .WithMany(v => v.Eventos)
                .UsingEntity<VoluntarioEvento>(
                    j => j.HasOne(ve => ve.Voluntario).WithMany(v => v.VoluntariosEventos).HasForeignKey(ve => ve.VoluntarioId),
                    j => j.HasOne(ve => ve.Evento).WithMany(e => e.VoluntariosEventos).HasForeignKey(ve => ve.EventoId),
                    j =>
                    {
                        j.HasKey(ve => ve.VoluntarioEventoId); // Define la clave primaria para la tabla intermedia
                        j.Property(ve => ve.FechaAsignacion).HasColumnType("datetime").HasDefaultValueSql("(getdate())"); // Agrega la fecha de asignación
                        j.ToTable("VoluntariosEventos"); // Nombre de la tabla intermedia
                    });
        });

        modelBuilder.Entity<HistorialCapacitacion>(entity =>
        {
            entity.HasKey(e => e.HistorialId);
            entity.ToTable("HistorialCapacitacion");
            entity.Property(e => e.HistorialId).HasColumnName("HistorialID");
            entity.Property(e => e.CursoId).HasColumnName("CursoID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.VoluntarioId).HasColumnName("VoluntarioID");

            entity.HasOne(d => d.Curso).WithMany(p => p.HistorialCapacitacions)
                .HasForeignKey(d => d.CursoId);
            entity.HasOne(d => d.Voluntario).WithMany(p => p.HistorialCapacitacions)
                .HasForeignKey(d => d.VoluntarioId);
        });

        modelBuilder.Entity<Privilegio>(entity =>
        {
            entity.HasKey(e => e.PrivilegioId);
            entity.HasIndex(e => e.NombrePrivilegio).IsUnique();
            entity.Property(e => e.PrivilegioId).HasColumnName("PrivilegioID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.NombrePrivilegio).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId);
            entity.HasIndex(e => e.NombreRol).IsUnique();
            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.NombreRol).HasMaxLength(50);

            entity.HasMany(d => d.Privilegios).WithMany(p => p.Rols)
                .UsingEntity<Dictionary<string, object>>(
                    "RolesPrivilegio",
                    r => r.HasOne<Privilegio>().WithMany().HasForeignKey("PrivilegioId").OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Role>().WithMany().HasForeignKey("RolId").OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("RolId", "PrivilegioId");
                        j.ToTable("RolesPrivilegios");
                        j.IndexerProperty<int>("RolId").HasColumnName("RolID");
                        j.IndexerProperty<int>("PrivilegioId").HasColumnName("PrivilegioID");
                    });
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.TareaId);
            entity.Property(e => e.TareaId).HasColumnName("TareaID");
            entity.Property(e => e.AdministradorId).HasColumnName("AdministradorID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaAsignacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.VoluntarioId).HasColumnName("VoluntarioID");

            entity.HasOne(d => d.Administrador).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.AdministradorId);
            entity.HasOne(d => d.Voluntario).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.VoluntarioId);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Contraseña).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.RolId).HasColumnName("RolID");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Voluntario>(entity =>
        {
            entity.HasKey(e => e.VoluntarioId);
            entity.HasIndex(e => e.UsuarioId).IsUnique();
            entity.Property(e => e.VoluntarioId).HasColumnName("VoluntarioID");
            entity.Property(e => e.Domicilio).HasMaxLength(255);
            entity.Property(e => e.EspecialidadId).HasColumnName("EspecialidadID");
            entity.Property(e => e.NumeroCelular).HasMaxLength(20);
            entity.Property(e => e.Sexo).HasMaxLength(1).IsUnicode(false).IsFixedLength();
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Especialidad).WithMany(p => p.Voluntarios)
                .HasForeignKey(d => d.EspecialidadId);
            entity.HasOne(d => d.Usuario).WithOne(p => p.Voluntario)
                .HasForeignKey<Voluntario>(d => d.UsuarioId);
        });

        modelBuilder.Entity<VoluntarioEvento>(entity =>
        {
            entity.ToTable("VoluntariosEventos");
            entity.HasKey(e => e.VoluntarioEventoId);
            entity.Property(e => e.VoluntarioEventoId).HasColumnName("VoluntarioEventoID");
            entity.Property(e => e.VoluntarioId).HasColumnName("VoluntarioID");
            entity.Property(e => e.EventoId).HasColumnName("EventoID");
            entity.Property(e => e.FechaAsignacion).HasColumnType("datetime").HasDefaultValueSql("(getdate())");

            entity.HasOne(e => e.Voluntario)
                .WithMany(v => v.VoluntariosEventos)
                .HasForeignKey(e => e.VoluntarioId)
                .HasConstraintName("FK_VoluntarioEvento_Voluntario");

            entity.HasOne(e => e.Evento)
                .WithMany(ev => ev.VoluntariosEventos)
                .HasForeignKey(e => e.EventoId)
                .HasConstraintName("FK_VoluntarioEvento_Evento");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
