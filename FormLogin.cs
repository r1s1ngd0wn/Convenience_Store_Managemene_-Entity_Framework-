// FormLogin.cs
using System;
using System.Windows.Forms;
using QLBanHang_3Tang.BS_layer; // Đảm bảo namespace này đúng
using Convenience_Store_Management.Helper;

namespace Convenience_Store_Management
{
    public partial class FormLogin : Form
    {
        private BLTaiKhoan blTaiKhoan = new BLTaiKhoan(); // Khởi tạo BLL

        public FormLogin()
        {
            InitializeComponent();
            txtPwd.PasswordChar = '*'; // Ẩn mật khẩu mặc định
            NhanVienCb.Checked = true; // Đặt lựa chọn mặc định
        }

        private void cbShowPwd_CheckedChanged(object sender, EventArgs e)
        {
            // Chuyển đổi hiển thị mật khẩu
            if (cbShowPwd.Checked)
            {
                txtPwd.PasswordChar = '\0'; // Hiển thị mật khẩu
            }
            else
            {
                txtPwd.PasswordChar = '*'; // Ẩn mật khẩu
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtAccount.Text.Trim();
            string password = txtPwd.Text.Trim();
            string userRole = "";
            string error = "";

            if (NhanVienCb.Checked)
            {
                userRole = "Employee";
            }
            else if (KhachHangCb.Checked)
            {
                userRole = "Customer";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn loại tài khoản (Nhân viên hoặc Khách hàng).", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (blTaiKhoan.KiemTraDangNhap(username, password, userRole, ref error))
            {
                MessageBox.Show($"Đăng nhập thành công với vai trò: {userRole}!", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (userRole == "Employee")
                {
                    string maNhanVien = blTaiKhoan.LayMaNhanVienTuTenDangNhap(username, ref error);
                    SessionManager.CurrentLoggedInEmployeeId = maNhanVien; // Store employee ID
                    FormNhanVien formNhanVien = new FormNhanVien();
                    formNhanVien.Show();
                }
                else if (userRole == "Customer")
                {
                    string sdtKhachHang = blTaiKhoan.LaySDTKhachHangTuTenDangNhap(username, ref error);
                    SessionManager.CurrentLoggedInCustomerSdt = sdtKhachHang; // Store customer SDT
                    FormKhachHang formKhachHang = new FormKhachHang();
                    formKhachHang.Show();
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng, hoặc bạn chọn sai loại tài khoản!\n" + error, "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NhanVienCb_CheckedChanged(object sender, EventArgs e)
        {
            if (NhanVienCb.Checked)
            {
                KhachHangCb.Checked = false;
            }
        }

        private void KhachHangCb_CheckedChanged(object sender, EventArgs e)
        {
            if (KhachHangCb.Checked)
            {
                NhanVienCb.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormReg formReg = new FormReg();
            formReg.Show();
            this.Hide();
        }
    }
}