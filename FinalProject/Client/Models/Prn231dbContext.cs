using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Client.Models;

public partial class Prn231dbContext : DbContext
{
    public Prn231dbContext()
    {
    }

    public Prn231dbContext(DbContextOptions<Prn231dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=DESKTOP-NGLD5V9; database=PRN231DB;uid=sa;pwd=1234567890;trustservercertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Account_1");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "IX_Account").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.StudentId)
                .HasMaxLength(8)
                .IsFixedLength();

            entity.HasOne(d => d.Student).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Account_Student");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_Account_Teacher");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.ToTable("Class");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Class_Teacher");

            entity.HasMany(d => d.Students).WithMany(p => p.Classes)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassStudent",
                    r => r.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Class_Student_Student"),
                    l => l.HasOne<Class>().WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Class_Student_Class"),
                    j =>
                    {
                        j.HasKey("ClassId", "StudentId");
                        j.ToTable("Class_Student");
                        j.IndexerProperty<string>("StudentId")
                            .HasMaxLength(8)
                            .IsFixedLength();
                    });
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teacher");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
