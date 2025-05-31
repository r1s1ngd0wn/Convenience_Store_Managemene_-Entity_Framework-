// BUS/BLTaiKhoan.cs
using System;
using System.Data;
using System.Linq; // For LINQ queries
using Convenience_Store_Management.DAL;
using Convenience_Store_Management.Models; // Add this namespace
using System.Data.Entity; // For transactions

namespace QLBanHang_3Tang.BS_layer
{
    public class BLTaiKhoan
    {
        // Remove private ConnectDB db;

        public BLTaiKhoan()
        {
        }

        public bool KiemTraDangNhap(string tenDangNhap, string matKhau, string loaiTaiKhoan, ref string error)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                try
                {
                    if (loaiTaiKhoan == "Employee")
                    {
                        return dbContext.DangNhapNhanViens.Any(dn => dn.TenDangNhap == tenDangNhap && dn.MatKhau == matKhau);
                    }
                    else if (loaiTaiKhoan == "Customer")
                    {
                        return dbContext.DangNhapKhachHangs.Any(dn => dn.TenDangNhap == tenDangNhap && dn.MatKhau == matKhau);
                    }
                    else
                    {
                        error = "Loại tài khoản không hợp lệ.";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    error = "Lỗi khi kiểm tra đăng nhập: " + ex.Message;
                    return false;
                }
            }
        }

        public bool KiemTraTonTaiTenDangNhap(string tenDangNhap, ref string error)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                try
                {
                    bool existsInEmployee = dbContext.DangNhapNhanViens.Any(dn => dn.TenDangNhap == tenDangNhap);
                    if (existsInEmployee) return true;

                    bool existsInCustomer = dbContext.DangNhapKhachHangs.Any(dn => dn.TenDangNhap == tenDangNhap);
                    if (existsInCustomer) return true;

                    return false;
                }
                catch (Exception ex)
                {
                    error = "Lỗi kiểm tra tên đăng nhập tồn tại: " + ex.Message;
                    return true; // Return true on error to prevent new registration if check fails.
                }
            }
        }

        // Method to add a login account (Employee or Customer)
        // This method will now take a DbContext parameter for transaction management
        public bool ThemTaiKhoan(ConvenienceStoreDbContext dbContext, string tenDangNhap, string matKhau, string loaiTaiKhoan, string identifier, ref string error)
        {
            try
            {
                if (loaiTaiKhoan == "Employee")
                {
                    var newEmployeeAccount = new DangNhapNhanVien
                    {
                        TenDangNhap = tenDangNhap,
                        MatKhau = matKhau,
                        MaNhanVien = identifier
                    };
                    dbContext.DangNhapNhanViens.Add(newEmployeeAccount);
                }
                else if (loaiTaiKhoan == "Customer")
                {
                    var newCustomerAccount = new DangNhapKhachHang
                    {
                        TenDangNhap = tenDangNhap,
                        MatKhau = matKhau,
                        SDTKhachHang = identifier
                    };
                    dbContext.DangNhapKhachHangs.Add(newCustomerAccount);
                }
                else
                {
                    error = "Loại tài khoản không hợp lệ.";
                    return false;
                }
                return true; // SaveChanges will be called by the caller
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Method to add an Employee record
        // This method will now take a DbContext parameter for transaction management
        public bool ThemNhanVien(ConvenienceStoreDbContext dbContext, string maNhanVien, string hoTenNV, string sdtNV, ref string error)
        {
            try
            {
                var newNhanVien = new NhanVien
                {
                    MaNhanVien = maNhanVien,
                    HoTenNV = hoTenNV,
                    SdtNV = sdtNV
                };
                dbContext.NhanViens.Add(newNhanVien);
                return true; // SaveChanges will be called by the caller
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Method to add a Customer record
        // This method will now take a DbContext parameter for transaction management
        public bool ThemKhachHang(ConvenienceStoreDbContext dbContext, string sdtKhachHang, string tenKH, DateTime? ngaySinh, ref string error)
        {
            try
            {
                var newKhachHang = new KhachHang
                {
                    SDT = sdtKhachHang,
                    TenKH = tenKH,
                    NgaySinh = ngaySinh
                };
                dbContext.KhachHangs.Add(newKhachHang);
                return true; // SaveChanges will be called by the caller
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Combined registration method to ensure atomicity
        public bool RegisterAccount(string username, string password, string userRole, string identifier, string fullName, DateTime? dob, ref string error)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                DbContextTransaction transaction = null;
                try
                {
                    transaction = dbContext.Database.BeginTransaction();

                    bool detailAdded = false;
                    if (userRole == "Employee")
                    {
                        detailAdded = ThemNhanVien(dbContext, identifier, fullName, identifier, ref error); // Assuming identifier is MaNhanVien and SdtNV initially for reg
                    }
                    else if (userRole == "Customer")
                    {
                        detailAdded = ThemKhachHang(dbContext, identifier, fullName, dob, ref error);
                    }

                    if (!detailAdded)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    bool accountAdded = ThemTaiKhoan(dbContext, username, password, userRole, identifier, ref error);
                    if (!accountAdded)
                    {
                        transaction.Rollback();
                        return false;
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
                    error = "Lỗi trong quá trình đăng ký: " + ex.Message;
                    return false;
                }
            }
        }

        public string LayMaNhanVienTuTenDangNhap(string tenDangNhap, ref string error)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                try
                {
                    var employeeAccount = dbContext.DangNhapNhanViens
                                                   .FirstOrDefault(dn => dn.TenDangNhap == tenDangNhap);
                    return employeeAccount?.MaNhanVien;
                }
                catch (Exception ex)
                {
                    error = "Lỗi lấy mã nhân viên từ tài khoản: " + ex.Message;
                    return null;
                }
            }
        }

        public string LaySDTKhachHangTuTenDangNhap(string tenDangNhap, ref string error)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                try
                {
                    var customerAccount = dbContext.DangNhapKhachHangs
                                                   .FirstOrDefault(dn => dn.TenDangNhap == tenDangNhap);
                    return customerAccount?.SDTKhachHang;
                }
                catch (Exception ex)
                {
                    error = "Lỗi lấy SĐT khách hàng từ tài khoản: " + ex.Message;
                    return null;
                }
            }
        }

        public string LayTenDangNhapTuSDTKhachHang(string sdtKhachHang, ref string error)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                try
                {
                    var customerAccount = dbContext.DangNhapKhachHangs
                                                   .FirstOrDefault(dn => dn.SDTKhachHang == sdtKhachHang);
                    return customerAccount?.TenDangNhap;
                }
                catch (Exception ex)
                {
                    error = "Lỗi lấy tên đăng nhập từ SĐT khách hàng: " + ex.Message;
                    return null;
                }
            }
        }

        // Updated to use new CapNhatMatKhauKhachHang
        public bool CapNhatTaiKhoanKhachHang(string sdtKhachHang, string oldUsername, string newUsername, string newPassword, ref string error)
        {
            // This method is now redundant if you only update password.
            // If you intend to allow username change too, it would look like this:
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                DbContextTransaction transaction = null;
                try
                {
                    transaction = dbContext.Database.BeginTransaction();
                    bool success = true;

                    // If username changes
                    if (oldUsername != newUsername)
                    {
                        var customerAccount = dbContext.DangNhapKhachHangs.FirstOrDefault(dn => dn.SDTKhachHang == sdtKhachHang && dn.TenDangNhap == oldUsername);
                        if (customerAccount != null)
                        {
                            customerAccount.TenDangNhap = newUsername;
                            dbContext.Entry(customerAccount).State = System.Data.Entity.EntityState.Modified;
                            dbContext.SaveChanges(); // Save this change immediately to avoid primary key conflicts later
                        }
                        else
                        {
                            success = false;
                            error = "Không tìm thấy tài khoản khách hàng với tên đăng nhập cũ.";
                        }
                    }

                    // Then update password
                    if (success)
                    {
                        success = CapNhatMatKhauKhachHang(dbContext, sdtKhachHang, newUsername, newPassword, ref error);
                    }

                    if (success)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return success;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    error = "Lỗi trong quá trình cập nhật tài khoản: " + ex.Message;
                    return false;
                }
            }
        }

        // NEW: Phương thức chỉ cập nhật mật khẩu khách hàng
        // This method will now take a DbContext parameter for transaction management
        public bool CapNhatMatKhauKhachHang(ConvenienceStoreDbContext dbContext, string sdtKhachHang, string tenDangNhap, string newPassword, ref string error)
        {
            try
            {
                var customerAccount = dbContext.DangNhapKhachHangs.FirstOrDefault(dn => dn.SDTKhachHang == sdtKhachHang && dn.TenDangNhap == tenDangNhap);
                if (customerAccount != null)
                {
                    customerAccount.MatKhau = newPassword;
                    dbContext.Entry(customerAccount).State = System.Data.Entity.EntityState.Modified;
                    // SaveChanges is expected to be called by the calling method if part of a larger transaction
                    return true;
                }
                else
                {
                    error = "Không tìm thấy tài khoản khách hàng để cập nhật mật khẩu.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = "Lỗi trong quá trình cập nhật mật khẩu: " + ex.Message;
                return false;
            }
        }

        // Overloaded public method for CapNhatMatKhauKhachHang to be called independently
        public bool CapNhatMatKhauKhachHang(string sdtKhachHang, string tenDangNhap, string newPassword, ref string error)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                DbContextTransaction transaction = null;
                try
                {
                    transaction = dbContext.Database.BeginTransaction();
                    bool success = CapNhatMatKhauKhachHang(dbContext, sdtKhachHang, tenDangNhap, newPassword, ref error);
                    if (success)
                    {
                        dbContext.SaveChanges(); // Commit changes if successful and not part of a larger transaction
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return success;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    error = "Lỗi trong quá trình cập nhật mật khẩu: " + ex.Message;
                    return false;
                }
            }
        }
    }
}