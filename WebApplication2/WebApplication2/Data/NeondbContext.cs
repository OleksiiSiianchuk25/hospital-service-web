using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EF.context;

public partial class NeondbContext : DbContext
{
    public NeondbContext()
    {
    }

    public NeondbContext(DbContextOptions<NeondbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=ep-damp-wood-56616604.eu-central-1.aws.neon.tech;Database=neondb;Username=Orik25;Password=fnF9SZOoqQR7");*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("pk_appointments");

            entity.ToTable("appointments");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.DateAndTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_and_time");
            entity.Property(e => e.DoctorRef).HasColumnName("doctor_ref");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .HasColumnName("message");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.PatientRef).HasColumnName("patient_ref");

            entity.HasOne(d => d.DoctorRefNavigation).WithMany(p => p.AppointmentDoctorRefNavigations)
                .HasForeignKey(d => d.DoctorRef)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doctor");

            entity.HasOne(d => d.PatientRefNavigation).WithMany(p => p.AppointmentPatientRefNavigations)
                .HasForeignKey(d => d.PatientRef)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_patient");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("pk_users");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email_uk").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(255)
                .HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.RoleRef).HasColumnName("role_ref");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");

            entity.HasOne(d => d.RoleRefNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleRef)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role");
            entity.HasMany(u => u.AppointmentPatientRefNavigations)
                .WithOne(a => a.PatientRefNavigation)
                .HasForeignKey(a => a.PatientRef)
                .HasConstraintName("fk_patient")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.AppointmentDoctorRefNavigations)
                .WithOne(a => a.DoctorRefNavigation)
                .HasForeignKey(a => a.DoctorRef)
                .HasConstraintName("fk_doctor")
                .OnDelete(DeleteBehavior.Cascade);

        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
