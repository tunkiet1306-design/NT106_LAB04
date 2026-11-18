namespace LAB04.Bai07;

partial class DishResultForm
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
        picDish = new PictureBox();
        lblName = new Label();
        lblPrice = new Label();
        lblAddress = new Label();
        lblContributor = new Label();
        lblDescription = new Label();
        btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)picDish).BeginInit();
        SuspendLayout();
        // 
        // picDish
        // 
        picDish.BackColor = Color.FromArgb(241, 245, 249);
        picDish.BorderStyle = BorderStyle.FixedSingle;
        picDish.Location = new Point(16, 16);
        picDish.Name = "picDish";
        picDish.Size = new Size(120, 120);
        picDish.SizeMode = PictureBoxSizeMode.Zoom;
        picDish.TabIndex = 0;
        picDish.TabStop = false;
        // 
        // lblName
        // 
        lblName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblName.ForeColor = Color.FromArgb(180, 52, 43);
        lblName.Location = new Point(148, 16);
        lblName.Name = "lblName";
        lblName.Size = new Size(276, 32);
        lblName.TabIndex = 1;
        lblName.Text = "Món ăn";
        // 
        // lblPrice
        // 
        lblPrice.Font = new Font("Segoe UI", 9.5F);
        lblPrice.Location = new Point(148, 52);
        lblPrice.Name = "lblPrice";
        lblPrice.Size = new Size(276, 22);
        lblPrice.TabIndex = 2;
        lblPrice.Text = "Giá:";
        // 
        // lblAddress
        // 
        lblAddress.Font = new Font("Segoe UI", 9.5F);
        lblAddress.Location = new Point(148, 76);
        lblAddress.Name = "lblAddress";
        lblAddress.Size = new Size(276, 22);
        lblAddress.TabIndex = 3;
        lblAddress.Text = "Địa chỉ:";
        // 
        // lblContributor
        // 
        lblContributor.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblContributor.ForeColor = Color.SeaGreen;
        lblContributor.Location = new Point(148, 100);
        lblContributor.Name = "lblContributor";
        lblContributor.Size = new Size(276, 22);
        lblContributor.TabIndex = 4;
        lblContributor.Text = "Đóng góp:";
        // 
        // lblDescription
        // 
        lblDescription.ForeColor = Color.FromArgb(71, 85, 105);
        lblDescription.Location = new Point(16, 146);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(408, 44);
        lblDescription.TabIndex = 5;
        lblDescription.Text = "";
        // 
        // btnClose
        // 
        btnClose.DialogResult = DialogResult.OK;
        btnClose.Location = new Point(340, 198);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(84, 28);
        btnClose.TabIndex = 6;
        btnClose.Text = "Đóng";
        btnClose.UseVisualStyleBackColor = true;
        // 
        // DishResultForm
        // 
        AcceptButton = btnClose;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(440, 240);
        Controls.Add(btnClose);
        Controls.Add(lblDescription);
        Controls.Add(lblContributor);
        Controls.Add(lblAddress);
        Controls.Add(lblPrice);
        Controls.Add(lblName);
        Controls.Add(picDish);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "DishResultForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Ăn gì đi!!!";
        ((System.ComponentModel.ISupportInitialize)picDish).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private PictureBox picDish = null!;
    private Label lblName = null!;
    private Label lblPrice = null!;
    private Label lblAddress = null!;
    private Label lblContributor = null!;
    private Label lblDescription = null!;
    private Button btnClose = null!;
}
