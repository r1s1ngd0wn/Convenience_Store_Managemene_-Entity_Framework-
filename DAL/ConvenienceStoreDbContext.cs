// ConvenienceStoreDbContext.cs (ONLY the OnModelCreating section should be updated)
using System.Data.Entity;
using Convenience_Store_Management.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Convenience_Store_Management.DAL
{
    public class ConvenienceStoreDbContext : DbContext
    {
        public ConvenienceStoreDbContext() : base("name=ConvenienceStoreDBContext")
        {
        }

        public DbSet<HangHoa> HangHoas { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<HoaDonBan> HoaDonBans { get; set; }
        public DbSet<ChiTietBan> ChiTietBans { get; set; }
        public DbSet<HoaDonNhap> HoaDonNhaps { get; set; }
        public DbSet<ChiTietNhap> ChiTietNhaps { get; set; }
        public DbSet<DangNhapNhanVien> DangNhapNhanViens { get; set; }
        public DbSet<DangNhapKhachHang> DangNhapKhachHangs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietBan>().HasKey(cb => new { cb.MaHoaDonBan, cb.MaSanPham });
            modelBuilder.Entity<ChiTietNhap>().HasKey(cn => new { cn.MaHoaDonNhap, cn.MaSanPham });

            // For HoaDonBan to KhachHang (nullable foreign key)
            modelBuilder.Entity<HoaDonBan>()
                .HasOptional(hdb => hdb.KhachHang)
                .WithMany(kh => kh.HoaDonBans)
                .HasForeignKey(hdb => hdb.SDTKhachHang);

            // For HoaDonBan to NhanVien (nullable foreign key)
            modelBuilder.Entity<HoaDonBan>()
                .HasOptional(hdb => hdb.NhanVien)
                .WithMany(nv => nv.HoaDonBans)
                .HasForeignKey(hdb => hdb.MaNhanVien);

            // REVISED RELATIONSHIPS FOR LOGIN TABLES (True One-to-Zero-or-One)
            // Configure NhanVien (principal) to DangNhapNhanVien (dependent)
            // This assumes MaNhanVien in DangNhapNhanVien is unique (due to SQL constraint)
            modelBuilder.Entity<NhanVien>()
            .HasOptional(nv => nv.DangNhapNhanVien) // NhanVien has an optional (0 or 1) DangNhapNhanVien
            .WithRequired(dn => dn.NhanVien);

            // Configure KhachHang (principal) to DangNhapKhachHang (dependent)
            // This assumes SDTKhachHang in DangNhapKhachHang is unique (due to SQL constraint)
            modelBuilder.Entity<KhachHang>()
            .HasOptional(kh => kh.DangNhapKhachHang) // KhachHang has an optional (0 or 1) DangNhapKhachHang
            .WithRequired(dn => dn.KhachHang);        // DangNhapKhachHang must have a required KhachHang

            base.OnModelCreating(modelBuilder);
        }
    }
}