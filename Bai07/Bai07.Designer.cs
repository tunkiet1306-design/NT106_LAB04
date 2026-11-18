#nullable disable

namespace LAB04.Bai07
{
    partial class Bai07
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.bthSearch = new System.Windows.Forms.Button();
            this.bthRandom = new System.Windows.Forms.Button();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabAll = new System.Windows.Forms.TabPage();
            this.dgvAllDishes = new System.Windows.Forms.DataGridView();
            this.tabMine = new System.Windows.Forms.TabPage();
            this.dgvMyDishes = new System.Windows.Forms.DataGridView();
            this.grpManageDish = new System.Windows.Forms.GroupBox();
            this.lblTenMon = new System.Windows.Forms.Label();
            this.txtTenMon = new System.Windows.Forms.TextBox();
            this.lblGia = new System.Windows.Forms.Label();
            this.txtGia = new System.Windows.Forms.TextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.lblHinhAnh = new System.Windows.Forms.Label();
            this.txtHinhAnh = new System.Windows.Forms.TextBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.bthThemMon = new System.Windows.Forms.Button();
            this.bthXoaMon = new System.Windows.Forms.Button();
            this.tabAccount = new System.Windows.Forms.TabPage();
            this.grpAuth = new System.Windows.Forms.GroupBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.bthLogin = new System.Windows.Forms.Button();
            this.bthSignUp = new System.Windows.Forms.Button();
            this.bthLogout = new System.Windows.Forms.Button();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.picDish = new System.Windows.Forms.PictureBox();
            this.lblDetailHeader = new System.Windows.Forms.Label();
            this.lblDetailName = new System.Windows.Forms.Label();
            this.lblDetailMeta = new System.Windows.Forms.Label();
            this.txtDetailDesc = new System.Windows.Forms.TextBox();
            this.pnlPagination = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.bthPrevPage = new System.Windows.Forms.Button();
            this.lblPage = new System.Windows.Forms.Label();
            this.cmbPage = new System.Windows.Forms.ComboBox();
            this.bthNextPage = new System.Windows.Forms.Button();
            this.lblPageSize = new System.Windows.Forms.Label();
            this.cmbPageSize = new System.Windows.Forms.ComboBox();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblAuthStatus = new System.Windows.Forms.Label();
            this.prgLoading = new System.Windows.Forms.ProgressBar();
            this.pnlTop.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabAll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllDishes)).BeginInit();
            this.tabMine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyDishes)).BeginInit();
            this.grpManageDish.SuspendLayout();
            this.tabAccount.SuspendLayout();
            this.grpAuth.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDish)).BeginInit();
            this.pnlPagination.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlTop.Controls.Add(this.bthRandom);
            this.pnlTop.Controls.Add(this.bthSearch);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(960, 58);
            this.pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(180, 52, 43);
            this.lblTitle.Location = new System.Drawing.Point(16, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(192, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "HÔM NAY ĂN GÌ?";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(250, 17);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm món ăn từ TheMealDB (vd: Chicken, Beef, Soup...)";
            this.txtSearch.Size = new System.Drawing.Size(340, 23);
            this.txtSearch.TabIndex = 1;
            // 
            // bthSearch
            // 
            this.bthSearch.Location = new System.Drawing.Point(598, 15);
            this.bthSearch.Name = "bthSearch";
            this.bthSearch.Size = new System.Drawing.Size(110, 27);
            this.bthSearch.TabIndex = 2;
            this.bthSearch.Text = "Tải / Tìm kiếm";
            this.bthSearch.UseVisualStyleBackColor = true;
            this.bthSearch.Click += new System.EventHandler(this.bthSearch_Click);
            // 
            // bthRandom
            // 
            this.bthRandom.BackColor = System.Drawing.Color.FromArgb(245, 194, 120);
            this.bthRandom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bthRandom.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.bthRandom.Location = new System.Drawing.Point(810, 12);
            this.bthRandom.Name = "bthRandom";
            this.bthRandom.Size = new System.Drawing.Size(134, 33);
            this.bthRandom.TabIndex = 3;
            this.bthRandom.Text = "Ăn gì giờ?";
            this.bthRandom.UseVisualStyleBackColor = false;
            this.bthRandom.Click += new System.EventHandler(this.bthRandom_Click);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabAll);
            this.tabMain.Controls.Add(this.tabMine);
            this.tabMain.Controls.Add(this.tabAccount);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 58);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(640, 442);
            this.tabMain.TabIndex = 1;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tabAll
            // 
            this.tabAll.Controls.Add(this.dgvAllDishes);
            this.tabAll.Location = new System.Drawing.Point(4, 24);
            this.tabAll.Name = "tabAll";
            this.tabAll.Padding = new System.Windows.Forms.Padding(6);
            this.tabAll.Size = new System.Drawing.Size(632, 414);
            this.tabAll.TabIndex = 0;
            this.tabAll.Text = "All (Public API)";
            this.tabAll.UseVisualStyleBackColor = true;
            // 
            // dgvAllDishes
            // 
            this.dgvAllDishes.AllowUserToAddRows = false;
            this.dgvAllDishes.AllowUserToDeleteRows = false;
            this.dgvAllDishes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAllDishes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllDishes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAllDishes.Location = new System.Drawing.Point(6, 6);
            this.dgvAllDishes.MultiSelect = false;
            this.dgvAllDishes.Name = "dgvAllDishes";
            this.dgvAllDishes.ReadOnly = true;
            this.dgvAllDishes.RowHeadersVisible = false;
            this.dgvAllDishes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAllDishes.Size = new System.Drawing.Size(620, 402);
            this.dgvAllDishes.TabIndex = 0;
            this.dgvAllDishes.SelectionChanged += new System.EventHandler(this.dgvAllDishes_SelectionChanged);
            // 
            // tabMine
            // 
            this.tabMine.Controls.Add(this.dgvMyDishes);
            this.tabMine.Controls.Add(this.grpManageDish);
            this.tabMine.Location = new System.Drawing.Point(4, 24);
            this.tabMine.Name = "tabMine";
            this.tabMine.Padding = new System.Windows.Forms.Padding(6);
            this.tabMine.Size = new System.Drawing.Size(632, 414);
            this.tabMine.TabIndex = 1;
            this.tabMine.Text = "Tôi đóng góp";
            this.tabMine.UseVisualStyleBackColor = true;
            // 
            // dgvMyDishes
            // 
            this.dgvMyDishes.AllowUserToAddRows = false;
            this.dgvMyDishes.AllowUserToDeleteRows = false;
            this.dgvMyDishes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMyDishes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMyDishes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMyDishes.Location = new System.Drawing.Point(6, 6);
            this.dgvMyDishes.MultiSelect = false;
            this.dgvMyDishes.Name = "dgvMyDishes";
            this.dgvMyDishes.ReadOnly = true;
            this.dgvMyDishes.RowHeadersVisible = false;
            this.dgvMyDishes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMyDishes.Size = new System.Drawing.Size(620, 232);
            this.dgvMyDishes.TabIndex = 0;
            this.dgvMyDishes.SelectionChanged += new System.EventHandler(this.dgvMyDishes_SelectionChanged);
            // 
            // grpManageDish
            // 
            this.grpManageDish.Controls.Add(this.bthXoaMon);
            this.grpManageDish.Controls.Add(this.bthThemMon);
            this.grpManageDish.Controls.Add(this.txtMoTa);
            this.grpManageDish.Controls.Add(this.lblMoTa);
            this.grpManageDish.Controls.Add(this.txtHinhAnh);
            this.grpManageDish.Controls.Add(this.lblHinhAnh);
            this.grpManageDish.Controls.Add(this.txtDiaChi);
            this.grpManageDish.Controls.Add(this.lblDiaChi);
            this.grpManageDish.Controls.Add(this.txtGia);
            this.grpManageDish.Controls.Add(this.lblGia);
            this.grpManageDish.Controls.Add(this.txtTenMon);
            this.grpManageDish.Controls.Add(this.lblTenMon);
            this.grpManageDish.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpManageDish.Location = new System.Drawing.Point(6, 238);
            this.grpManageDish.Name = "grpManageDish";
            this.grpManageDish.Size = new System.Drawing.Size(620, 170);
            this.grpManageDish.TabIndex = 1;
            this.grpManageDish.TabStop = false;
            this.grpManageDish.Text = "Thêm / Xóa món ăn của tôi";
            // 
            // lblTenMon
            // 
            this.lblTenMon.AutoSize = true;
            this.lblTenMon.Location = new System.Drawing.Point(12, 26);
            this.lblTenMon.Name = "lblTenMon";
            this.lblTenMon.Size = new System.Drawing.Size(70, 15);
            this.lblTenMon.TabIndex = 0;
            this.lblTenMon.Text = "Tên món ăn";
            // 
            // txtTenMon
            // 
            this.txtTenMon.Location = new System.Drawing.Point(92, 23);
            this.txtTenMon.Name = "txtTenMon";
            this.txtTenMon.Size = new System.Drawing.Size(200, 23);
            this.txtTenMon.TabIndex = 1;
            // 
            // lblGia
            // 
            this.lblGia.AutoSize = true;
            this.lblGia.Location = new System.Drawing.Point(312, 26);
            this.lblGia.Name = "lblGia";
            this.lblGia.Size = new System.Drawing.Size(58, 15);
            this.lblGia.TabIndex = 2;
            this.lblGia.Text = "Giá (VNĐ)";
            // 
            // txtGia
            // 
            this.txtGia.Location = new System.Drawing.Point(382, 23);
            this.txtGia.Name = "txtGia";
            this.txtGia.Size = new System.Drawing.Size(222, 23);
            this.txtGia.TabIndex = 3;
            // 
            // lblDiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Location = new System.Drawing.Point(12, 58);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(43, 15);
            this.lblDiaChi.TabIndex = 4;
            this.lblDiaChi.Text = "Địa chỉ";
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Location = new System.Drawing.Point(92, 55);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(200, 23);
            this.txtDiaChi.TabIndex = 5;
            // 
            // lblHinhAnh
            // 
            this.lblHinhAnh.AutoSize = true;
            this.lblHinhAnh.Location = new System.Drawing.Point(312, 58);
            this.lblHinhAnh.Name = "lblHinhAnh";
            this.lblHinhAnh.Size = new System.Drawing.Size(61, 15);
            this.lblHinhAnh.TabIndex = 6;
            this.lblHinhAnh.Text = "URL Ảnh";
            // 
            // txtHinhAnh
            // 
            this.txtHinhAnh.Location = new System.Drawing.Point(382, 55);
            this.txtHinhAnh.Name = "txtHinhAnh";
            this.txtHinhAnh.Size = new System.Drawing.Size(222, 23);
            this.txtHinhAnh.TabIndex = 7;
            // 
            // lblMoTa
            // 
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Location = new System.Drawing.Point(12, 92);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(37, 15);
            this.lblMoTa.TabIndex = 8;
            this.lblMoTa.Text = "Mô tả";
            // 
            // txtMoTa
            // 
            this.txtMoTa.Location = new System.Drawing.Point(92, 88);
            this.txtMoTa.Multiline = true;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.Size = new System.Drawing.Size(512, 38);
            this.txtMoTa.TabIndex = 9;
            // 
            // bthThemMon
            // 
            this.bthThemMon.Location = new System.Drawing.Point(412, 134);
            this.bthThemMon.Name = "bthThemMon";
            this.bthThemMon.Size = new System.Drawing.Size(92, 28);
            this.bthThemMon.TabIndex = 10;
            this.bthThemMon.Text = "Thêm món";
            this.bthThemMon.UseVisualStyleBackColor = true;
            this.bthThemMon.Click += new System.EventHandler(this.bthThemMon_Click);
            // 
            // bthXoaMon
            // 
            this.bthXoaMon.Location = new System.Drawing.Point(512, 134);
            this.bthXoaMon.Name = "bthXoaMon";
            this.bthXoaMon.Size = new System.Drawing.Size(92, 28);
            this.bthXoaMon.TabIndex = 11;
            this.bthXoaMon.Text = "Xóa món chọn";
            this.bthXoaMon.UseVisualStyleBackColor = true;
            this.bthXoaMon.Click += new System.EventHandler(this.bthXoaMon_Click);
            // 
            // tabAccount
            // 
            this.tabAccount.Controls.Add(this.grpAuth);
            this.tabAccount.Location = new System.Drawing.Point(4, 24);
            this.tabAccount.Name = "tabAccount";
            this.tabAccount.Padding = new System.Windows.Forms.Padding(12);
            this.tabAccount.Size = new System.Drawing.Size(632, 414);
            this.tabAccount.TabIndex = 2;
            this.tabAccount.Text = "Tài khoản";
            this.tabAccount.UseVisualStyleBackColor = true;
            // 
            // grpAuth
            // 
            this.grpAuth.Controls.Add(this.bthLogout);
            this.grpAuth.Controls.Add(this.bthSignUp);
            this.grpAuth.Controls.Add(this.bthLogin);
            this.grpAuth.Controls.Add(this.txtFullName);
            this.grpAuth.Controls.Add(this.lblFullName);
            this.grpAuth.Controls.Add(this.txtEmail);
            this.grpAuth.Controls.Add(this.lblEmail);
            this.grpAuth.Controls.Add(this.txtPassword);
            this.grpAuth.Controls.Add(this.lblPassword);
            this.grpAuth.Controls.Add(this.txtUsername);
            this.grpAuth.Controls.Add(this.lblUsername);
            this.grpAuth.Location = new System.Drawing.Point(16, 16);
            this.grpAuth.Name = "grpAuth";
            this.grpAuth.Size = new System.Drawing.Size(440, 230);
            this.grpAuth.TabIndex = 0;
            this.grpAuth.TabStop = false;
            this.grpAuth.Text = "Đăng nhập / Đăng ký tài khoản";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(20, 34);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 15);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(110, 31);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(290, 23);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.Text = "demo";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(20, 68);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(110, 65);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(290, 23);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Text = "123456";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(20, 102);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(36, 15);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(110, 99);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(290, 23);
            this.txtEmail.TabIndex = 5;
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(20, 136);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(51, 15);
            this.lblFullName.TabIndex = 6;
            this.lblFullName.Text = "Họ và tên";
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(110, 133);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(290, 23);
            this.txtFullName.TabIndex = 7;
            // 
            // bthLogin
            // 
            this.bthLogin.Location = new System.Drawing.Point(110, 174);
            this.bthLogin.Name = "bthLogin";
            this.bthLogin.Size = new System.Drawing.Size(90, 30);
            this.bthLogin.TabIndex = 8;
            this.bthLogin.Text = "Đăng nhập";
            this.bthLogin.UseVisualStyleBackColor = true;
            this.bthLogin.Click += new System.EventHandler(this.bthLogin_Click);
            // 
            // bthSignUp
            // 
            this.bthSignUp.Location = new System.Drawing.Point(210, 174);
            this.bthSignUp.Name = "bthSignUp";
            this.bthSignUp.Size = new System.Drawing.Size(90, 30);
            this.bthSignUp.TabIndex = 9;
            this.bthSignUp.Text = "Đăng ký";
            this.bthSignUp.UseVisualStyleBackColor = true;
            this.bthSignUp.Click += new System.EventHandler(this.bthSignUp_Click);
            // 
            // bthLogout
            // 
            this.bthLogout.Location = new System.Drawing.Point(310, 174);
            this.bthLogout.Name = "bthLogout";
            this.bthLogout.Size = new System.Drawing.Size(90, 30);
            this.bthLogout.TabIndex = 10;
            this.bthLogout.Text = "Đăng xuất";
            this.bthLogout.UseVisualStyleBackColor = true;
            this.bthLogout.Click += new System.EventHandler(this.bthLogout_Click);
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.Color.White;
            this.pnlDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetail.Controls.Add(this.txtDetailDesc);
            this.pnlDetail.Controls.Add(this.lblDetailMeta);
            this.pnlDetail.Controls.Add(this.lblDetailName);
            this.pnlDetail.Controls.Add(this.lblDetailHeader);
            this.pnlDetail.Controls.Add(this.picDish);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetail.Location = new System.Drawing.Point(640, 58);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Padding = new System.Windows.Forms.Padding(12);
            this.pnlDetail.Size = new System.Drawing.Size(320, 442);
            this.pnlDetail.TabIndex = 2;
            // 
            // picDish
            // 
            this.picDish.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.picDish.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picDish.Location = new System.Drawing.Point(14, 42);
            this.picDish.Name = "picDish";
            this.picDish.Size = new System.Drawing.Size(290, 180);
            this.picDish.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDish.TabIndex = 0;
            this.picDish.TabStop = false;
            // 
            // lblDetailHeader
            // 
            this.lblDetailHeader.AutoSize = true;
            this.lblDetailHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDetailHeader.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblDetailHeader.Location = new System.Drawing.Point(14, 12);
            this.lblDetailHeader.Name = "lblDetailHeader";
            this.lblDetailHeader.Size = new System.Drawing.Size(116, 19);
            this.lblDetailHeader.TabIndex = 1;
            this.lblDetailHeader.Text = "Chi tiết món ăn";
            // 
            // lblDetailName
            // 
            this.lblDetailName.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblDetailName.ForeColor = System.Drawing.Color.FromArgb(180, 52, 43);
            this.lblDetailName.Location = new System.Drawing.Point(14, 230);
            this.lblDetailName.Name = "lblDetailName";
            this.lblDetailName.Size = new System.Drawing.Size(290, 28);
            this.lblDetailName.TabIndex = 2;
            this.lblDetailName.Text = "Chọn một món ăn...";
            // 
            // lblDetailMeta
            // 
            this.lblDetailMeta.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblDetailMeta.Location = new System.Drawing.Point(14, 262);
            this.lblDetailMeta.Name = "lblDetailMeta";
            this.lblDetailMeta.Size = new System.Drawing.Size(290, 42);
            this.lblDetailMeta.TabIndex = 3;
            this.lblDetailMeta.Text = "";
            // 
            // txtDetailDesc
            // 
            this.txtDetailDesc.Location = new System.Drawing.Point(14, 310);
            this.txtDetailDesc.Multiline = true;
            this.txtDetailDesc.Name = "txtDetailDesc";
            this.txtDetailDesc.ReadOnly = true;
            this.txtDetailDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetailDesc.Size = new System.Drawing.Size(290, 118);
            this.txtDetailDesc.TabIndex = 4;
            // 
            // pnlPagination
            // 
            this.pnlPagination.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlPagination.Controls.Add(this.cmbPageSize);
            this.pnlPagination.Controls.Add(this.lblPageSize);
            this.pnlPagination.Controls.Add(this.bthNextPage);
            this.pnlPagination.Controls.Add(this.cmbPage);
            this.pnlPagination.Controls.Add(this.lblPage);
            this.pnlPagination.Controls.Add(this.bthPrevPage);
            this.pnlPagination.Controls.Add(this.lblTotal);
            this.pnlPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPagination.Location = new System.Drawing.Point(0, 500);
            this.pnlPagination.Name = "pnlPagination";
            this.pnlPagination.Size = new System.Drawing.Size(960, 40);
            this.pnlPagination.TabIndex = 3;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblTotal.Location = new System.Drawing.Point(16, 12);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(92, 15);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "Tổng: 0 món ăn";
            // 
            // bthPrevPage
            // 
            this.bthPrevPage.Location = new System.Drawing.Point(596, 7);
            this.bthPrevPage.Name = "bthPrevPage";
            this.bthPrevPage.Size = new System.Drawing.Size(34, 25);
            this.bthPrevPage.TabIndex = 1;
            this.bthPrevPage.Text = "<";
            this.bthPrevPage.UseVisualStyleBackColor = true;
            this.bthPrevPage.Click += new System.EventHandler(this.bthPrevPage_Click);
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Location = new System.Drawing.Point(638, 12);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(33, 15);
            this.lblPage.TabIndex = 2;
            this.lblPage.Text = "Page";
            // 
            // cmbPage
            // 
            this.cmbPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPage.FormattingEnabled = true;
            this.cmbPage.Location = new System.Drawing.Point(676, 8);
            this.cmbPage.Name = "cmbPage";
            this.cmbPage.Size = new System.Drawing.Size(56, 23);
            this.cmbPage.TabIndex = 3;
            this.cmbPage.SelectedIndexChanged += new System.EventHandler(this.cmbPage_SelectedIndexChanged);
            // 
            // bthNextPage
            // 
            this.bthNextPage.Location = new System.Drawing.Point(738, 7);
            this.bthNextPage.Name = "bthNextPage";
            this.bthNextPage.Size = new System.Drawing.Size(34, 25);
            this.bthNextPage.TabIndex = 4;
            this.bthNextPage.Text = ">";
            this.bthNextPage.UseVisualStyleBackColor = true;
            this.bthNextPage.Click += new System.EventHandler(this.bthNextPage_Click);
            // 
            // lblPageSize
            // 
            this.lblPageSize.AutoSize = true;
            this.lblPageSize.Location = new System.Drawing.Point(786, 12);
            this.lblPageSize.Name = "lblPageSize";
            this.lblPageSize.Size = new System.Drawing.Size(55, 15);
            this.lblPageSize.TabIndex = 5;
            this.lblPageSize.Text = "Page size";
            // 
            // cmbPageSize
            // 
            this.cmbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPageSize.FormattingEnabled = true;
            this.cmbPageSize.Items.AddRange(new object[] {
            "5",
            "10",
            "20"});
            this.cmbPageSize.Location = new System.Drawing.Point(848, 8);
            this.cmbPageSize.Name = "cmbPageSize";
            this.cmbPageSize.Size = new System.Drawing.Size(96, 23);
            this.cmbPageSize.TabIndex = 6;
            this.cmbPageSize.SelectedIndexChanged += new System.EventHandler(this.cmbPageSize_SelectedIndexChanged);
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlStatus.Controls.Add(this.prgLoading);
            this.pnlStatus.Controls.Add(this.lblAuthStatus);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Location = new System.Drawing.Point(0, 540);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(960, 30);
            this.pnlStatus.TabIndex = 4;
            // 
            // lblAuthStatus
            // 
            this.lblAuthStatus.AutoSize = true;
            this.lblAuthStatus.ForeColor = System.Drawing.Color.Firebrick;
            this.lblAuthStatus.Location = new System.Drawing.Point(14, 7);
            this.lblAuthStatus.Name = "lblAuthStatus";
            this.lblAuthStatus.Size = new System.Drawing.Size(96, 15);
            this.lblAuthStatus.TabIndex = 0;
            this.lblAuthStatus.Text = "Unauthenticated";
            // 
            // prgLoading
            // 
            this.prgLoading.Location = new System.Drawing.Point(794, 7);
            this.prgLoading.Name = "prgLoading";
            this.prgLoading.Size = new System.Drawing.Size(150, 16);
            this.prgLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.prgLoading.TabIndex = 1;
            this.prgLoading.Visible = false;
            // 
            // Bai07
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 570);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.pnlDetail);
            this.Controls.Add(this.pnlPagination);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(860, 520);
            this.Name = "Bai07";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bài 07 - Hôm nay ăn gì?";
            this.Load += new System.EventHandler(this.Bai07_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabAll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllDishes)).EndInit();
            this.tabMine.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyDishes)).EndInit();
            this.grpManageDish.ResumeLayout(false);
            this.grpManageDish.PerformLayout();
            this.tabAccount.ResumeLayout(false);
            this.grpAuth.ResumeLayout(false);
            this.grpAuth.PerformLayout();
            this.pnlDetail.ResumeLayout(false);
            this.pnlDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDish)).EndInit();
            this.pnlPagination.ResumeLayout(false);
            this.pnlPagination.PerformLayout();
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button bthSearch;
        private System.Windows.Forms.Button bthRandom;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabAll;
        private System.Windows.Forms.DataGridView dgvAllDishes;
        private System.Windows.Forms.TabPage tabMine;
        private System.Windows.Forms.DataGridView dgvMyDishes;
        private System.Windows.Forms.GroupBox grpManageDish;
        private System.Windows.Forms.Label lblTenMon;
        private System.Windows.Forms.TextBox txtTenMon;
        private System.Windows.Forms.Label lblGia;
        private System.Windows.Forms.TextBox txtGia;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblHinhAnh;
        private System.Windows.Forms.TextBox txtHinhAnh;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.Button bthThemMon;
        private System.Windows.Forms.Button bthXoaMon;
        private System.Windows.Forms.TabPage tabAccount;
        private System.Windows.Forms.GroupBox grpAuth;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Button bthLogin;
        private System.Windows.Forms.Button bthSignUp;
        private System.Windows.Forms.Button bthLogout;
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.PictureBox picDish;
        private System.Windows.Forms.Label lblDetailHeader;
        private System.Windows.Forms.Label lblDetailName;
        private System.Windows.Forms.Label lblDetailMeta;
        private System.Windows.Forms.TextBox txtDetailDesc;
        private System.Windows.Forms.Panel pnlPagination;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button bthPrevPage;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.ComboBox cmbPage;
        private System.Windows.Forms.Button bthNextPage;
        private System.Windows.Forms.Label lblPageSize;
        private System.Windows.Forms.ComboBox cmbPageSize;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblAuthStatus;
        private System.Windows.Forms.ProgressBar prgLoading;
    }
}
