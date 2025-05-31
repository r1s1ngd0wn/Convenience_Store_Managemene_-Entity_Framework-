using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Convenience_Store_Management
{
    public partial class FormNhanVien : Form
    {
        public FormNhanVien()
        {
            InitializeComponent();
        }

        private void LoadSubForm(UserControl subForm)
        {
            pnlNhanVien.Controls.Clear(); // remove existing control(s)
            subForm.Dock = DockStyle.Fill; // fill the panel
            pnlNhanVien.Controls.Add(subForm); // add new control
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            LoadSubForm(new GUI.UC_SanPham());
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            LoadSubForm(new GUI.UC_HoaDon());
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadSubForm(new GUI.UC_TimKiem());
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            LoadSubForm(new GUI.UC_ThongKe());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
