// GUI/UC_HoaDon.cs
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBanHang_3Tang.BS_layer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Convenience_Store_Management.GUI
{
    public partial class UC_HoaDon : UserControl
    {
        private BLHoaDonBan blHoaDonBan = new BLHoaDonBan();

        public UC_HoaDon()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Retrieve data from UI controls
            string maHoaDon = txtMaHD.Text.Trim();
            string tenSanPham = txtTenSP.Text.Trim(); // Assuming this is TenSP input
            string soLuongStr = txtSoLuong.Text.Trim();
            string maNhanVien = txtMaNV.Text.Trim();
            DateTime ngayBan = dtpNgayBan.Value;

            // Basic validation for required fields
            if (string.IsNullOrEmpty(maHoaDon) || string.IsNullOrEmpty(tenSanPham) ||
                string.IsNullOrEmpty(soLuongStr) || string.IsNullOrEmpty(maNhanVien))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hóa đơn và sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuong;
            if (!int.TryParse(soLuongStr, out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string error = "";

            // Use the new encapsulated transaction method
            if (blHoaDonBan.ProcessSingleInvoiceTransaction(maHoaDon, maNhanVien, tenSanPham, soLuong, ngayBan, ref error))
            {
                MessageBox.Show("Thêm hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optionally clear the input fields
                txtMaHD.Clear();
                txtTenSP.Clear(); // Clear TenSP
                txtSoLuong.Clear();
                txtMaNV.Clear();
                dtpNgayBan.Value = DateTime.Now;
            }
            else
            {
                MessageBox.Show($"Thêm hóa đơn thất bại: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}