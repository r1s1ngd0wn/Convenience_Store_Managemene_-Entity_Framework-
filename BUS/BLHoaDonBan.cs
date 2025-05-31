// BUS/BLHoaDonBan.cs
using System;
using System.Data;
using System.Linq; // For LINQ queries
using Convenience_Store_Management.DAL;
using Convenience_Store_Management.Models; // Add this namespace
using System.Data.Entity; // For transactions and Include

namespace QLBanHang_3Tang.BS_layer
{
    public class BLHoaDonBan
    {
        // No longer need ConnectDB directly. Remove public ConnectDB db;
        // DbContext instances will be created per operation.

        public BLHoaDonBan()
        {
        }

        // Method to add a sales invoice (HOA_DON_BAN) - 5 arguments
        // This method will now take a DbContext parameter for transaction management
        public bool ThemHoaDonBan(ConvenienceStoreDbContext dbContext, string maHoaDonBan, string maNhanVien, string sdtKhachHang, DateTime ngayBan, ref string error)
        {
            try
            {
                var newHoaDonBan = new HoaDonBan
                {
                    MaHoaDonBan = maHoaDonBan,
                    MaNhanVien = maNhanVien,
                    SDTKhachHang = sdtKhachHang,
                    NgayBan = ngayBan
                };
                dbContext.HoaDonBans.Add(newHoaDonBan);
                return true; // SaveChanges will be called at the end of the transaction
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Method to add a sales detail item (CHI_TIET_BAN)
        // This method will now take a DbContext parameter for transaction management
        public bool ThemChiTietBan(ConvenienceStoreDbContext dbContext, string maHoaDonBan, string maSanPham, int soLuong, decimal giaBan, decimal thanhTien, ref string error)
        {
            try
            {
                var newChiTietBan = new ChiTietBan
                {
                    MaHoaDonBan = maHoaDonBan,
                    MaSanPham = maSanPham,
                    SoLuong = soLuong,
                    GiaBan = giaBan,
                    ThanhTien = thanhTien
                };
                dbContext.ChiTietBans.Add(newChiTietBan);
                return true; // SaveChanges will be called at the end of the transaction
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Method to update product stock quantity (increase or decrease)
        // This method will now take a DbContext parameter for transaction management
        public bool CapNhatSoLuongHangHoa(ConvenienceStoreDbContext dbContext, string maSanPham, int soLuongThayDoi, ref string error)
        {
            try
            {
                var hangHoa = dbContext.HangHoas.Find(maSanPham);
                if (hangHoa != null)
                {
                    hangHoa.SoLuong += soLuongThayDoi;
                    // dbContext.Entry(hangHoa).State = EntityState.Modified; // Often not needed if entity is tracked
                    return true; // SaveChanges will be called at the end of the transaction
                }
                else
                {
                    error = "Sản phẩm không tồn tại.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Method to process a full sales transaction (used by UC_GioHang_Khach)
        public bool ProcessSaleTransaction(string maHoaDonBan, string maNhanVien, string sdtKhachHang,
                                           DataTable cartItems, DateTime ngayBan, ref string error)
        {
            // totalBillAmount is calculated but not inserted into HOA_DON_BAN as per your request
            // decimal totalBillAmount = 0; // Removed as not used in this method

            using (var dbContext = new ConvenienceStoreDbContext())
            {
                DbContextTransaction transaction = null;
                try
                {
                    transaction = dbContext.Database.BeginTransaction(); // Begin EF transaction

                    if (!ThemHoaDonBan(dbContext, maHoaDonBan, maNhanVien, sdtKhachHang, ngayBan, ref error)) // Changed DateTime.Now to ngayBan
                    {
                        transaction.Rollback();
                        return false;
                    }

                    foreach (DataRow row in cartItems.Rows)
                    {
                        string maSanPham = row.Field<string>("MaSanPham");
                        int soLuong = row.Field<int>("SoLuong");
                        decimal giaBan = row.Field<decimal>("GiaBan");
                        decimal thanhTien = row.Field<decimal>("ThanhTien");

                        if (!ThemChiTietBan(dbContext, maHoaDonBan, maSanPham, soLuong, giaBan, thanhTien, ref error))
                        {
                            transaction.Rollback();
                            return false;
                        }

                        if (!CapNhatSoLuongHangHoa(dbContext, maSanPham, -soLuong, ref error))
                        {
                            transaction.Rollback();
                            return false;
                        }
                        // totalBillAmount += thanhTien; // Keep calculating total for internal use if needed // Removed as not used
                    }

                    dbContext.SaveChanges(); // Commit all changes
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    error = "Lỗi giao dịch bán hàng: " + ex.Message;
                    return false;
                }
            }
        }

        // NEW: Method to process a single sales invoice transaction (used by UC_HoaDon)
        public bool ProcessSingleInvoiceTransaction(string maHoaDon, string maNhanVien, string tenSanPham, int soLuong, DateTime ngayBan, ref string error)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                DbContextTransaction transaction = null;
                try
                {
                    transaction = dbContext.Database.BeginTransaction();

                    // Get MaSanPham from TenSP
                    string maSanPham = LayMaSanPhamTuTen(tenSanPham);
                    if (string.IsNullOrEmpty(maSanPham))
                    {
                        error = "Không tìm thấy mã sản phẩm cho tên sản phẩm đã nhập. Vui lòng kiểm tra tên sản phẩm.";
                        transaction.Rollback();
                        return false;
                    }

                    // Get GiaBan from MaSanPham
                    decimal? giaBan = LayGiaBan(maSanPham);
                    if (giaBan == null)
                    {
                        error = "Không lấy được giá bán của sản phẩm. Vui lòng kiểm tra mã sản phẩm hoặc dữ liệu sản phẩm.";
                        transaction.Rollback();
                        return false;
                    }

                    decimal thanhTien = soLuong * giaBan.Value;
                    string sdtKhachHang = null; // Assuming SDTKhachHang is nullable for single invoice UI

                    if (!ThemHoaDonBan(dbContext, maHoaDon, maNhanVien, sdtKhachHang, ngayBan, ref error))
                    {
                        transaction.Rollback();
                        return false;
                    }

                    if (!ThemChiTietBan(dbContext, maHoaDon, maSanPham, soLuong, giaBan.Value, thanhTien, ref error))
                    {
                        transaction.Rollback();
                        return false;
                    }

                    if (!CapNhatSoLuongHangHoa(dbContext, maSanPham, -soLuong, ref error))
                    {
                        transaction.Rollback();
                        return false;
                    }

                    dbContext.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    error = "Đã xảy ra lỗi không mong muốn trong quá trình thêm hóa đơn: " + ex.Message;
                    return false;
                }
            }
        }


        // Method to get MaSanPham from TenSP (used by UC_HoaDon)
        public string LayMaSanPhamTuTen(string tenSP)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                var hangHoa = dbContext.HangHoas
                                       .FirstOrDefault(hh => hh.TenSP == tenSP);
                return hangHoa?.MaSanPham;
            }
        }

        // Method to get GiaBan from MaSanPham (used by UC_HoaDon)
        public decimal? LayGiaBan(string maSP)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                var hangHoa = dbContext.HangHoas
                                       .FirstOrDefault(hh => hh.MaSanPham == maSP);
                return hangHoa?.Gia;
            }
        }

        // Statistics methods (used by UC_ThongKe)
        public DataSet LayDoanhThu(string filterType, ref string error)
        {
            try
            {
                using (var dbContext = new ConvenienceStoreDbContext())
                {
                    IQueryable<HoaDonBan> query = dbContext.HoaDonBans.Include(hdb => hdb.ChiTietBans);

                    DateTime now = DateTime.Now;
                    if (filterType == "Tuần")
                    {
                        DateTime startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
                        DateTime endOfWeek = startOfWeek.AddDays(7);
                        query = query.Where(hdb => hdb.NgayBan >= startOfWeek && hdb.NgayBan < endOfWeek);
                    }
                    else if (filterType == "Tháng")
                    {
                        DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
                        DateTime endOfMonth = startOfMonth.AddMonths(1);
                        query = query.Where(hdb => hdb.NgayBan >= startOfMonth && hdb.NgayBan < endOfMonth);
                    }

                    var result = query
                        .GroupBy(hdb => new { hdb.MaHoaDonBan, hdb.NgayBan })
                        .Select(g => new
                        {
                            g.Key.MaHoaDonBan,
                            g.Key.NgayBan,
                            TongDoanhThu = g.Sum(hdb => hdb.ChiTietBans.Sum(ctb => ctb.ThanhTien))
                        })
                        .OrderByDescending(x => x.NgayBan)
                        .ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaHoaDonBan", typeof(string));
                    dt.Columns.Add("NgayBan", typeof(DateTime));
                    dt.Columns.Add("TongDoanhThu", typeof(decimal));

                    foreach (var item in result)
                    {
                        dt.Rows.Add(item.MaHoaDonBan, item.NgayBan, item.TongDoanhThu);
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

        public DataSet LayLoiNhuan(string filterType, ref string error)
        {
            try
            {
                using (var dbContext = new ConvenienceStoreDbContext())
                {
                    // Query ChiTietBan and include HoaDonBan (for NgayBan) and HangHoa (for GiaNhap)
                    IQueryable<ChiTietBan> query = dbContext.ChiTietBans
                                                        .Include(ctb => ctb.HoaDonBan)
                                                        .Include(ctb => ctb.HangHoa); // Eager load HangHoa to access GiaNhap

                    DateTime now = DateTime.Now;
                    if (filterType == "Tuần")
                    {
                        DateTime startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
                        DateTime endOfWeek = startOfWeek.AddDays(7);
                        query = query.Where(ctb => ctb.HoaDonBan.NgayBan >= startOfWeek && ctb.HoaDonBan.NgayBan < endOfWeek);
                    }
                    else if (filterType == "Tháng")
                    {
                        DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
                        DateTime endOfMonth = startOfMonth.AddMonths(1);
                        query = query.Where(ctb => ctb.HoaDonBan.NgayBan >= startOfMonth && ctb.HoaDonBan.NgayBan < endOfMonth);
                    }

                    var result = query
                        .GroupBy(ctb => new { ctb.MaHoaDonBan, ctb.HoaDonBan.NgayBan }) // Group by invoice
                        .Select(g => new
                        {
                            g.Key.MaHoaDonBan,
                            g.Key.NgayBan,
                            // Calculate total profit for each invoice: Sum of (SalePrice - ImportPrice) * Quantity
                            TongLoiNhuan = g.Sum(ctb => (ctb.GiaBan - ctb.HangHoa.GiaNhap) * ctb.SoLuong)
                        })
                        .OrderByDescending(x => x.NgayBan)
                        .ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaHoaDonBan", typeof(string));
                    dt.Columns.Add("NgayBan", typeof(DateTime));
                    dt.Columns.Add("TongLoiNhuan", typeof(decimal)); // New column name for profit

                    foreach (var item in result)
                    {
                        dt.Rows.Add(item.MaHoaDonBan, item.NgayBan, item.TongLoiNhuan);
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

        public DataSet LayCacMatHangDaBan(string filterType, ref string error)
        {
            try
            {
                using (var dbContext = new ConvenienceStoreDbContext())
                {
                    IQueryable<ChiTietBan> query = dbContext.ChiTietBans.Include(ctb => ctb.HoaDonBan).Include(ctb => ctb.HangHoa);

                    DateTime now = DateTime.Now;
                    if (filterType == "Tuần")
                    {
                        DateTime startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
                        DateTime endOfWeek = startOfWeek.AddDays(7);
                        query = query.Where(ctb => ctb.HoaDonBan.NgayBan >= startOfWeek && ctb.HoaDonBan.NgayBan < endOfWeek);
                    }
                    else if (filterType == "Tháng")
                    {
                        DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
                        DateTime endOfMonth = startOfMonth.AddMonths(1);
                        query = query.Where(ctb => ctb.HoaDonBan.NgayBan >= startOfMonth && ctb.HoaDonBan.NgayBan < endOfMonth);
                    }

                    var result = query
                        .GroupBy(ctb => new { ctb.MaSanPham, ctb.HangHoa.TenSP })
                        .Select(g => new
                        {
                            g.Key.MaSanPham,
                            g.Key.TenSP,
                            TongSoLuongDaBan = g.Sum(ctb => ctb.SoLuong),
                            TongThanhTien = g.Sum(ctb => ctb.ThanhTien)
                        })
                        .OrderByDescending(x => x.TongSoLuongDaBan)
                        .ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaSanPham", typeof(string));
                    dt.Columns.Add("TenSP", typeof(string));
                    dt.Columns.Add("TongSoLuongDaBan", typeof(int));
                    dt.Columns.Add("TongThanhTien", typeof(decimal));

                    foreach (var item in result)
                    {
                        dt.Rows.Add(item.MaSanPham, item.TenSP, item.TongSoLuongDaBan, item.TongThanhTien);
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

        // Helper method to get product details by MaSanPham
        // This method is crucial for UC_HoaDon's new functionality
        public DataTable GetProductDetailsByMaSP(string maSP)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                var product = dbContext.HangHoas
                                       .Where(hh => hh.MaSanPham == maSP)
                                       .Select(hh => new { hh.MaSanPham, hh.TenSP, hh.Gia, hh.SoLuong })
                                       .FirstOrDefault();

                if (product != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaSanPham", typeof(string));
                    dt.Columns.Add("TenSP", typeof(string));
                    dt.Columns.Add("Gia", typeof(decimal)); // This is sale price
                    dt.Columns.Add("SoLuong", typeof(int)); // This is stock quantity

                    dt.Rows.Add(product.MaSanPham, product.TenSP, product.Gia, product.SoLuong);
                    return dt;
                }
                return null; // Product not found
            }
        }

        // Search methods (used by UC_TimKiem)
        public DataSet TimHoaDon(string maHoaDonBan, ref string error)
        {
            try
            {
                using (var dbContext = new ConvenienceStoreDbContext())
                {
                    var query = dbContext.HoaDonBans
                        .Include(hdb => hdb.ChiTietBans) // Include related ChiTietBan entries
                        .Where(hdb => hdb.MaHoaDonBan.Contains(maHoaDonBan))
                        .GroupBy(hdb => new { hdb.MaHoaDonBan, hdb.MaNhanVien, hdb.SDTKhachHang, hdb.NgayBan })
                        .Select(g => new
                        {
                            g.Key.MaHoaDonBan,
                            g.Key.MaNhanVien,
                            g.Key.SDTKhachHang,
                            g.Key.NgayBan,
                            TongCong = g.Sum(hdb => hdb.ChiTietBans.Sum(ctb => ctb.ThanhTien))
                        })
                        .ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaHoaDonBan", typeof(string));
                    dt.Columns.Add("MaNhanVien", typeof(string));
                    dt.Columns.Add("SDTKhachHang", typeof(string));
                    dt.Columns.Add("NgayBan", typeof(DateTime));
                    dt.Columns.Add("TongCong", typeof(decimal));

                    foreach (var item in query)
                    {
                        dt.Rows.Add(item.MaHoaDonBan, item.MaNhanVien, item.SDTKhachHang, item.NgayBan, item.TongCong);
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