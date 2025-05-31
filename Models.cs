// Models.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Convenience_Store_Management.Models
{
    // HANG_HOA (Products)
    [Table("HANG_HOA")]
    public class HangHoa
    {
        [Key]
        [StringLength(20)]
        public string MaSanPham { get; set; }

        [Required]
        [StringLength(100)]
        public string TenSP { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        //[Column(TypeName = "DECIMAL(18, 2)")]
        public decimal Gia { get; set; } // Selling Price

        [Required]
        //[Column(TypeName = "DECIMAL(18, 2)")]
        public decimal GiaNhap { get; set; } // Import Price

        // Navigation properties for relationships
        public virtual ICollection<ChiTietBan> ChiTietBans { get; set; }
        public virtual ICollection<ChiTietNhap> ChiTietNhaps { get; set; }
    }

    // KHACH_HANG (Customers)
    [Table("KHACH_HANG")]
    public class KhachHang
    {
        [Key]
        [StringLength(15)]
        [RegularExpression(@"^\d{10}$")] // Ensures 10 digits
        public string SDT { get; set; } // Phone number (used as primary key)

        [Required]
        [StringLength(100)]
        public string TenKH { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime? NgaySinh { get; set; }

        // Navigation properties for relationships
        public virtual ICollection<HoaDonBan> HoaDonBans { get; set; }
        public virtual DangNhapKhachHang DangNhapKhachHang { get; set; }
    }

    // NHAN_VIEN (Employees)
    [Table("NHAN_VIEN")]
    public class NhanVien
    {
        [Key]
        [StringLength(20)]
        public string MaNhanVien { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTenNV { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^\d{10}$")] // Ensures 10 digits
        public string SdtNV { get; set; }

        // Navigation properties for relationships
        public virtual ICollection<HoaDonBan> HoaDonBans { get; set; }
        public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; }
        public virtual DangNhapNhanVien DangNhapNhanVien { get; set; }
    }

    // HOA_DON_BAN (Sales Invoices)
    [Table("HOA_DON_BAN")]
    public class HoaDonBan
    {
        [Key]
        [StringLength(20)]
        public string MaHoaDonBan { get; set; }

        [StringLength(20)]
        public string MaNhanVien { get; set; } // NULLABLE

        [StringLength(15)]
        public string SDTKhachHang { get; set; } // NULLABLE

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime NgayBan { get; set; }

        // Navigation properties
        [ForeignKey("MaNhanVien")]
        public virtual NhanVien NhanVien { get; set; }

        [ForeignKey("SDTKhachHang")]
        public virtual KhachHang KhachHang { get; set; }

        public virtual ICollection<ChiTietBan> ChiTietBans { get; set; }
    }

    // CHI_TIET_BAN (Sales Invoice Details)
    [Table("CHI_TIET_BAN")]
    public class ChiTietBan
    {
        [Key, Column(Order = 0)]
        [StringLength(20)]
        public string MaHoaDonBan { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(20)]
        public string MaSanPham { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        //[Column(TypeName = "DECIMAL(18, 2)")]
        public decimal GiaBan { get; set; }

        [Required]
        //[Column(TypeName = "DECIMAL(18, 2)")]
        public decimal ThanhTien { get; set; }

        // Navigation properties
        [ForeignKey("MaHoaDonBan")]
        public virtual HoaDonBan HoaDonBan { get; set; }

        [ForeignKey("MaSanPham")]
        public virtual HangHoa HangHoa { get; set; }
    }

    // HOA_DON_NHAP (Purchase Invoices)
    [Table("HOA_DON_NHAP")]
    public class HoaDonNhap
    {
        [Key]
        [StringLength(20)]
        public string MaHoaDonNhap { get; set; }

        [Required]
        [StringLength(20)]
        public string MaNhanVien { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime NgayNhap { get; set; }

        // Navigation properties
        [ForeignKey("MaNhanVien")]
        public virtual NhanVien NhanVien { get; set; }

        public virtual ICollection<ChiTietNhap> ChiTietNhaps { get; set; }
    }

    // CHI_TIET_NHAP (Purchase Invoice Details)
    [Table("CHI_TIET_NHAP")]
    public class ChiTietNhap
    {
        [Key, Column(Order = 0)]
        [StringLength(20)]
        public string MaHoaDonNhap { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(20)]
        public string MaSanPham { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        //[Column(TypeName = "DECIMAL(18, 2)")]
        public decimal GiaNhap { get; set; }

        [Required]
        //[Column(TypeName = "DECIMAL(18, 2)")]
        public decimal ThanhTien { get; set; }

        // Navigation properties
        [ForeignKey("MaHoaDonNhap")]
        public virtual HoaDonNhap HoaDonNhap { get; set; }

        [ForeignKey("MaSanPham")]
        public virtual HangHoa HangHoa { get; set; }
    }

    // DANG_NHAP_NHAN_VIEN (Employee Login Accounts)
    // DangNhapNhanVien (Employee Login Accounts)
    [Table("DANG_NHAP_NHAN_VIEN")]
    public class DangNhapNhanVien
    {
        // Remove [Key] from TenDangNhap
        [StringLength(50)]
        [Required] // TenDangNhap is still required and unique
                   // [Index(IsUnique = true)] // Requires System.ComponentModel.DataAnnotations.Schema for Index
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

        [Key] // MaNhanVien is now the PK of this table
        [StringLength(20)]
        [Required] // Still required
                   // [ForeignKey("NhanVien")] // Redundant if PK is also FK
        public string MaNhanVien { get; set; }

        // Navigation property
        public virtual NhanVien NhanVien { get; set; }
    }

    // DANG_NHAP_KHACH_HANG (Customer Login Accounts)
    [Table("DANG_NHAP_KHACH_HANG")]
    public class DangNhapKhachHang
    {
        [Key]
        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

        [Required]
        [StringLength(15)]
        public string SDTKhachHang { get; set; }

        // Navigation property
        public virtual KhachHang KhachHang { get; set; }
    }
}