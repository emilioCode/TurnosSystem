using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TurnosSystem.Models
{
    public partial class dbServicioTurnosContext : DbContext
    {
        public dbServicioTurnosContext()
        {
        }

        public dbServicioTurnosContext(DbContextOptions<dbServicioTurnosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Servicios> Servicios { get; set; }
        public virtual DbSet<Turnos> Turnos { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=dbServicioTurnos;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Servicios>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CodigoServicio)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.TipoServicio)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Turnos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Atendido).HasColumnName("atendido");

                entity.Property(e => e.Fecha)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HoraFin)
                    .HasColumnName("Hora_fin")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HoraInicio)
                    .HasColumnName("Hora_inicio")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario).HasColumnName("usuario");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cedula)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCompleto)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
