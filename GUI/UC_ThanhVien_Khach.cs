using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBanHang_3Tang.BS_layer;
using Convenience_Store_Management.Helper;

namespace Convenience_Store_Management.GUI
{
    public partial class UC_ThanhVien_Khach : UserControl
    {
        private BLTaiKhoan blTaiKhoan = new BLTaiKhoan();

        public UC_ThanhVien_Khach()
        {
            InitializeComponent();
        }

        private void UC_ThanhVien_Khach_Load(object sender, EventArgs e)
        {
            string error = "";
            // Lấy tên đăng nhập hiện tại từ SDT khách hàng (đã đăng nhập)
            string currentUsername = blTaiKhoan.LayTenDangNhapTuSDTKhachHang(SessionManager.CurrentLoggedInCustomerSdt, ref error); //

            if (!string.IsNullOrEmpty(currentUsername))
            {
                txtCurrentUsername.Text = currentUsername;
            }
            else
            {
                MessageBox.Show("Không thể tải thông tin tên đăng nhập. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtCurrentPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
            txtNewPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
            txtConfirmNewPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        private void btnChangeCredentials_Click(object sender, EventArgs e)
        {
            string currentUsername = txtCurrentUsername.Text.Trim(); // Tên đăng nhập hiện tại (không đổi)
            string currentPassword = txtCurrentPassword.Text.Trim();
            // txtNewUsername không được sử dụng
            string newPassword = txtNewPassword.Text.Trim();
            string confirmNewPassword = txtConfirmNewPassword.Text.Trim();
            string sdtKhachHang = SessionManager.CurrentLoggedInCustomerSdt; // Lấy SDT khách hàng đã đăng nhập

            string error = "";

            // 1. Xác thực thông tin đầu vào
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            {
                MessageBox.Show("Vui lòng điền đầy đủ tất cả các trường mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmNewPassword)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không khớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Kiểm tra mật khẩu cũ có đúng không
            bool isCurrentPasswordValid = blTaiKhoan.KiemTraDangNhap(currentUsername, currentPassword, "Customer", ref error); //
            if (!isCurrentPasswordValid)
            {
                MessageBox.Show("Mật khẩu cũ không đúng. Vui lòng nhập lại.", "Lỗi Xác Thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Gọi phương thức BL để chỉ cập nhật mật khẩu
            // Gọi phương thức mới hoặc sửa đổi CapNhatTaiKhoanKhachHang để chỉ cập nhật mật khẩu
            if (blTaiKhoan.CapNhatMatKhauKhachHang(sdtKhachHang, currentUsername, newPassword, ref error)) //
            {
                MessageBox.Show("Cập nhật mật khẩu thành công!", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Sau khi đổi thành công, xóa các trường mật khẩu
                txtCurrentPassword.Clear();
                txtNewPassword.Clear();
                txtConfirmNewPassword.Clear();
                chkShowPassword.Checked = false;
            }
            else
            {
                MessageBox.Show($"Cập nhật mật khẩu thất bại: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}