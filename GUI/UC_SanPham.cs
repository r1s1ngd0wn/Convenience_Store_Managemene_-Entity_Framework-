// GUI/UC_SanPham.cs
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

namespace Convenience_Store_Management.GUI
{
    public partial class UC_SanPham : UserControl
    {
        private BLHangHoa blHangHoa = new BLHangHoa();

        public UC_SanPham()
        {
            InitializeComponent();
        }

        private void btnThemHH_Click(object sender, EventArgs e)
        {
            string maSanPham = txtMaHHThem.Text.Trim();
            string tenSP = txtTenHH.Text.Trim();
            string soLuongStr = txtSoLuong.Text.Trim();
            string giaStr = txtGiaBan.Text.Trim();
            string giaNhapStr = txtGiaNhap.Text.Trim(); // NEW: Get GiaNhap from textbox

            if (string.IsNullOrEmpty(maSanPham) || string.IsNullOrEmpty(tenSP) ||
                string.IsNullOrEmpty(soLuongStr) || string.IsNullOrEmpty(giaStr) ||
                string.IsNullOrEmpty(giaNhapStr)) // NEW: Validate GiaNhap input
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm (Mã, Tên, Số lượng, Giá bán, Giá nhập).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuong;
            if (!int.TryParse(soLuongStr, out soLuong) || soLuong < 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên không âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal gia; // This is selling price
            if (!decimal.TryParse(giaStr, out gia) || gia <= 0)
            {
                MessageBox.Show("Giá bán phải là một số dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal giaNhap; // This is import price
            if (!decimal.TryParse(giaNhapStr, out giaNhap) || giaNhap < 0) // GiaNhap can be 0 or positive
            {
                MessageBox.Show("Giá nhập phải là một số không âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string error = "";
            bool success = false;

            // Removed blHangHoa.db.BeginTransaction();
            try
            {
                // Call ThemHangHoa with 6 arguments (added giaNhap)
                success = blHangHoa.ThemHangHoa(maSanPham, tenSP, soLuong, gia, giaNhap, ref error);

                if (success)
                {
                    // Removed blHangHoa.db.CommitTransaction();
                    MessageBox.Show("Thêm hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally clear input fields
                    txtMaHHThem.Clear();
                    txtTenHH.Clear();
                    txtSoLuong.Clear();
                    txtGiaBan.Clear();
                    txtGiaNhap.Clear(); // NEW: Clear GiaNhap
                }
                else
                {
                    // Removed blHangHoa.db.RollbackTransaction();
                    MessageBox.Show($"Thêm hàng hóa thất bại: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Removed blHangHoa.db.RollbackTransaction();
                MessageBox.Show($"Đã xảy ra lỗi không mong muốn trong quá trình thêm hàng hóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // btnXoaHH_Click remains unchanged
        private void btnXoaHH_Click(object sender, EventArgs e)
        {
            string maSanPhamXoa = txtMaHHXoa.Text.Trim();

            if (string.IsNullOrEmpty(maSanPhamXoa))
            {
                MessageBox.Show("Vui lòng nhập Mã hàng hóa cần xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa hàng hóa có mã '{maSanPhamXoa}'?",
                                                 "Xác nhận Xóa",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                string error = "";
                bool success = false;

                // Removed blHangHoa.db.BeginTransaction();
                try
                {
                    success = blHangHoa.XoaHangHoa(maSanPhamXoa, ref error);

                    if (success)
                    {
                        // Removed blHangHoa.db.CommitTransaction();
                        MessageBox.Show("Xóa hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtMaHHXoa.Clear();
                    }
                    else
                    {
                        // Removed blHangHoa.db.RollbackTransaction();
                        MessageBox.Show($"Xóa hàng hóa thất bại: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Removed blHangHoa.db.RollbackTransaction();
                    MessageBox.Show($"Đã xảy ra lỗi không mong muốn trong quá trình xóa hàng hóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}