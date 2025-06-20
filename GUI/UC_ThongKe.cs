﻿using System;
using System.Data;
using System.Windows.Forms;
using QLBanHang_3Tang.BS_layer;

namespace Convenience_Store_Management.GUI
{
    public partial class UC_ThongKe : UserControl
    {
        private BLHoaDonBan blHoaDonBan = new BLHoaDonBan();

        public UC_ThongKe()
        {
            InitializeComponent();

            cbDoanhThu.SelectedIndex = 0;
            cbLoiNhuan.SelectedIndex = 0;
            cbHangHoa.SelectedIndex = 0;
        }

        private void UC_ThongKe_Load(object sender, EventArgs e)
        {
            LoadDoanhThuData();
            LoadLoiNhuanData();
            LoadHangHoaDaBanData();
        }

        private void LoadDoanhThuData()
        {
            string filter = cbDoanhThu.SelectedItem?.ToString();
            string error = "";
            DataSet ds = blHoaDonBan.LayDoanhThu(filter, ref error);
            if (ds != null && ds.Tables.Count > 0)
            {
                dgvDoanhThu.DataSource = ds.Tables[0];
                // Format total revenue column
                if (dgvDoanhThu.Columns.Contains("TongDoanhThu"))
                {
                    dgvDoanhThu.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
                }
            }
            else
            {
                MessageBox.Show($"Lỗi tải doanh thu: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvDoanhThu.DataSource = null;
            }
        }

        private void LoadLoiNhuanData()
        {
            string filter = cbLoiNhuan.SelectedItem?.ToString();
            string error = "";
            DataSet ds = blHoaDonBan.LayLoiNhuan(filter, ref error);
            if (ds != null && ds.Tables.Count > 0)
            {
                dgvLoiNhuan.DataSource = ds.Tables[0];
                // Format total revenue column (since profit is simplified to revenue here)
                if (dgvLoiNhuan.Columns.Contains("TongDoanhThu"))
                {
                    dgvLoiNhuan.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
                }
            }
            else
            {
                MessageBox.Show($"Lỗi tải lợi nhuận: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvLoiNhuan.DataSource = null;
            }
        }

        private void LoadHangHoaDaBanData()
        {
            string filter = cbHangHoa.SelectedItem?.ToString();
            string error = "";
            DataSet ds = blHoaDonBan.LayCacMatHangDaBan(filter, ref error);
            if (ds != null && ds.Tables.Count > 0)
            {
                dgvHHDaBan.DataSource = ds.Tables[0];
                // Format currency columns
                if (dgvHHDaBan.Columns.Contains("TongThanhTien"))
                {
                    dgvHHDaBan.Columns["TongThanhTien"].DefaultCellStyle.Format = "N0";
                }
            }
            else
            {
                MessageBox.Show($"Lỗi tải mặt hàng đã bán: {error}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvHHDaBan.DataSource = null;
            }
        }

        private void cbDoanhThu_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDoanhThuData();
        }

        private void cbLoiNhuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLoiNhuanData();
        }

        private void cbHangHoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHangHoaDaBanData();
        }
    }
}