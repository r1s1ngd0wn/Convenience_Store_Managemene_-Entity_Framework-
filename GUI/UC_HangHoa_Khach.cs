﻿using System;
using System.Data;
using System.Windows.Forms;
using QLBanHang_3Tang.BS_layer;

namespace Convenience_Store_Management.GUI
{
    public partial class UC_HangHoa_Khach : UserControl
    {
        private BLHangHoa blHangHoa = new BLHangHoa();

        // Delegate tham chiếu sự kiện khi sản phẩm được thêm vào giỏ hàng
        public delegate void AddToCartEventHandler(object sender, string maSanPham, string tenSP, int soLuong, decimal gia);

        public event AddToCartEventHandler OnAddToCart;

        public UC_HangHoa_Khach()
        {
            InitializeComponent();
            soluongText.Text = "1";
        }

        private void UC_HangHoa_Khach_Load(object sender, EventArgs e)
        {
            LoadHangHoaData();
        }

        public void LoadHangHoaData()
        {
            try
            {
                DataSet ds = blHangHoa.LayHangHoa();
                dataGridView1.DataSource = ds.Tables[0];

                if (dataGridView1.Columns.Contains("MaSanPham"))
                    dataGridView1.Columns["MaSanPham"].HeaderText = "Mã Sản Phẩm";
                if (dataGridView1.Columns.Contains("TenSP"))
                    dataGridView1.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                if (dataGridView1.Columns.Contains("SoLuong"))
                    dataGridView1.Columns["SoLuong"].HeaderText = "Số Lượng Tồn";
                if (dataGridView1.Columns.Contains("Gia"))
                    dataGridView1.Columns["Gia"].HeaderText = "Giá";
                if (dataGridView1.Columns.Contains("GiaNhap"))
                {
                    dataGridView1.Columns["GiaNhap"].Visible = false;
                }

                if (dataGridView1.Columns.Contains("Gia"))
                {
                    dataGridView1.Columns["Gia"].DefaultCellStyle.Format = "N0";
                }

                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                tensanpham_label.Text = row.Cells["TenSP"].Value.ToString();
                soluongText.Text = "1";
            }
        }

        private void btnThemGioHang_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string maSanPham = selectedRow.Cells["MaSanPham"].Value.ToString();
                string tenSP = selectedRow.Cells["TenSP"].Value.ToString();
                int soLuongTon = Convert.ToInt32(selectedRow.Cells["SoLuong"].Value);
                decimal gia = Convert.ToDecimal(selectedRow.Cells["Gia"].Value);

                int quantityToAdd;

                if (!int.TryParse(soluongText.Text, out quantityToAdd) || quantityToAdd <= 0)
                {
                    MessageBox.Show("Số lượng phải là một số nguyên dương.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (quantityToAdd > soLuongTon)
                {
                    MessageBox.Show($"Sản phẩm '{tenSP}' chỉ còn {soLuongTon} sản phẩm. Không đủ số lượng bạn yêu cầu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kích hoạt sự kiện OnAddToCart, truyền thông tin sản phẩm
                OnAddToCart?.Invoke(this, maSanPham, tenSP, quantityToAdd, gia);

                MessageBox.Show($"{quantityToAdd} x '{tenSP}' đã được thêm vào giỏ hàng.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để thêm vào giỏ hàng.", "Chưa Chọn Sản Phẩm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}