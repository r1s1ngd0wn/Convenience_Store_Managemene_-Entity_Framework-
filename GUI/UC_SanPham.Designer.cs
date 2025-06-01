using System;
using System.Drawing;
using System.Windows.Forms;
namespace Convenience_Store_Management.GUI
{
    partial class UC_SanPham
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGiaNhap = new System.Windows.Forms.TextBox();
            this.btnThemHH = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGiaBan = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.txtMaHHThem = new System.Windows.Forms.TextBox();
            this.txtTenHH = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnXoaHH = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaHHXoa = new System.Windows.Forms.TextBox();
            this.dgvXoaHH = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvXoaHH)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 2);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(864, 545);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.txtGiaNhap);
            this.tabPage1.Controls.Add(this.btnThemHH);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtGiaBan);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtSoLuong);
            this.tabPage1.Controls.Add(this.txtMaHHThem);
            this.tabPage1.Controls.Add(this.txtTenHH);
            this.tabPage1.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(856, 519);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Thêm hàng hóa";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(124, 322);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 39);
            this.label6.TabIndex = 10;
            this.label6.Text = "Giá nhập:";
            // 
            // txtGiaNhap
            // 
            this.txtGiaNhap.Location = new System.Drawing.Point(489, 314);
            this.txtGiaNhap.Margin = new System.Windows.Forms.Padding(2);
            this.txtGiaNhap.Name = "txtGiaNhap";
            this.txtGiaNhap.Size = new System.Drawing.Size(246, 47);
            this.txtGiaNhap.TabIndex = 9;
            // 
            // btnThemHH
            // 
            this.btnThemHH.Location = new System.Drawing.Point(654, 452);
            this.btnThemHH.Margin = new System.Windows.Forms.Padding(2);
            this.btnThemHH.Name = "btnThemHH";
            this.btnThemHH.Size = new System.Drawing.Size(120, 39);
            this.btnThemHH.TabIndex = 8;
            this.btnThemHH.Text = "Thêm";
            this.btnThemHH.UseVisualStyleBackColor = true;
            this.btnThemHH.Click += new System.EventHandler(this.btnThemHH_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(124, 271);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 39);
            this.label4.TabIndex = 7;
            this.label4.Text = "Giá bán:";
            // 
            // txtGiaBan
            // 
            this.txtGiaBan.Location = new System.Drawing.Point(489, 263);
            this.txtGiaBan.Margin = new System.Windows.Forms.Padding(2);
            this.txtGiaBan.Name = "txtGiaBan";
            this.txtGiaBan.Size = new System.Drawing.Size(246, 47);
            this.txtGiaBan.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(124, 222);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 39);
            this.label3.TabIndex = 5;
            this.label3.Text = "Số lượng:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(124, 169);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 39);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mã hàng hóa:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(124, 118);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 39);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tên hàng hóa:";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(489, 214);
            this.txtSoLuong.Margin = new System.Windows.Forms.Padding(2);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(246, 47);
            this.txtSoLuong.TabIndex = 2;
            // 
            // txtMaHHThem
            // 
            this.txtMaHHThem.Location = new System.Drawing.Point(489, 161);
            this.txtMaHHThem.Margin = new System.Windows.Forms.Padding(2);
            this.txtMaHHThem.Name = "txtMaHHThem";
            this.txtMaHHThem.Size = new System.Drawing.Size(246, 47);
            this.txtMaHHThem.TabIndex = 1;
            // 
            // txtTenHH
            // 
            this.txtTenHH.Location = new System.Drawing.Point(489, 110);
            this.txtTenHH.Margin = new System.Windows.Forms.Padding(2);
            this.txtTenHH.Name = "txtTenHH";
            this.txtTenHH.Size = new System.Drawing.Size(246, 47);
            this.txtTenHH.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvXoaHH);
            this.tabPage2.Controls.Add(this.btnXoaHH);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.txtMaHHXoa);
            this.tabPage2.Font = new System.Drawing.Font("Sans Serif Collection", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(856, 519);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Xóa hàng hóa";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnXoaHH
            // 
            this.btnXoaHH.Location = new System.Drawing.Point(741, 154);
            this.btnXoaHH.Margin = new System.Windows.Forms.Padding(2);
            this.btnXoaHH.Name = "btnXoaHH";
            this.btnXoaHH.Size = new System.Drawing.Size(110, 32);
            this.btnXoaHH.TabIndex = 9;
            this.btnXoaHH.Text = "Xóa";
            this.btnXoaHH.UseVisualStyleBackColor = true;
            this.btnXoaHH.Click += new System.EventHandler(this.btnXoaHH_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(77, 80);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(171, 39);
            this.label5.TabIndex = 5;
            this.label5.Text = "Mã hàng hóa:";
            // 
            // txtMaHHXoa
            // 
            this.txtMaHHXoa.Location = new System.Drawing.Point(271, 80);
            this.txtMaHHXoa.Margin = new System.Windows.Forms.Padding(2);
            this.txtMaHHXoa.Name = "txtMaHHXoa";
            this.txtMaHHXoa.Size = new System.Drawing.Size(414, 39);
            this.txtMaHHXoa.TabIndex = 4;
            // 
            // dgvXoaHH
            // 
            this.dgvXoaHH.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvXoaHH.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Sans Serif Collection", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvXoaHH.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvXoaHH.Location = new System.Drawing.Point(5, 191);
            this.dgvXoaHH.Name = "dgvXoaHH";
            this.dgvXoaHH.Size = new System.Drawing.Size(846, 323);
            this.dgvXoaHH.TabIndex = 10;
            // 
            // UC_SanPham
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UC_SanPham";
            this.Size = new System.Drawing.Size(864, 547);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvXoaHH)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TextBox txtTenHH;
        private TabPage tabPage2;
        private Button btnThemHH;
        private Label label4;
        private TextBox txtGiaBan;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtSoLuong;
        private TextBox txtMaHHThem;
        private Button btnXoaHH;
        private Label label5;
        private TextBox txtMaHHXoa;
        private Label label6;
        private TextBox txtGiaNhap;
        private DataGridView dgvXoaHH;
    }
}