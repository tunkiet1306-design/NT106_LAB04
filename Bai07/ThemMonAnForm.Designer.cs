namespace LAB04.Bai07;

partial class ThemMonAnForm
{
    private System.ComponentModel.IContainer? components = null;

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
        lblTitle = new Label();
        lblTenMonAn = new Label();
        txtTenMonAn = new TextBox();
        lblGia = new Label();
        txtGia = new TextBox();
        lblDiaChi = new Label();
        txtDiaChi = new TextBox();
        lblHinhAnh = new Label();
        txtHinhAnh = new TextBox();
        lblMoTa = new Label();
        txtMoTa = new TextBox();
        btnClear = new Button();
        btnThem = new Button();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(180, 52, 43);
        lblTitle.Location = new Point(20, 16);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(340, 36);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Thêm Món Ăn";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTenMonAn
        // 
        lblTenMonAn.AutoSize = true;
        lblTenMonAn.Location = new Point(24, 72);
        lblTenMonAn.Name = "lblTenMonAn";
        lblTenMonAn.Size = new Size(73, 15);
        lblTenMonAn.TabIndex = 1;
        lblTenMonAn.Text = "Tên món ăn:";
        // 
        // txtTenMonAn
        // 
        txtTenMonAn.Location = new Point(112, 69);
        txtTenMonAn.Name = "txtTenMonAn";
        txtTenMonAn.Size = new Size(240, 23);
        txtTenMonAn.TabIndex = 2;
        // 
        // lblGia
        // 
        lblGia.AutoSize = true;
        lblGia.Location = new Point(24, 106);
        lblGia.Name = "lblGia";
        lblGia.Size = new Size(27, 15);
        lblGia.TabIndex = 3;
        lblGia.Text = "Giá:";
        // 
        // txtGia
        // 
        txtGia.Location = new Point(112, 103);
        txtGia.Name = "txtGia";
        txtGia.Size = new Size(240, 23);
        txtGia.TabIndex = 4;
        // 
        // lblDiaChi
        // 
        lblDiaChi.AutoSize = true;
        lblDiaChi.Location = new Point(24, 140);
        lblDiaChi.Name = "lblDiaChi";
        lblDiaChi.Size = new Size(46, 15);
        lblDiaChi.TabIndex = 5;
        lblDiaChi.Text = "Địa chỉ:";
        // 
        // txtDiaChi
        // 
        txtDiaChi.Location = new Point(112, 137);
        txtDiaChi.Name = "txtDiaChi";
        txtDiaChi.Size = new Size(240, 23);
        txtDiaChi.TabIndex = 6;
        // 
        // lblHinhAnh
        // 
        lblHinhAnh.AutoSize = true;
        lblHinhAnh.Location = new Point(24, 174);
        lblHinhAnh.Name = "lblHinhAnh";
        lblHinhAnh.Size = new Size(58, 15);
        lblHinhAnh.TabIndex = 7;
        lblHinhAnh.Text = "Hình ảnh:";
        // 
        // txtHinhAnh
        // 
        txtHinhAnh.Location = new Point(112, 171);
        txtHinhAnh.Name = "txtHinhAnh";
        txtHinhAnh.PlaceholderText = "URL ảnh (tùy chọn)";
        txtHinhAnh.Size = new Size(240, 23);
        txtHinhAnh.TabIndex = 8;
        // 
        // lblMoTa
        // 
        lblMoTa.AutoSize = true;
        lblMoTa.Location = new Point(24, 208);
        lblMoTa.Name = "lblMoTa";
        lblMoTa.Size = new Size(40, 15);
        lblMoTa.TabIndex = 9;
        lblMoTa.Text = "Mô tả:";
        // 
        // txtMoTa
        // 
        txtMoTa.Location = new Point(112, 205);
        txtMoTa.Multiline = true;
        txtMoTa.Name = "txtMoTa";
        txtMoTa.ScrollBars = ScrollBars.Vertical;
        txtMoTa.Size = new Size(240, 84);
        txtMoTa.TabIndex = 10;
        // 
        // btnClear
        // 
        btnClear.Location = new Point(170, 308);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(86, 30);
        btnClear.TabIndex = 11;
        btnClear.Text = "Clear";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;
        // 
        // btnThem
        // 
        btnThem.Location = new Point(266, 308);
        btnThem.Name = "btnThem";
        btnThem.Size = new Size(86, 30);
        btnThem.TabIndex = 12;
        btnThem.Text = "Thêm";
        btnThem.UseVisualStyleBackColor = true;
        btnThem.Click += btnThem_Click;
        // 
        // ThemMonAnForm
        // 
        AcceptButton = btnThem;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(380, 358);
        Controls.Add(btnThem);
        Controls.Add(btnClear);
        Controls.Add(txtMoTa);
        Controls.Add(lblMoTa);
        Controls.Add(txtHinhAnh);
        Controls.Add(lblHinhAnh);
        Controls.Add(txtDiaChi);
        Controls.Add(lblDiaChi);
        Controls.Add(txtGia);
        Controls.Add(lblGia);
        Controls.Add(txtTenMonAn);
        Controls.Add(lblTenMonAn);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ThemMonAnForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "ThemMonAn";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTitle = null!;
    private Label lblTenMonAn = null!;
    private TextBox txtTenMonAn = null!;
    private Label lblGia = null!;
    private TextBox txtGia = null!;
    private Label lblDiaChi = null!;
    private TextBox txtDiaChi = null!;
    private Label lblHinhAnh = null!;
    private TextBox txtHinhAnh = null!;
    private Label lblMoTa = null!;
    private TextBox txtMoTa = null!;
    private Button btnClear = null!;
    private Button btnThem = null!;
}
