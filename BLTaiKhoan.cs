// BUS/BLTaiKhoan.cs
using Convenience_Store_Management.DAL;
using Convenience_Store_Management.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms; // This using is technically not needed if MessageBox is not used here, but harmless

namespace QLBanHang_3Tang.BS_layer
{
    public class BLTaiKhoan
    {
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
                    return true;
                }
            }
        }

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
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

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
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

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
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Combined registration method to ensure atomicity
        public bool RegisterAccount(string username, string password, string userRole, string identifier, string fullName, string employeePhoneNumber, DateTime? dob, ref string error)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                DbContextTransaction transaction = null;
                try
                {
                    transaction = dbContext.Database.BeginTransaction();

                    NhanVien addedNhanVien = null;
                    KhachHang addedKhachHang = null;

                    if (userRole == "Employee")
                    {
                        addedNhanVien = new NhanVien { MaNhanVien = identifier, HoTenNV = fullName, SdtNV = employeePhoneNumber };
                        dbContext.NhanViens.Add(addedNhanVien);
                    }
                    else if (userRole == "Customer")
                    {
                        addedKhachHang = new KhachHang { SDT = identifier, TenKH = fullName, NgaySinh = dob };
                        dbContext.KhachHangs.Add(addedKhachHang);
                    }
                    else
                    {
                        error = "Loại tài khoản không hợp lệ.";
                        transaction.Rollback();
                        return false;
                    }

                    if (userRole == "Employee")
                    {
                        var newEmployeeAccount = new DangNhapNhanVien { TenDangNhap = username, MatKhau = password, MaNhanVien = identifier };
                        newEmployeeAccount.NhanVien = addedNhanVien;
                        dbContext.DangNhapNhanViens.Add(newEmployeeAccount);
                    }
                    else if (userRole == "Customer")
                    {
                        var newCustomerAccount = new DangNhapKhachHang { TenDangNhap = username, MatKhau = password, SDTKhachHang = identifier };
                        newCustomerAccount.KhachHang = addedKhachHang;
                        dbContext.DangNhapKhachHangs.Add(newCustomerAccount);
                    }


                    dbContext.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException exValidation)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    var allValidationErrors = exValidation.EntityValidationErrors
                        .SelectMany(entityErrors => entityErrors.ValidationErrors)
                        .Select(validationError => $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");

                    string fullErrorMessage = string.Join(";\n", allValidationErrors);

                    error = "Lỗi xác thực dữ liệu khi đăng ký: " + fullErrorMessage;
                    return false;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    Exception innermostException = ex;
                    while (innermostException.InnerException != null)
                    {
                        innermostException = innermostException.InnerException;
                    }

                    error = "Lỗi trong quá trình đăng ký: " + ex.Message;
                    if (innermostException != ex)
                    {
                        error += "\nChi tiết lỗi (Innermost): " + innermostException.Message;
                    }

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

        public bool CapNhatTaiKhoanKhachHang(string sdtKhachHang, string oldUsername, string newUsername, string newPassword, ref string error)
        {
            using (var dbContext = new ConvenienceStoreDbContext())
            {
                DbContextTransaction transaction = null;
                try
                {
                    transaction = dbContext.Database.BeginTransaction();
                    bool success = true;

                    if (oldUsername != newUsername)
                    {
                        var customerAccount = dbContext.DangNhapKhachHangs.FirstOrDefault(dn => dn.SDTKhachHang == sdtKhachHang && dn.TenDangNhap == oldUsername);
                        if (customerAccount != null)
                        {
                            customerAccount.TenDangNhap = newUsername;
                            dbContext.Entry(customerAccount).State = System.Data.Entity.EntityState.Modified;
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            success = false;
                            error = "Không tìm thấy tài khoản khách hàng với tên đăng nhập cũ.";
                        }
                    }

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

        public bool CapNhatMatKhauKhachHang(ConvenienceStoreDbContext dbContext, string sdtKhachHang, string tenDangNhap, string newPassword, ref string error)
        {
            try
            {
                var customerAccount = dbContext.DangNhapKhachHangs.FirstOrDefault(dn => dn.SDTKhachHang == sdtKhachHang && dn.TenDangNhap == tenDangNhap);
                if (customerAccount != null)
                {
                    customerAccount.MatKhau = newPassword;
                    dbContext.Entry(customerAccount).State = System.Data.Entity.EntityState.Modified;
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
                        dbContext.SaveChanges();
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