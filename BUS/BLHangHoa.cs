// BUS/BLHangHoa.cs
using System;
using System.Data;
using System.Linq; // For LINQ queries
using Convenience_Store_Management.DAL;
using Convenience_Store_Management.Models; // Add this namespace

namespace QLBanHang_3Tang.BS_layer
{
    public class BLHangHoa
    {
        // No longer need ConnectDB directly here.
        // The DbContext will manage connections.

        public BLHangHoa()
        {
            // DbContext is typically instantiated per-operation or per-request in web apps.
            // For a desktop app, you might manage its lifecycle differently,
            // but for simplicity here, we'll create it inside methods as needed.
        }

        public DataSet LayHangHoa()
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                var hangHoas = dbContext.HangHoas.Select(hh => new
                {
                    hh.MaSanPham,
                    hh.TenSP,
                    hh.SoLuong,
                    hh.Gia,
                    hh.GiaNhap
                }).ToList(); // Fetch data

                // Convert to DataSet for compatibility with existing UI
                DataTable dt = new DataTable();
                dt.Columns.Add("MaSanPham", typeof(string));
                dt.Columns.Add("TenSP", typeof(string));
                dt.Columns.Add("SoLuong", typeof(int));
                dt.Columns.Add("Gia", typeof(decimal));
                dt.Columns.Add("GiaNhap", typeof(decimal));

                foreach (var hh in hangHoas)
                {
                    dt.Rows.Add(hh.MaSanPham, hh.TenSP, hh.SoLuong, hh.Gia, hh.GiaNhap);
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;
            }
        }

        public bool ThemHangHoa(string maSanPham, string tenSP, int soLuong, decimal gia, decimal giaNhap, ref string error)
        {
            try
            {
                using (var dbContext = new ConvenienceStoreDbContext())
                {
                    var newHangHoa = new HangHoa
                    {
                        MaSanPham = maSanPham,
                        TenSP = tenSP,
                        SoLuong = soLuong,
                        Gia = gia,
                        GiaNhap = giaNhap
                    };

                    dbContext.HangHoas.Add(newHangHoa);
                    dbContext.SaveChanges(); // Persist changes to the database
                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool XoaHangHoa(string maSanPham, ref string error)
        {
            try
            {
                using (var dbContext = new ConvenienceStoreDbContext())
                {
                    var hangHoaToDelete = dbContext.HangHoas.Find(maSanPham); // Find by primary key
                    if (hangHoaToDelete != null)
                    {
                        dbContext.HangHoas.Remove(hangHoaToDelete);
                        dbContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        error = "Không tìm thấy hàng hóa để xóa.";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public string LayMaSanPhamTuTen(string tenSP)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                var hangHoa = dbContext.HangHoas
                                       .FirstOrDefault(hh => hh.TenSP == tenSP);
                return hangHoa?.MaSanPham;
            }
        }

        public decimal? LayGiaBan(string maSP)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                var hangHoa = dbContext.HangHoas
                                       .FirstOrDefault(hh => hh.MaSanPham == maSP);
                return hangHoa?.Gia;
            }
        }

        public DataSet TimHangHoa(string maSanPham, ref string error)
        {
            try
            {
                using (var dbContext = new ConvenienceStoreDbContext())
                {
                    var hangHoas = dbContext.HangHoas
                                           .Where(hh => hh.MaSanPham.Contains(maSanPham))
                                           .Select(hh => new
                                           {
                                               hh.MaSanPham,
                                               hh.TenSP,
                                               hh.SoLuong,
                                               hh.Gia,
                                               hh.GiaNhap
                                           }).ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaSanPham", typeof(string));
                    dt.Columns.Add("TenSP", typeof(string));
                    dt.Columns.Add("SoLuong", typeof(int));
                    dt.Columns.Add("Gia", typeof(decimal));
                    dt.Columns.Add("GiaNhap", typeof(decimal));

                    foreach (var hh in hangHoas)
                    {
                        dt.Rows.Add(hh.MaSanPham, hh.TenSP, hh.SoLuong, hh.Gia, hh.GiaNhap);
                    }

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }
    }
}