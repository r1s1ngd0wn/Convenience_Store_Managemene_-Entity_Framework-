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
    public partial class UC_GioHang_Khach : UserControl
    {
        private DataTable cartTable = new DataTable();
        private BLHangHoa blHangHoa;
        private BLHoaDonBan blHoaDonBan;

        public UC_GioHang_Khach()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                blHangHoa = new BLHangHoa();
                blHoaDonBan = new BLHoaDonBan();
            }
            SetupCartTable();
            UpdateTotalCost();
        }

        private void UpdateTotalCost()
        {
            decimal totalCost = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                if (row["ThanhTien"] != DBNull.Value)
                {
                    totalCost += Convert.ToDecimal(row["ThanhTien"]);
                }
            }

            if (lblTongThanhTien != null)
            {
                lblTongThanhTien.Text = totalCost.ToString("N0"); // "N0" for number format
            }
        }

        private void SetupCartTable()
        {
            cartTable.Columns.Add("MaSanPham", typeof(string));
            cartTable.Columns.Add("TenSP", typeof(string));
            cartTable.Columns.Add("GiaBan", typeof(decimal));
            cartTable.Columns.Add("SoLuong", typeof(int));
            cartTable.Columns.Add("ThanhTien", typeof(decimal));

            dgvGioHang.DataSource = cartTable;

            dgvGioHang.AutoGenerateColumns = false;

            if (!dgvGioHang.Columns.Contains("MaSanPham")) dgvGioHang.Columns.Add("MaSanPham", "Mã SP");
            dgvGioHang.Columns["MaSanPham"].DataPropertyName = "MaSanPham";

            if (!dgvGioHang.Columns.Contains("TenSP")) dgvGioHang.Columns.Add("TenSP", "Tên Sản Phẩm");
            dgvGioHang.Columns["TenSP"].DataPropertyName = "TenSP";

            if (!dgvGioHang.Columns.Contains("GiaBan"))
            {
                dgvGioHang.Columns.Add("GiaBan", "Giá Bán");
            }
            dgvGioHang.Columns["GiaBan"].DataPropertyName = "GiaBan";
            dgvGioHang.Columns["GiaBan"].DefaultCellStyle.Format = "N0";

            if (!dgvGioHang.Columns.Contains("SoLuong")) dgvGioHang.Columns.Add("SoLuong", "Số Lượng");
            dgvGioHang.Columns["SoLuong"].DataPropertyName = "SoLuong";

            if (!dgvGioHang.Columns.Contains("ThanhTien")) dgvGioHang.Columns.Add("ThanhTien", "Thành Tiền");
            dgvGioHang.Columns["ThanhTien"].DataPropertyName = "ThanhTien";
            dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";

            dgvGioHang.ReadOnly = true;
            dgvGioHang.AllowUserToAddRows = false;
        }

        public void AddItemToCart(object sender, string maSanPham, string tenSP, int soLuong, decimal gia)
        {
            DataRow existingRow = cartTable.AsEnumerable()
                                           .FirstOrDefault(row => row.Field<string>("MaSanPham") == maSanPham);

            if (existingRow != null)
            {
                int currentSoLuong = existingRow.Field<int>("SoLuong");
                decimal currentGia = existingRow.Field<decimal>("GiaBan");

                existingRow["SoLuong"] = currentSoLuong + soLuong;
                existingRow["ThanhTien"] = (currentSoLuong + soLuong) * currentGia;
            }
            else
            {
                DataRow newRow = cartTable.NewRow();
                newRow["MaSanPham"] = maSanPham;
                newRow["TenSP"] = tenSP;
                newRow["GiaBan"] = gia;
                newRow["SoLuong"] = soLuong;
                newRow["ThanhTien"] = soLuong * gia;
                cartTable.Rows.Add(newRow);
            }
            UpdateTotalCost();
        }

        private void btnXoaGioHang_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.SelectedRows.Count > 0)
            {
                int rowIndex = dgvGioHang.SelectedRows[0].Index;

                if (rowIndex >= 0 && rowIndex < cartTable.Rows.Count)
                {
                    DataRow rowToRemove = cartTable.Rows[rowIndex];
                    string tenSP = rowToRemove.Field<string>("TenSP");

                    cartTable.Rows.Remove(rowToRemove);
                    UpdateTotalCost();

                    MessageBox.Show($"Đã xóa '{tenSP}' khỏi giỏ hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa khỏi giỏ hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Thanh toán
        {
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm để thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string errorMessage = "";

            string maNhanVien = null;
            string sdtKhachHang = SessionManager.CurrentLoggedInCustomerSdt;

            string maHoaDonBanMoi = "HDB" + DateTime.Now.ToString("yyyyMMddHHmmss");
            DateTime ngayBan = DateTime.Now;

            if (blHoaDonBan.ProcessSaleTransaction(maHoaDonBanMoi, maNhanVien, sdtKhachHang, cartTable, ngayBan, ref errorMessage))
            {
                MessageBox.Show("Thanh toán thành công! Giỏ hàng đã được xóa.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cartTable.Clear();
                UpdateTotalCost();

                if (this.ParentForm is FormKhachHang mainForm)
                {
                    mainForm.RefreshHangHoaUC();
                }
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra trong quá trình thanh toán: " + errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}