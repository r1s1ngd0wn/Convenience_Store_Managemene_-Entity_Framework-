using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq; // Cần thiết cho FirstOrDefault, AsEnumerable
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
        private BLHangHoa blHangHoa; // Declare but don't initialize here
        private BLHoaDonBan blHoaDonBan; // Declare but don't initialize here

        public UC_GioHang_Khach()
        {
            InitializeComponent();
            // Initialize BLL objects only when not in design mode
            if (!DesignMode)
            {
                blHangHoa = new BLHangHoa();
                blHoaDonBan = new BLHoaDonBan();
            }
            SetupCartTable();
        }

        private void SetupCartTable()
        {
            // Định nghĩa các cột cho DataTable giống như bạn muốn hiển thị trên DataGridView
            cartTable.Columns.Add("MaSanPham", typeof(string));
            cartTable.Columns.Add("TenSP", typeof(string));
            cartTable.Columns.Add("GiaBan", typeof(decimal)); // Column name is GiaBan
            cartTable.Columns.Add("SoLuong", typeof(int));
            cartTable.Columns.Add("ThanhTien", typeof(decimal)); // Cột tính toán

            // Gán DataTable làm nguồn dữ liệu cho DataGridView
            dgvGioHang.DataSource = cartTable;

            // Thiết lập header text và định dạng cho DataGridView
            dgvGioHang.AutoGenerateColumns = false; // Tắt tự động tạo cột để tự kiểm soát

            // Kiểm tra và thêm cột nếu chưa có (để tránh lỗi nếu Designer đã tạo sẵn)
            if (!dgvGioHang.Columns.Contains("MaSanPham")) dgvGioHang.Columns.Add("MaSanPham", "Mã SP");
            dgvGioHang.Columns["MaSanPham"].DataPropertyName = "MaSanPham";

            if (!dgvGioHang.Columns.Contains("TenSP")) dgvGioHang.Columns.Add("TenSP", "Tên Sản Phẩm");
            dgvGioHang.Columns["TenSP"].DataPropertyName = "TenSP";

            // Fix the column binding to GiaBan
            if (!dgvGioHang.Columns.Contains("GiaBan")) // Check for "GiaBan" instead of "Gia"
            {
                dgvGioHang.Columns.Add("GiaBan", "Giá Bán"); // Header text can be "Giá Bán" for clarity
            }
            dgvGioHang.Columns["GiaBan"].DataPropertyName = "GiaBan"; // CHANGE: DataPropertyName to "GiaBan"
            dgvGioHang.Columns["GiaBan"].DefaultCellStyle.Format = "N0"; // Apply format to "GiaBan"

            if (!dgvGioHang.Columns.Contains("SoLuong")) dgvGioHang.Columns.Add("SoLuong", "Số Lượng");
            dgvGioHang.Columns["SoLuong"].DataPropertyName = "SoLuong";

            if (!dgvGioHang.Columns.Contains("ThanhTien")) dgvGioHang.Columns.Add("ThanhTien", "Thành Tiền");
            dgvGioHang.Columns["ThanhTien"].DataPropertyName = "ThanhTien";
            dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";

            // Chặn người dùng chỉnh sửa trực tiếp trên DataGridView
            dgvGioHang.ReadOnly = true;
            dgvGioHang.AllowUserToAddRows = false;
        }

        // Phương thức này sẽ được gọi từ UC_HangHoa_Khach khi một sản phẩm được thêm vào
        public void AddItemToCart(object sender, string maSanPham, string tenSP, int soLuong, decimal gia)
        {
            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa bằng LINQ
            DataRow existingRow = cartTable.AsEnumerable()
                                           .FirstOrDefault(row => row.Field<string>("MaSanPham") == maSanPham);

            if (existingRow != null)
            {
                // Nếu có rồi, tăng số lượng và cập nhật thành tiền
                int currentSoLuong = existingRow.Field<int>("SoLuong");
                decimal currentGia = existingRow.Field<decimal>("GiaBan"); // Correctly accessing "GiaBan"

                existingRow["SoLuong"] = currentSoLuong + soLuong;
                existingRow["ThanhTien"] = (currentSoLuong + soLuong) * currentGia;
            }
            else
            {
                // Nếu chưa có, thêm hàng mới vào DataTable
                DataRow newRow = cartTable.NewRow();
                newRow["MaSanPham"] = maSanPham;
                newRow["TenSP"] = tenSP;
                newRow["GiaBan"] = gia; // Correctly setting "GiaBan"
                newRow["SoLuong"] = soLuong;
                newRow["ThanhTien"] = soLuong * gia;
                cartTable.Rows.Add(newRow);
            }
        }

        private void RefreshCartDisplay()
        {
            dgvGioHang.Refresh();
        }

        private void btnXoaGioHang_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.SelectedRows.Count > 0)
            {
                // Lấy chỉ số dòng được chọn trong DataGridView
                int rowIndex = dgvGioHang.SelectedRows[0].Index;

                // Đảm bảo chỉ số hợp lệ và có dữ liệu trong cartTable
                if (rowIndex >= 0 && rowIndex < cartTable.Rows.Count)
                {
                    // Lấy DataRow tương ứng từ DataTable
                    DataRow rowToRemove = cartTable.Rows[rowIndex];
                    string tenSP = rowToRemove.Field<string>("TenSP");

                    cartTable.Rows.Remove(rowToRemove); // Xóa dòng khỏi DataTable

                    MessageBox.Show($"Đã xóa '{tenSP}' khỏi giỏ hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa khỏi giỏ hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e) // This is the "Thanh toán" button
        {
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm để thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string errorMessage = "";
            // bool success = true; // Variable not used

            string maNhanVien = null; // For customer-initiated checkout
            string sdtKhachHang = SessionManager.CurrentLoggedInCustomerSdt;

            string maHoaDonBanMoi = "HDB" + DateTime.Now.ToString("yyyyMMddHHmmss");
            DateTime ngayBan = DateTime.Now;

            if (blHoaDonBan.ProcessSaleTransaction(maHoaDonBanMoi, maNhanVien, sdtKhachHang, cartTable, ngayBan, ref errorMessage))
            {
                MessageBox.Show("Thanh toán thành công! Giỏ hàng đã được xóa.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cartTable.Clear();

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