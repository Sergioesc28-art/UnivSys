using Microsoft.EntityFrameworkCore;
using UnivSys.API.Models;

namespace UnivSys.API.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor: Recibe las opciones, incluida la cadena de conexión
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Mapeo de Tablas de SQL Server
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<HistorialAcademico> HistorialAcademico { get; set; }
        public DbSet<EstudianteBecado> EstudiantesBecados { get; set; }
        public DbSet<EstudianteEgresado> EstudiantesEgresados { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // Configuración de Mapeos (Claves, relaciones 1:1)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llama a la implementación base
            base.OnModelCreating(modelBuilder);
            
            // ===================================================================
            // CORRECCIÓN CLAVE: Configuración Explícita de Relaciones Uno a Uno (1:1)
            // Esto resuelve el error "The dependent side could not be determined..."
            // ===================================================================

            // 1. Relación UNO A UNO: Estudiante <--> EstudianteBecado
            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.DetalleBecado)   // Estudiante tiene una propiedad de navegación DetalleBecado
                .WithOne(b => b.Estudiante)     // DetalleBecado tiene una propiedad de navegación Estudiante
                .HasForeignKey<EstudianteBecado>(b => b.IDEstudiante) // La FK (y PK) está en EstudianteBecado y apunta a Estudiante.IDEstudiante
                .IsRequired(false); // La relación no es obligatoria (un estudiante puede no ser becado)

            // 2. Relación UNO A UNO: Estudiante <--> EstudianteEgresado
            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.DetalleEgresado) // Estudiante tiene una propiedad de navegación DetalleEgresado
                .WithOne(g => g.Estudiante)     // DetalleEgresado tiene una propiedad de navegación Estudiante
                .HasForeignKey<EstudianteEgresado>(g => g.IDEstudiante) // La FK (y PK) está en EstudianteEgresado y apunta a Estudiante.IDEstudiante
                .IsRequired(false); // La relación no es obligatoria (un estudiante puede no ser egresado)

            // 3. Relación UNO A MUCHOS: Carrera <--> Estudiante
            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.Carrera) // Estudiante tiene una Carrera
                .WithMany() // (Asumimos que la entidad Carrera no necesita una lista de Estudiantes, si la necesita se usaría .WithMany(c => c.Estudiantes))
                .HasForeignKey(e => e.CarreraID)
                .IsRequired(); // Un estudiante debe tener una carrera

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol) // Usuario tiene un Rol
                .WithMany() // (Asumimos que la entidad Rol no necesita una lista de Usuarios, si la necesita se usaría .WithMany(r => r.Usuarios))
                .HasForeignKey(u => u.IDRol)
                .IsRequired(); // Un usuario debe tener un rol
        }
    }
}