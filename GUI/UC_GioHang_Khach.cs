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
        // Sử dụng DataTable để lưu trữ giỏ hàng trong bộ nhớ
        private DataTable cartTable = new DataTable();
        private BLHangHoa blHangHoa = new BLHangHoa();
        private BLHoaDonBan blHoaDonBan = new BLHoaDonBan();

        public UC_GioHang_Khach()
        {
            InitializeComponent();
            SetupCartTable();    // Khởi tạo cấu trúc DataTable
            // RefreshCartDisplay(); // Không cần gọi ở đây vì SetupCartTable đã gán DataSource
        }

        private void SetupCartTable()
        {
            // Định nghĩa các cột cho DataTable giống như bạn muốn hiển thị trên DataGridView
            cartTable.Columns.Add("MaSanPham", typeof(string));
            cartTable.Columns.Add("TenSP", typeof(string));
            cartTable.Columns.Add("Gia", typeof(decimal));
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

            if (!dgvGioHang.Columns.Contains("Gia")) dgvGioHang.Columns.Add("Gia", "Giá");
            dgvGioHang.Columns["Gia"].DataPropertyName = "Gia";
            dgvGioHang.Columns["Gia"].DefaultCellStyle.Format = "N0";

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
                decimal currentGia = existingRow.Field<decimal>("Gia");

                existingRow["SoLuong"] = currentSoLuong + soLuong;
                existingRow["ThanhTien"] = (currentSoLuong + soLuong) * currentGia;
            }
            else
            {
                // Nếu chưa có, thêm hàng mới vào DataTable
                DataRow newRow = cartTable.NewRow();
                newRow["MaSanPham"] = maSanPham;
                newRow["TenSP"] = tenSP;
                newRow["Gia"] = gia;
                newRow["SoLuong"] = soLuong;
                newRow["ThanhTien"] = soLuong * gia;
                cartTable.Rows.Add(newRow);
            }
            // Không cần gọi Refresh() cho DataGridView nếu DataSource là DataTable
            // DataTable tự động cập nhật khi thay đổi dữ liệu bên trong.
            // dgvGioHang.Refresh(); // Có thể bỏ qua nếu bạn thấy nó tự cập nhật
        }

        private void RefreshCartDisplay()
        {
            // Nếu bạn dùng DataTable làm DataSource, việc thay đổi DataTable sẽ tự động cập nhật DGV.
            // Tuy nhiên, đôi khi Refresh() hoặc Invalidate() có thể giúp cập nhật ngay lập tức nếu có vấn đề về hiển thị.
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
                    // RefreshCartDisplay(); // DataTable tự cập nhật DGV
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
            bool success = true;

            // For customer-initiated checkout, set MaNhanVien to NULL
            string maNhanVien = null; // Changed from SessionManager.CurrentLoggedInEmployeeId;

            // Remove validation for maNhanVien as it's now optional for customer checkouts
            // if (string.IsNullOrEmpty(maNhanVien))
            // {
            //     MessageBox.Show("Không có nhân viên nào đang đăng nhập. Vui lòng đăng nhập với vai trò nhân viên để thực hiện thanh toán.", "Lỗi Thanh Toán", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //     return;
            // }

            // SDTKhachHang will still come from SessionManager if customer is logged in

            string sdtKhachHang = SessionManager.CurrentLoggedInCustomerSdt;

            string maHoaDonBanMoi = "HDB" + DateTime.Now.ToString("yyyyMMddHHmmss");
            DateTime ngayBan = DateTime.Now;

            if (blHoaDonBan.ProcessSaleTransaction(maHoaDonBanMoi, maNhanVien, sdtKhachHang, cartTable, ref errorMessage))
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