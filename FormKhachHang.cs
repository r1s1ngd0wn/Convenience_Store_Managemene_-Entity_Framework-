using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Convenience_Store_Management.GUI; // Thêm namespace này

namespace Convenience_Store_Management
{
    public partial class FormKhachHang : Form
    {
        private UC_GioHang_Khach ucGioHang; // Khai báo thể hiện của UC_GioHang_Khach
        private UC_HangHoa_Khach ucHangHoa; // Khai báo thể hiện của UC_HangHoa_Khach

        public FormKhachHang()
        {
            InitializeComponent();
            InitializeUserControls(); // Khởi tạo các UserControl khi Form được tạo
        }

        private void InitializeUserControls()
        {
            ucGioHang = new UC_GioHang_Khach();
            ucHangHoa = new UC_HangHoa_Khach();

            // Đăng ký sự kiện OnAddToCart từ ucHangHoa tới một phương thức trong FormKhachHang
            ucHangHoa.OnAddToCart += ucGioHang.AddItemToCart; // Gắn sự kiện trực tiếp vào phương thức AddItemToCart của ucGioHang

            // Load UC_HangHoa_Khach ban đầu
            LoadSubForm(ucHangHoa);
        }

        private void LoadSubForm(UserControl subForm)
        {
            pnlKhachHang.Controls.Clear(); // remove existing control(s)
            subForm.Dock = DockStyle.Fill; // fill the panel
            pnlKhachHang.Controls.Add(subForm); // add new control
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnThanhVien_Click(object sender, EventArgs e)
        {
            LoadSubForm(new GUI.UC_ThanhVien_Khach());
        }

        private void btnGioHang_Click(object sender, EventArgs e)
        {
            LoadSubForm(ucGioHang); // Sử dụng thể hiện đã tồn tại của ucGioHang
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            LoadSubForm(ucHangHoa); // Sử dụng thể hiện đã tồn tại của ucHangHoa
        }

        // Bạn có thể thêm phương thức này nếu bạn cần refresh UC_HangHoa_Khach sau khi thanh toán
        public void RefreshHangHoaUC()
        {
            ucHangHoa.LoadHangHoaData();
        }
    }
}