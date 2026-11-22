namespace LAB04.Bai07
{
    partial class Bai07
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlTieuDe = new System.Windows.Forms.Panel();
            this.btnThemMonAn = new System.Windows.Forms.Button();
            this.btnAnGiGio = new System.Windows.Forms.Button();
            this.lblTieuDeChinh = new System.Windows.Forms.Label();
            this.tabChinh = new System.Windows.Forms.TabControl();
            this.tabTatCa = new System.Windows.Forms.TabPage();
            this.flpTatCa = new System.Windows.Forms.FlowLayoutPanel();
            this.tabToiDongGop = new System.Windows.Forms.TabPage();
            this.flpToiDongGop = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlPhanTrang = new System.Windows.Forms.Panel();
            this.lblThongTinTrang = new System.Windows.Forms.Label();
            this.cboKichThuocTrang = new System.Windows.Forms.ComboBox();
            this.lblKichThuocTrang = new System.Windows.Forms.Label();
            this.cboTrang = new System.Windows.Forms.ComboBox();
            this.lblTrang = new System.Windows.Forms.Label();
            this.statusStripChinh = new System.Windows.Forms.StatusStrip();
            this.lblTrangThai = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblNutTaiKhoan = new System.Windows.Forms.ToolStripStatusLabel();
            this.prgTaiDuLieu = new System.Windows.Forms.ToolStripProgressBar();
            this.pnlTieuDe.SuspendLayout();
            this.tabChinh.SuspendLayout();
            this.tabTatCa.SuspendLayout();
            this.tabToiDongGop.SuspendLayout();
            this.pnlPhanTrang.SuspendLayout();
            this.statusStripChinh.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTieuDe
            // 
            this.pnlTieuDe.BackColor = System.Drawing.Color.White;
            this.pnlTieuDe.Controls.Add(this.btnThemMonAn);
            this.pnlTieuDe.Controls.Add(this.btnAnGiGio);
            this.pnlTieuDe.Controls.Add(this.lblTieuDeChinh);
            this.pnlTieuDe.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTieuDe.Location = new System.Drawing.Point(0, 0);
            this.pnlTieuDe.Name = "pnlTieuDe";
            this.pnlTieuDe.Size = new System.Drawing.Size(764, 72);
            this.pnlTieuDe.TabIndex = 0;
            // 
            // btnThemMonAn
            // 
            this.btnThemMonAn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThemMonAn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnThemMonAn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemMonAn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(149)))), ((int)(((byte)(237)))));
            this.btnThemMonAn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemMonAn.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemMonAn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.btnThemMonAn.Location = new System.Drawing.Point(584, 15);
            this.btnThemMonAn.Name = "btnThemMonAn";
            this.btnThemMonAn.Size = new System.Drawing.Size(160, 44);
            this.btnThemMonAn.TabIndex = 2;
            this.btnThemMonAn.Text = "Thêm món ăn";
            this.btnThemMonAn.UseVisualStyleBackColor = false;
            // 
            // btnAnGiGio
            // 
            this.btnAnGiGio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnGiGio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(205)))));
            this.btnAnGiGio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnGiGio.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnAnGiGio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnGiGio.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnGiGio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.btnAnGiGio.Location = new System.Drawing.Point(412, 15);
            this.btnAnGiGio.Name = "btnAnGiGio";
            this.btnAnGiGio.Size = new System.Drawing.Size(160, 44);
            this.btnAnGiGio.TabIndex = 1;
            this.btnAnGiGio.Text = "Ăn gì giờ?";
            this.btnAnGiGio.UseVisualStyleBackColor = false;
            // 
            // lblTieuDeChinh
            // 
            this.lblTieuDeChinh.AutoSize = true;
            this.lblTieuDeChinh.Font = new System.Drawing.Font("Segoe UI", 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDeChinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(83)))), ((int)(((byte)(79)))));
            this.lblTieuDeChinh.Location = new System.Drawing.Point(16, 17);
            this.lblTieuDeChinh.Name = "lblTieuDeChinh";
            this.lblTieuDeChinh.Size = new System.Drawing.Size(256, 38);
            this.lblTieuDeChinh.TabIndex = 0;
            this.lblTieuDeChinh.Text = "HÔM NAY ĂN GÌ?";
            // 
            // tabChinh
            // 
            this.tabChinh.Controls.Add(this.tabTatCa);
            this.tabChinh.Controls.Add(this.tabToiDongGop);
            this.tabChinh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabChinh.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabChinh.Location = new System.Drawing.Point(0, 72);
            this.tabChinh.Name = "tabChinh";
            this.tabChinh.Padding = new System.Drawing.Point(14, 5);
            this.tabChinh.SelectedIndex = 0;
            this.tabChinh.Size = new System.Drawing.Size(764, 540);
            this.tabChinh.TabIndex = 1;
            // 
            // tabTatCa
            // 
            this.tabTatCa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tabTatCa.Controls.Add(this.flpTatCa);
            this.tabTatCa.Location = new System.Drawing.Point(4, 30);
            this.tabTatCa.Name = "tabTatCa";
            this.tabTatCa.Padding = new System.Windows.Forms.Padding(6);
            this.tabTatCa.Size = new System.Drawing.Size(756, 506);
            this.tabTatCa.TabIndex = 0;
            this.tabTatCa.Text = "Tất cả";
            // 
            // flpTatCa
            // 
            this.flpTatCa.AutoScroll = true;
            this.flpTatCa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.flpTatCa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTatCa.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpTatCa.Location = new System.Drawing.Point(6, 6);
            this.flpTatCa.Name = "flpTatCa";
            this.flpTatCa.Size = new System.Drawing.Size(744, 494);
            this.flpTatCa.TabIndex = 0;
            this.flpTatCa.WrapContents = false;
            // 
            // tabToiDongGop
            // 
            this.tabToiDongGop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tabToiDongGop.Controls.Add(this.flpToiDongGop);
            this.tabToiDongGop.Location = new System.Drawing.Point(4, 30);
            this.tabToiDongGop.Name = "tabToiDongGop";
            this.tabToiDongGop.Padding = new System.Windows.Forms.Padding(6);
            this.tabToiDongGop.Size = new System.Drawing.Size(756, 506);
            this.tabToiDongGop.TabIndex = 1;
            this.tabToiDongGop.Text = "Tôi đóng góp";
            // 
            // flpToiDongGop
            // 
            this.flpToiDongGop.AutoScroll = true;
            this.flpToiDongGop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.flpToiDongGop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpToiDongGop.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpToiDongGop.Location = new System.Drawing.Point(6, 6);
            this.flpToiDongGop.Name = "flpToiDongGop";
            this.flpToiDongGop.Size = new System.Drawing.Size(744, 494);
            this.flpToiDongGop.TabIndex = 0;
            this.flpToiDongGop.WrapContents = false;
            // 
            // pnlPhanTrang
            // 
            this.pnlPhanTrang.BackColor = System.Drawing.Color.White;
            this.pnlPhanTrang.Controls.Add(this.lblThongTinTrang);
            this.pnlPhanTrang.Controls.Add(this.cboKichThuocTrang);
            this.pnlPhanTrang.Controls.Add(this.lblKichThuocTrang);
            this.pnlPhanTrang.Controls.Add(this.cboTrang);
            this.pnlPhanTrang.Controls.Add(this.lblTrang);
            this.pnlPhanTrang.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPhanTrang.Location = new System.Drawing.Point(0, 612);
            this.pnlPhanTrang.Name = "pnlPhanTrang";
            this.pnlPhanTrang.Size = new System.Drawing.Size(764, 42);
            this.pnlPhanTrang.TabIndex = 2;
            // 
            // lblThongTinTrang
            // 
            this.lblThongTinTrang.AutoSize = true;
            this.lblThongTinTrang.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThongTinTrang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.lblThongTinTrang.Location = new System.Drawing.Point(20, 12);
            this.lblThongTinTrang.Name = "lblThongTinTrang";
            this.lblThongTinTrang.Size = new System.Drawing.Size(156, 17);
            this.lblThongTinTrang.TabIndex = 4;
            this.lblThongTinTrang.Text = "Tổng cộng: 0 món ăn";
            // 
            // cboKichThuocTrang
            // 
            this.cboKichThuocTrang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboKichThuocTrang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKichThuocTrang.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboKichThuocTrang.FormattingEnabled = true;
            this.cboKichThuocTrang.Items.AddRange(new object[] {
            "5",
            "10",
            "20"});
            this.cboKichThuocTrang.Location = new System.Drawing.Point(676, 8);
            this.cboKichThuocTrang.Name = "cboKichThuocTrang";
            this.cboKichThuocTrang.Size = new System.Drawing.Size(68, 25);
            this.cboKichThuocTrang.TabIndex = 3;
            // 
            // lblKichThuocTrang
            // 
            this.lblKichThuocTrang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKichThuocTrang.AutoSize = true;
            this.lblKichThuocTrang.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKichThuocTrang.Location = new System.Drawing.Point(562, 12);
            this.lblKichThuocTrang.Name = "lblKichThuocTrang";
            this.lblKichThuocTrang.Size = new System.Drawing.Size(108, 17);
            this.lblKichThuocTrang.TabIndex = 2;
            this.lblKichThuocTrang.Text = "Kích thước trang:";
            // 
            // cboTrang
            // 
            this.cboTrang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTrang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrang.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTrang.FormattingEnabled = true;
            this.cboTrang.Items.AddRange(new object[] {
            "1"});
            this.cboTrang.Location = new System.Drawing.Point(482, 8);
            this.cboTrang.Name = "cboTrang";
            this.cboTrang.Size = new System.Drawing.Size(68, 25);
            this.cboTrang.TabIndex = 1;
            // 
            // lblTrang
            // 
            this.lblTrang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrang.AutoSize = true;
            this.lblTrang.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrang.Location = new System.Drawing.Point(436, 12);
            this.lblTrang.Name = "lblTrang";
            this.lblTrang.Size = new System.Drawing.Size(42, 17);
            this.lblTrang.TabIndex = 0;
            this.lblTrang.Text = "Trang:";
            // 
            // statusStripChinh
            // 
            this.statusStripChinh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(243)))), ((int)(((byte)(245)))));
            this.statusStripChinh.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTrangThai,
            this.lblNutTaiKhoan,
            this.prgTaiDuLieu});
            this.statusStripChinh.Location = new System.Drawing.Point(0, 654);
            this.statusStripChinh.Name = "statusStripChinh";
            this.statusStripChinh.Size = new System.Drawing.Size(764, 27);
            this.statusStripChinh.SizingGrip = false;
            this.statusStripChinh.TabIndex = 3;
            this.statusStripChinh.Text = "Thanh trạng thái";
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrangThai.ForeColor = System.Drawing.Color.Firebrick;
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(112, 22);
            this.lblTrangThai.Text = "Chưa đăng nhập";
            // 
            // lblNutTaiKhoan
            // 
            this.lblNutTaiKhoan.ActiveLinkColor = System.Drawing.Color.DarkBlue;
            this.lblNutTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNutTaiKhoan.IsLink = true;
            this.lblNutTaiKhoan.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblNutTaiKhoan.LinkColor = System.Drawing.Color.RoyalBlue;
            this.lblNutTaiKhoan.Name = "lblNutTaiKhoan";
            this.lblNutTaiKhoan.Size = new System.Drawing.Size(70, 22);
            this.lblNutTaiKhoan.Text = "Đăng nhập";
            // 
            // prgTaiDuLieu
            // 
            this.prgTaiDuLieu.Name = "prgTaiDuLieu";
            this.prgTaiDuLieu.Size = new System.Drawing.Size(180, 21);
            this.prgTaiDuLieu.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // bai07
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(764, 681);
            this.Controls.Add(this.tabChinh);
            this.Controls.Add(this.pnlPhanTrang);
            this.Controls.Add(this.pnlTieuDe);
            this.Controls.Add(this.statusStripChinh);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "bai07";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hôm nay ăn gì?";
            this.pnlTieuDe.ResumeLayout(false);
            this.pnlTieuDe.PerformLayout();
            this.tabChinh.ResumeLayout(false);
            this.tabTatCa.ResumeLayout(false);
            this.tabToiDongGop.ResumeLayout(false);
            this.pnlPhanTrang.ResumeLayout(false);
            this.pnlPhanTrang.PerformLayout();
            this.statusStripChinh.ResumeLayout(false);
            this.statusStripChinh.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTieuDe;
        private System.Windows.Forms.Label lblTieuDeChinh;
        private System.Windows.Forms.Button btnAnGiGio;
        private System.Windows.Forms.Button btnThemMonAn;
        private System.Windows.Forms.TabControl tabChinh;
        private System.Windows.Forms.TabPage tabTatCa;
        private System.Windows.Forms.FlowLayoutPanel flpTatCa;
        private System.Windows.Forms.TabPage tabToiDongGop;
        private System.Windows.Forms.FlowLayoutPanel flpToiDongGop;
        private System.Windows.Forms.Panel pnlPhanTrang;
        private System.Windows.Forms.Label lblTrang;
        private System.Windows.Forms.ComboBox cboTrang;
        private System.Windows.Forms.Label lblKichThuocTrang;
        private System.Windows.Forms.ComboBox cboKichThuocTrang;
        private System.Windows.Forms.Label lblThongTinTrang;
        private System.Windows.Forms.StatusStrip statusStripChinh;
        private System.Windows.Forms.ToolStripStatusLabel lblTrangThai;
        private System.Windows.Forms.ToolStripStatusLabel lblNutTaiKhoan;
        private System.Windows.Forms.ToolStripProgressBar prgTaiDuLieu;
    }
}
