// GUI/UC_HoaDon.cs
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBanHang_3Tang.BS_layer;

namespace Convenience_Store_Management.GUI
{
    public partial class UC_HoaDon : UserControl
    {
        private BLHoaDonBan blHoaDonBan = new BLHoaDonBan();
        private DataTable invoiceDetailTable;

        public UC_HoaDon()
        {
            InitializeComponent();
            SetupInvoiceDetailTable();
        }

        private void SetupInvoiceDetailTable()
        {
            invoiceDetailTable = new DataTable();
            invoiceDetailTable.Columns.Add("MaSanPham", typeof(string));
            invoiceDetailTable.Columns.Add("TenSP", typeof(string)); // We will fetch this for display
            invoiceDetailTable.Columns.Add("SoLuong", typeof(int));
            invoiceDetailTable.Columns.Add("GiaBan", typeof(decimal)); // Price at time of sale
            invoiceDetailTable.Columns.Add("ThanhTien", typeof(decimal));

            dgvInvoiceDetails.DataSource = invoiceDetailTable;

            // Configure DataGridView columns
            dgvInvoiceDetails.AutoGenerateColumns = false; // We will define columns manually

            // Add columns to dgvInvoiceDetails
            // Check if columns already exist before adding to avoid duplicates if designer generates them
            if (!dgvInvoiceDetails.Columns.Contains("ColMaSP"))
            {
                dgvInvoiceDetails.Columns.Add("ColMaSP", "Mã SP");
                dgvInvoiceDetails.Columns["ColMaSP"].DataPropertyName = "MaSanPham";
            }
            if (!dgvInvoiceDetails.Columns.Contains("ColTenSP"))
            {
                dgvInvoiceDetails.Columns.Add("ColTenSP", "Tên Sản Phẩm");
                dgvInvoiceDetails.Columns["ColTenSP"].DataPropertyName = "TenSP";
            }
            if (!dgvInvoiceDetails.Columns.Contains("ColSoLuong"))
            {
                dgvInvoiceDetails.Columns.Add("ColSoLuong", "Số Lượng");
                dgvInvoiceDetails.Columns["ColSoLuong"].DataPropertyName = "SoLuong";
            }
            if (!dgvInvoiceDetails.Columns.Contains("ColGiaBan"))
            {
                dgvInvoiceDetails.Columns.Add("ColGiaBan", "Giá Bán");
                dgvInvoiceDetails.Columns["ColGiaBan"].DataPropertyName = "GiaBan";
                dgvInvoiceDetails.Columns["ColGiaBan"].DefaultCellStyle.Format = "N0";
            }
            if (!dgvInvoiceDetails.Columns.Contains("ColThanhTien"))
            {
                dgvInvoiceDetails.Columns.Add("ColThanhTien", "Thành Tiền");
                dgvInvoiceDetails.Columns["ColThanhTien"].DataPropertyName = "ThanhTien";
                dgvInvoiceDetails.Columns["ColThanhTien"].DefaultCellStyle.Format = "N0";
            }

            dgvInvoiceDetails.ReadOnly = true;
            dgvInvoiceDetails.AllowUserToAddRows = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Retrieve data from UI controls
            string maSanPham = txtMaSP.Text.Trim(); // Use MaSP directly
            string soLuongStr = txtSoLuong.Text.Trim();

            // Basic validation for product input
            if (string.IsNullOrEmpty(maSanPham) || string.IsNullOrEmpty(soLuongStr))
            {
                MessageBox.Show("Vui lòng nhập Mã Sản Phẩm và Số lượng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuong;
            if (!int.TryParse(soLuongStr, out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string error = "";

            // Fetch product details from BLHangHoa based on MaSanPham
            // We need TenSP and Gia (sale price)
            var productInfo = blHoaDonBan.GetProductDetailsByMaSP(maSanPham); // Assuming BLHoaDonBan has this method

            if (productInfo == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm với mã đã nhập. Vui lòng kiểm tra mã sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenSP = productInfo.Rows[0]["TenSP"].ToString();
            decimal giaBan = Convert.ToDecimal(productInfo.Rows[0]["Gia"]);
            int soLuongTon = Convert.ToInt32(productInfo.Rows[0]["SoLuong"]);


            if (soLuong > soLuongTon)
            {
                MessageBox.Show($"Sản phẩm '{tenSP}' chỉ còn {soLuongTon} sản phẩm trong kho. Không đủ số lượng bạn yêu cầu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal thanhTien = soLuong * giaBan;

            // Check if product already exists in the current invoice details table
            DataRow existingRow = invoiceDetailTable.AsEnumerable()
                                                   .FirstOrDefault(row => row.Field<string>("MaSanPham") == maSanPham);

            if (existingRow != null)
            {
                int currentSoLuongInCart = existingRow.Field<int>("SoLuong");
                if (currentSoLuongInCart + soLuong > soLuongTon)
                {
                    MessageBox.Show($"Tổng số lượng sản phẩm '{tenSP}' trong hóa đơn sẽ vượt quá số lượng tồn kho ({soLuongTon}).", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existingRow["SoLuong"] = currentSoLuongInCart + soLuong;
                existingRow["ThanhTien"] = (currentSoLuongInCart + soLuong) * giaBan;
            }
            else
            {
                // Add product to the in-memory DataTable
                invoiceDetailTable.Rows.Add(maSanPham, tenSP, soLuong, giaBan, thanhTien);
            }


            // Clear product-specific input fields for the next item
            txtMaSP.Clear();
            txtSoLuong.Clear();

            MessageBox.Show($"Đã thêm '{soLuong} x {tenSP}' vào hóa đơn tạm thời.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UC_HoaDon_Load(object sender, EventArgs e)
        {

        }

        private void btnXuLyHD_Click(object sender, EventArgs e)
        {
            string maHoaDon = txtMaHD.Text.Trim();
            string maNhanVien = txtMaNV.Text.Trim();
            DateTime ngayBan = dtpNgayBan.Value;

            // Validate invoice header fields
            if (string.IsNullOrEmpty(maHoaDon) || string.IsNullOrEmpty(maNhanVien))
            {
                MessageBox.Show("Vui lòng nhập Mã Hóa Đơn và Mã Nhân Viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if there are any products added to the invoice
            if (invoiceDetailTable.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm vào hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string error = "";
            string sdtKhachHang = null; // Assuming no customer SDT input on this UI

            // Reuse ProcessSaleTransaction from BLHoaDonBan
            if (blHoaDonBan.ProcessSaleTransaction(maHoaDon, maNhanVien, sdtKhachHang, invoiceDetailTable, ref error))
            {
                MessageBox.Show("Hóa đơn đã được hoàn tất thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear all fields and the temporary invoice details table
                txtMaHD.Clear();
                txtMaNV.Clear();
                dtpNgayBan.Value = DateTime.Now;
                invoiceDetailTable.Clear(); // Clear all rows from the DataTable
            }
            else
            {
                MessageBox.Show($"Hoàn tất hóa đơn thất bại: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}