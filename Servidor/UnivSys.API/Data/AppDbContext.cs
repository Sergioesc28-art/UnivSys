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

        // Configuración de Mapeos (Claves, relaciones 1:1)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             // Configuración específica de las relaciones 1:1 que diseñaste:
             
             // EstudiantesBecados (IDEstudiante como PK y FK)
             modelBuilder.Entity<EstudianteBecado>()
                .HasKey(b => b.IDEstudiante); 
             modelBuilder.Entity<EstudianteBecado>()
                .HasOne<Estudiante>() 
                .WithOne()
                .HasForeignKey<EstudianteBecado>(b => b.IDEstudiante);
                
             // EstudiantesEgresados (IDEstudiante como PK y FK)
             modelBuilder.Entity<EstudianteEgresado>()
                .HasKey(e => e.IDEstudiante);
             modelBuilder.Entity<EstudianteEgresado>()
                .HasOne<Estudiante>()
                .WithOne()
                .HasForeignKey<EstudianteEgresado>(e => e.IDEstudiante);
                
            base.OnModelCreating(modelBuilder);
        }
    }
}