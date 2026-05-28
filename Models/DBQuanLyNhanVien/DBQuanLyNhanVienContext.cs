using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _28_backend_netcore_06.Models.DBQuanLyNhanVien;

public partial class DBQuanLyNhanVienContext : DbContext
{
    public DBQuanLyNhanVienContext()
    {
    }

    public DBQuanLyNhanVienContext(DbContextOptions<DBQuanLyNhanVienContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NhamChuc> NhamChucs { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhongBan> PhongBans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DBQuanLyNhanVienConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NhamChuc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhamChuc__3214EC079EF07937");

            entity.ToTable("NhamChuc");

            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.TenChucVu).HasMaxLength(255);

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.NhamChucs)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__NhamChuc__MaNV__4E88ABD4");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhanVien__3214EC07E090166D");

            entity.ToTable("NhanVien");

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.MaPb).HasColumnName("MaPB");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.Ten).HasMaxLength(255);

            entity.HasOne(d => d.MaPbNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaPb)
                .HasConstraintName("FK_NhanVien_PhongBan");
        });

        modelBuilder.Entity<PhongBan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhongBan__3214EC07C0125CB7");

            entity.ToTable("PhongBan");

            entity.Property(e => e.TenPb)
                .HasMaxLength(255)
                .HasColumnName("TenPB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
