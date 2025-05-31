// BUS/BLKhachHang.cs
using System;
using System.Data;
using System.Linq; // For LINQ queries
using Convenience_Store_Management.DAL;
using Convenience_Store_Management.Models; // Add this namespace

namespace QLBanHang_3Tang.BS_layer
{
    public class BLKhachHang
    {
        // Remove public ConnectDB db;

        public BLKhachHang()
        {
        }

        public DataSet LayKhachHang(ref string error)
        {
            try
            {
                using (var dbContext = new ConvenienceStoreDbContext())
                {
                    var khachHangs = dbContext.KhachHangs.Select(kh => new
                    {
                        kh.SDT,
                        kh.TenKH,
                        kh.NgaySinh
                    }).ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("SDT", typeof(string));
                    dt.Columns.Add("TenKH", typeof(string));
                    dt.Columns.Add("NgaySinh", typeof(DateTime));

                    foreach (var kh in khachHangs)
                    {
                        dt.Rows.Add(kh.SDT, kh.TenKH, kh.NgaySinh);
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

        public DataSet TimKhachHang(string sdt, ref string error)
        {
            try
            {
                using (var dbContext = new ConvenienceStoreDbContext())
                {
                    var khachHangs = dbContext.KhachHangs
                                           .Where(kh => kh.SDT.Contains(sdt))
                                           .Select(kh => new
                                           {
                                               kh.SDT,
                                               kh.TenKH,
                                               kh.NgaySinh
                                           }).ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("SDT", typeof(string));
                    dt.Columns.Add("TenKH", typeof(string));
                    dt.Columns.Add("NgaySinh", typeof(DateTime));

                    foreach (var kh in khachHangs)
                    {
                        dt.Rows.Add(kh.SDT, kh.TenKH, kh.NgaySinh);
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