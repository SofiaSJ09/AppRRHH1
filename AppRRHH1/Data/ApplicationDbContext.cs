using AppRRHH1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppRRHH1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Departamento> Departamentos { get; set; }

        public DbSet<Empleado> Empleados { get; set; }

        public DbSet<Jornada> Jornadas { get; set; }

        public DbSet<PuestoTrabajo> PuestoTrabajos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.ToTable("Departamento");

                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.ToTable("Empleado");

                entity.Property(e => e.Apellidos).HasMaxLength(75);
                entity.Property(e => e.Correo).HasMaxLength(50);
                entity.Property(e => e.Nombre).HasMaxLength(50);
                entity.Property(e => e.Telefono).HasMaxLength(50);

                entity.HasOne(d => d.PuestoTrabajo).WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.PuestoTrabajoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Empleado_PuestoTrabajo");
            });

            modelBuilder.Entity<Jornada>(entity =>
            {
                entity.Property(e => e.HorasTrabajadas).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.SalarioBruto).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Empleado).WithMany(p => p.Jornada)
                    .HasForeignKey(d => d.EmpleadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Jornadas_Empleado");
            });

            modelBuilder.Entity<PuestoTrabajo>(entity =>
            {
                entity.ToTable("PuestoTrabajo");

                entity.Property(e => e.Nombre).HasMaxLength(50);
                entity.Property(e => e.PagoHora).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Departamento).WithMany(p => p.PuestoTrabajos)
                    .HasForeignKey(d => d.DepartamentoId)
                    .HasConstraintName("FK_PuestoTrabajo_Departamento");
            });
        }
    }
}
