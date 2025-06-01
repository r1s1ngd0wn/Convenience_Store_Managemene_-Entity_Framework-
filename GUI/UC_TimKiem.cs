// GUI/UC_TimKiem.cs
using System;
using System.Data;
using System.Windows.Forms;
using QLBanHang_3Tang.BS_layer;

namespace Convenience_Store_Management.GUI
{
    public partial class UC_TimKiem : UserControl
    {
        private BLHangHoa blHangHoa = new BLHangHoa();
        private BLHoaDonBan blHoaDonBan = new BLHoaDonBan();
        private BLKhachHang blKhachHang = new BLKhachHang();

        public UC_TimKiem()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.UC_TimKiem_Load);
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClickForUpdate);
        }

        private void UC_TimKiem_Load(object sender, EventArgs e)
        {
            btnTimHH_Click(null, null); // Preload all products
            btnTimHD_Click(null, null); // Preload all invoices
            btnTimKH_Click(null, null); // Preload all customers
        }

        private void btnTimHH_Click(object sender, EventArgs e)
        {
            string maHangHoa = txtMaHH.Text.Trim();
            string error = "";
            DataSet ds = blHangHoa.TimHangHoa(maHangHoa, ref error);
            if (ds != null && ds.Tables.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hàng hóa nào khớp với mã đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show($"Lỗi tìm kiếm hàng hóa: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.DataSource = null;
            }
        }

        private void dataGridView1_CellClickForUpdate(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtGiaBanMoi.Text = row.Cells["Gia"].Value?.ToString();
                txtGiaNhapMoi.Text = row.Cells["GiaNhap"].Value?.ToString();
                txtSoLuongMoi.Text = row.Cells["SoLuong"].Value?.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hàng hóa từ danh sách để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSanPham = dataGridView1.SelectedRows[0].Cells["MaSanPham"].Value?.ToString();
            if (string.IsNullOrEmpty(maSanPham))
            {
                MessageBox.Show("Không thể lấy Mã Sản Phẩm từ hàng đã chọn. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string giaBanMoiStr = txtGiaBanMoi.Text.Trim();
            string giaNhapMoiStr = txtGiaNhapMoi.Text.Trim();
            string soLuongMoiStr = txtSoLuongMoi.Text.Trim();

            decimal newGiaBan;
            decimal newGiaNhap;
            int newSoLuong;

            if (string.IsNullOrEmpty(giaBanMoiStr) || string.IsNullOrEmpty(giaNhapMoiStr) || string.IsNullOrEmpty(soLuongMoiStr))
            {
                MessageBox.Show("Vui lòng điền đầy đủ tất cả các trường cập nhật (Giá bán mới, Giá nhập mới, Số lượng mới).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(giaBanMoiStr, out newGiaBan) || newGiaBan <= 0)
            {
                MessageBox.Show("Giá bán mới phải là một số dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(giaNhapMoiStr, out newGiaNhap) || newGiaNhap < 0)
            {
                MessageBox.Show("Giá nhập mới phải là một số không âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(soLuongMoiStr, out newSoLuong) || newSoLuong < 0)
            {
                MessageBox.Show("Số lượng mới phải là một số nguyên không âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string error = "";
            bool success = false;

            try
            {
                success = blHangHoa.CapNhatHangHoa(maSanPham, newGiaBan, newGiaNhap, newSoLuong, ref error);

                if (success)
                {
                    MessageBox.Show($"Cập nhật hàng hóa '{maSanPham}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtGiaBanMoi.Clear();
                    txtGiaNhapMoi.Clear();
                    txtSoLuongMoi.Clear();

                    btnTimHH_Click(null, null);
                }
                else
                {
                    MessageBox.Show($"Cập nhật hàng hóa thất bại: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi không mong muốn trong quá trình cập nhật hàng hóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimHD_Click(object sender, EventArgs e)
        {
            string maHoaDon = txtMaHD.Text.Trim();
            string error = "";
            DataSet ds = blHoaDonBan.TimHoaDon(maHoaDon, ref error);
            if (ds != null && ds.Tables.Count > 0)
            {
                dataGridView3.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn nào khớp với mã đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Format total column if exists
                if (dataGridView3.Columns.Contains("TongCong"))
                {
                    dataGridView3.Columns["TongCong"].DefaultCellStyle.Format = "N0";
                }
            }
            else
            {
                MessageBox.Show($"Lỗi tìm kiếm hóa đơn: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView3.DataSource = null;
            }
        }

        private void btnTimKH_Click(object sender, EventArgs e)
        {
            string sdtKhachHang = textBox1.Text.Trim();
            string error = "";
            DataSet ds = blKhachHang.TimKhachHang(sdtKhachHang, ref error);
            if (ds != null && ds.Tables.Count > 0)
            {
                dataGridView4.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy khách hàng nào khớp với SĐT đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show($"Lỗi tìm kiếm khách hàng: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView4.DataSource = null;
            }
        }
    }
}