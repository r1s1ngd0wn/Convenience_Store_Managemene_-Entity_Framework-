using System;
using System.Windows.Forms;
using QLBanHang_3Tang.BS_layer;
using System.Linq;

namespace Convenience_Store_Management
{
    public partial class FormReg : Form
    {
        private BLTaiKhoan blTaiKhoan = new BLTaiKhoan();

        public FormReg()
        {
            InitializeComponent();
            txtPwd.PasswordChar = '*';
            NhanVienCb.Checked = true;

            UpdateVisibilityBasedOnRole();
        }

        private void UpdateVisibilityBasedOnRole()
        {
            if (NhanVienCb.Checked)
            {
                manv_text.Visible = true;
                manhanvien_label.Visible = true;
                sdt_text.Visible = true;
                label.Visible = true;
                manv_text.Text = "";
                sdt_text.Text = "";
            }
            else if (KhachHangCb.Checked)
            {
                manv_text.Visible = false;
                manhanvien_label.Visible = false;
                sdt_text.Visible = true;
                label.Visible = true;
                sdt_text.Text = "";
                manv_text.Text = "";
            }
        }

        private void cbShowPwd_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPwd.Checked)
            {
                txtPwd.PasswordChar = '\0';
            }
            else
            {
                txtPwd.PasswordChar = '*';
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtAccount.Text.Trim();
            string password = txtPwd.Text.Trim();
            string identifier = ""; // Holds MaNhanVien or SDT
            string userRole = "";
            string error = "";
            string fullName = username; // Assuming username is used as full name
            DateTime? ngaySinh = null; // dob null for employee

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
                MessageBox.Show("Vui lòng chọn loại tài khoản (Nhân viên hoặc Khách hàng).", "Lỗi Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.", "Lỗi Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 7)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 7 ký tự.", "Lỗi Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Sanitize phone number to ensure only digits
            string cleanPhoneNumber = new string(sdt_text.Text.Trim().Where(char.IsDigit).ToArray());


            // Validate identifier field(s) based on role
            if (userRole == "Employee")
            {
                if (string.IsNullOrEmpty(manv_text.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng nhập Mã NV của bạn.", "Lỗi Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(cleanPhoneNumber)) // Use cleanPhoneNumber for validation
                {
                    MessageBox.Show("Vui lòng nhập SĐT của nhân viên.", "Lỗi Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Validate phone number length for employee
                if (cleanPhoneNumber.Length != 10)
                {
                    MessageBox.Show("Số điện thoại nhân viên phải có đúng 10 chữ số.", "Lỗi Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                identifier = manv_text.Text.Trim();
                fullName = username;
            }
            else if (userRole == "Customer")
            {
                if (string.IsNullOrEmpty(cleanPhoneNumber)) // Use cleanPhoneNumber for validation
                {
                    MessageBox.Show("Vui lòng nhập SĐT của khách hàng.", "Lỗi Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Validate phone number length for customer
                if (cleanPhoneNumber.Length != 10)
                {
                    MessageBox.Show("Số điện thoại khách hàng phải có đúng 10 chữ số.", "Lỗi Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                identifier = cleanPhoneNumber; // SDT for customer
                fullName = username; // TenKH (used as full name for customer)
            }

            // Check if username already exists to prevent duplicate registrations
            if (blTaiKhoan.KiemTraTonTaiTenDangNhap(username, ref error))
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.", "Lỗi Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Pass all required arguments to RegisterAccount based on userRole
                if (userRole == "Employee")
                {
                    if (blTaiKhoan.RegisterAccount(username, password, userRole, identifier, fullName, cleanPhoneNumber, null, ref error))
                    {
                        MessageBox.Show("Đăng ký tài khoản thành công!", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FormLogin formLogin = new FormLogin();
                        formLogin.Show();
                        this.Close();
                    }
                }
                else if (userRole == "Customer")
                {
                    if (blTaiKhoan.RegisterAccount(username, password, userRole, identifier, fullName, null, ngaySinh, ref error))
                    {
                        MessageBox.Show("Đăng ký tài khoản thành công!", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FormLogin formLogin = new FormLogin();
                        formLogin.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi: Loại tài khoản không xác định.", "Lỗi Đăng Ký", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi trong quá trình đăng ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                UpdateVisibilityBasedOnRole();
            }
        }

        private void KhachHangCb_CheckedChanged(object sender, EventArgs e)
        {
            if (KhachHangCb.Checked)
            {
                NhanVienCb.Checked = false;
                UpdateVisibilityBasedOnRole();
            }
        }
    }
}