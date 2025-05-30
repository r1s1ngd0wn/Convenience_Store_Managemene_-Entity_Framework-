using System;
using System.Drawing;
using System.Windows.Forms;
namespace Convenience_Store_Management.GUI
{
    partial class UC_ThongKe
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvDoanhThu = new System.Windows.Forms.DataGridView();
            this.cbDoanhThu = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvLoiNhuan = new System.Windows.Forms.DataGridView();
            this.cbLoiNhuan = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvHHDaBan = new System.Windows.Forms.DataGridView();
            this.cbHangHoa = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoanhThu)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoiNhuan)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHHDaBan)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(864, 547);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvDoanhThu);
            this.tabPage1.Controls.Add(this.cbDoanhThu);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(856, 521);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Doanh thu";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvDoanhThu
            // 
            this.dgvDoanhThu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDoanhThu.Location = new System.Drawing.Point(4, 181);
            this.dgvDoanhThu.Margin = new System.Windows.Forms.Padding(2);
            this.dgvDoanhThu.Name = "dgvDoanhThu";
            this.dgvDoanhThu.Size = new System.Drawing.Size(848, 336);
            this.dgvDoanhThu.TabIndex = 18;
            // 
            // cbDoanhThu
            // 
            this.cbDoanhThu.FormattingEnabled = true;
            this.cbDoanhThu.Items.AddRange(new object[] {
            "Tuần",
            "Tháng"});
            this.cbDoanhThu.Location = new System.Drawing.Point(196, 86);
            this.cbDoanhThu.Margin = new System.Windows.Forms.Padding(2);
            this.cbDoanhThu.Name = "cbDoanhThu";
            this.cbDoanhThu.Size = new System.Drawing.Size(103, 21);
            this.cbDoanhThu.TabIndex = 17;
            this.cbDoanhThu.SelectedIndexChanged += new System.EventHandler(this.cbDoanhThu_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(28, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 39);
            this.label3.TabIndex = 16;
            this.label3.Text = "Lọc theo:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvLoiNhuan);
            this.tabPage2.Controls.Add(this.cbLoiNhuan);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(856, 521);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Lợi nhuận";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvLoiNhuan
            // 
            this.dgvLoiNhuan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLoiNhuan.Location = new System.Drawing.Point(4, 180);
            this.dgvLoiNhuan.Margin = new System.Windows.Forms.Padding(2);
            this.dgvLoiNhuan.Name = "dgvLoiNhuan";
            this.dgvLoiNhuan.Size = new System.Drawing.Size(848, 337);
            this.dgvLoiNhuan.TabIndex = 18;
            // 
            // cbLoiNhuan
            // 
            this.cbLoiNhuan.FormattingEnabled = true;
            this.cbLoiNhuan.Items.AddRange(new object[] {
            "Tuần",
            "Tháng"});
            this.cbLoiNhuan.Location = new System.Drawing.Point(193, 86);
            this.cbLoiNhuan.Margin = new System.Windows.Forms.Padding(2);
            this.cbLoiNhuan.Name = "cbLoiNhuan";
            this.cbLoiNhuan.Size = new System.Drawing.Size(107, 21);
            this.cbLoiNhuan.TabIndex = 17;
            this.cbLoiNhuan.SelectedIndexChanged += new System.EventHandler(this.cbLoiNhuan_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 39);
            this.label1.TabIndex = 16;
            this.label1.Text = "Lọc theo:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgvHHDaBan);
            this.tabPage3.Controls.Add(this.cbHangHoa);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(856, 521);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Các mặt hàng đã bán";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvHHDaBan
            // 
            this.dgvHHDaBan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHHDaBan.Location = new System.Drawing.Point(4, 180);
            this.dgvHHDaBan.Margin = new System.Windows.Forms.Padding(2);
            this.dgvHHDaBan.Name = "dgvHHDaBan";
            this.dgvHHDaBan.Size = new System.Drawing.Size(848, 337);
            this.dgvHHDaBan.TabIndex = 15;
            // 
            // cbHangHoa
            // 
            this.cbHangHoa.FormattingEnabled = true;
            this.cbHangHoa.Items.AddRange(new object[] {
            "Tuần",
            "Tháng"});
            this.cbHangHoa.Location = new System.Drawing.Point(195, 86);
            this.cbHangHoa.Margin = new System.Windows.Forms.Padding(2);
            this.cbHangHoa.Name = "cbHangHoa";
            this.cbHangHoa.Size = new System.Drawing.Size(99, 21);
            this.cbHangHoa.TabIndex = 14;
            this.cbHangHoa.SelectedIndexChanged += new System.EventHandler(this.cbHangHoa_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 39);
            this.label2.TabIndex = 13;
            this.label2.Text = "Lọc theo:";
            // 
            // UC_ThongKe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UC_ThongKe";
            this.Size = new System.Drawing.Size(864, 547);
            this.Load += new System.EventHandler(this.UC_ThongKe_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoanhThu)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoiNhuan)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHHDaBan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Label label2;
        private DataGridView dgvDoanhThu;
        private ComboBox cbDoanhThu;
        private Label label3;
        private DataGridView dgvLoiNhuan;
        private ComboBox cbLoiNhuan;
        private Label label1;
        private DataGridView dgvHHDaBan;
        private ComboBox cbHangHoa;
    }
}