using System;
using System.Data;
using System.Linq;
using Convenience_Store_Management.DAL;
using Convenience_Store_Management.Models;

namespace QLBanHang_3Tang.BS_layer
{
    public class BLHangHoa
    {
        public BLHangHoa()
        {
            
        }

        public DataSet LayHangHoa()
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                var hangHoas = dbContext.HangHoas
                                       .Where(hh => hh.IsActive)
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
                        GiaNhap = giaNhap,
                        IsActive = true
                    };

                    dbContext.HangHoas.Add(newHangHoa);
                    dbContext.SaveChanges();
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
                    var hangHoaToDeactivate = dbContext.HangHoas.Find(maSanPham);
                    if (hangHoaToDeactivate != null)
                    {
                        hangHoaToDeactivate.IsActive = false;
                        dbContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        error = "Không tìm thấy hàng hóa để ngưng kinh doanh.";
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
                                           .Where(hh => hh.MaSanPham.Contains(maSanPham) && hh.IsActive)
                                           .Select(hh => new
                                           {
                                               hh.MaSanPham,
                                               hh.TenSP,
                                               hh.SoLuong,
                                               hh.Gia,
                                               hh.GiaNhap,
                                               IsActive = true
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

        public bool CapNhatHangHoa(string maSanPham, decimal newGia, decimal newGiaNhap, int newSoLuong, ref string error)
        {
            try
            {
                using (var dbContext = new ConvenienceStoreDbContext())
                {
                    var productToUpdate = dbContext.HangHoas.Find(maSanPham);

                    if (productToUpdate != null)
                    {
                        productToUpdate.Gia = newGia;
                        productToUpdate.GiaNhap = newGiaNhap;
                        productToUpdate.SoLuong = newSoLuong;

                        dbContext.SaveChanges(); 
                        return true;
                    }
                    else
                    {
                        error = "Không tìm thấy hàng hóa để cập nhật.";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                // Include inner exception message for better debugging
                if (ex.InnerException != null)
                {
                    error += "\nInner Exception: " + ex.InnerException.Message;
                }
                return false;
            }
        }
    }
}