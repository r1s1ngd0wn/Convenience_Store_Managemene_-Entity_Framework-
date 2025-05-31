using System;
using System.Drawing;
using System.Windows.Forms;
namespace Convenience_Store_Management.GUI
{
    partial class UC_HangHoa_Khach
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.btnThemGioHang = new System.Windows.Forms.Button();
            this.tensanpham_label = new System.Windows.Forms.Label();
            this.soluongText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 167);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(864, 378);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Sans Serif Collection", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 32);
            this.label3.TabIndex = 17;
            this.label3.Text = "Sản phẩm:";
            // 
            // btnThemGioHang
            // 
            this.btnThemGioHang.Font = new System.Drawing.Font("Sans Serif Collection", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemGioHang.Location = new System.Drawing.Point(682, 114);
            this.btnThemGioHang.Margin = new System.Windows.Forms.Padding(2);
            this.btnThemGioHang.Name = "btnThemGioHang";
            this.btnThemGioHang.Size = new System.Drawing.Size(130, 35);
            this.btnThemGioHang.TabIndex = 18;
            this.btnThemGioHang.Text = "Thêm vào giỏ hàng";
            this.btnThemGioHang.UseVisualStyleBackColor = true;
            this.btnThemGioHang.Click += new System.EventHandler(this.btnThemGioHang_Click);
            // 
            // tensanpham_label
            // 
            this.tensanpham_label.AutoSize = true;
            this.tensanpham_label.Font = new System.Drawing.Font("Sans Serif Collection", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tensanpham_label.Location = new System.Drawing.Point(88, 79);
            this.tensanpham_label.Name = "tensanpham_label";
            this.tensanpham_label.Size = new System.Drawing.Size(148, 32);
            this.tensanpham_label.TabIndex = 19;
            this.tensanpham_label.Text = "Tên sản phẩm:";
            // 
            // soluongText
            // 
            this.soluongText.Location = new System.Drawing.Point(260, 86);
            this.soluongText.Margin = new System.Windows.Forms.Padding(2);
            this.soluongText.Name = "soluongText";
            this.soluongText.Size = new System.Drawing.Size(357, 20);
            this.soluongText.TabIndex = 20;
            // 
            // UC_HangHoa_Khach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.soluongText);
            this.Controls.Add(this.tensanpham_label);
            this.Controls.Add(this.btnThemGioHang);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UC_HangHoa_Khach";
            this.Size = new System.Drawing.Size(864, 547);
            this.Load += new System.EventHandler(this.UC_HangHoa_Khach_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView dataGridView1;
        private Label label3;
        private Button btnThemGioHang;
        private Label tensanpham_label;
        private TextBox soluongText;
    }
}