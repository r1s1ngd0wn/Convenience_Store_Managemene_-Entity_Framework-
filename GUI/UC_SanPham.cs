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

            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
        }

        private void LoadProductsForDeletion()
        {
            try
            {
                // blHangHoa.LayHangHoa() already filters for IsActive=true
                DataSet ds = blHangHoa.LayHangHoa();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dgvXoaHH.DataSource = ds.Tables[0];

                    // Configure dgvXoaHH columns (adjust as needed)
                    dgvXoaHH.AutoGenerateColumns = true; // Auto-generate for simplicity, or set manually
                    dgvXoaHH.ReadOnly = true;
                    dgvXoaHH.AllowUserToAddRows = false;
                    dgvXoaHH.AllowUserToDeleteRows = false;

                    // Set header texts (optional, but good practice)
                    if (dgvXoaHH.Columns.Contains("MaSanPham")) dgvXoaHH.Columns["MaSanPham"].HeaderText = "Mã SP";
                    if (dgvXoaHH.Columns.Contains("TenSP")) dgvXoaHH.Columns["TenSP"].HeaderText = "Tên SP";
                    if (dgvXoaHH.Columns.Contains("SoLuong")) dgvXoaHH.Columns["SoLuong"].HeaderText = "Số Lượng Tồn";
                    if (dgvXoaHH.Columns.Contains("Gia")) dgvXoaHH.Columns["Gia"].HeaderText = "Giá Bán";
                    if (dgvXoaHH.Columns.Contains("GiaNhap")) dgvXoaHH.Columns["GiaNhap"].HeaderText = "Giá Nhập";

                    // Format Gia and GiaNhap columns
                    if (dgvXoaHH.Columns.Contains("Gia")) dgvXoaHH.Columns["Gia"].DefaultCellStyle.Format = "N0";
                    if (dgvXoaHH.Columns.Contains("GiaNhap")) dgvXoaHH.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
                }
                else
                {
                    MessageBox.Show("Không tải được danh sách hàng hóa để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgvXoaHH.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu hàng hóa để xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvXoaHH.DataSource = null;
            }
        }

        // Event handler for TabControl's SelectedIndexChanged
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the "Xóa hàng hóa" tab (tabPage2) is selected
            if (tabControl1.SelectedTab == tabPage2)
            {
                if (!DesignMode) // Only load data at runtime
                {
                    LoadProductsForDeletion();
                }
            }
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

            // Optional: If user selected a row in dgvXoaHH, use its MaSanPham
            if (string.IsNullOrEmpty(maSanPhamXoa) && dgvXoaHH.SelectedRows.Count > 0)
            {
                maSanPhamXoa = dgvXoaHH.SelectedRows[0].Cells["MaSanPham"].Value?.ToString();
            }


            if (string.IsNullOrEmpty(maSanPhamXoa))
            {
                MessageBox.Show("Vui lòng nhập Mã hàng hóa cần xóa hoặc chọn một hàng từ danh sách.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn ngưng kinh doanh hàng hóa có mã '{maSanPhamXoa}'?", // Changed text
                                                 "Xác nhận Ngừng Kinh Doanh", // Changed text
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                string error = "";
                bool success = false;

                try
                {
                    // This will now perform soft deletion (set IsActive=false)
                    success = blHangHoa.XoaHangHoa(maSanPhamXoa, ref error);

                    if (success)
                    {
                        MessageBox.Show("Ngừng kinh doanh hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); // Changed text
                        txtMaHHXoa.Clear();
                        // Reload the DataGridView to reflect the change
                        if (!DesignMode)
                        {
                            LoadProductsForDeletion();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Ngừng kinh doanh hàng hóa thất bại: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); // Changed text
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi không mong muốn trong quá trình ngừng kinh doanh hàng hóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); // Changed text
                }
            }
        }
    }
}