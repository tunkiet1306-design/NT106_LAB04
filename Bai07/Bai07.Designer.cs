namespace LAB04.Bai07;

partial class Bai07
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
        pnlTop = new Panel();
        lblTitle = new Label();
        btnRandom = new Button();
        btnAddDish = new Button();
        lblApiUrl = new Label();
        cmbApiUrl = new ComboBox();
        btnConnectApi = new Button();
        tabMain = new TabControl();
        tabAll = new TabPage();
        flpAllDishes = new FlowLayoutPanel();
        tabMine = new TabPage();
        flpMyDishes = new FlowLayoutPanel();
        pnlPagination = new Panel();
        btnPrevPage = new Button();
        lblPage = new Label();
        cmbPage = new ComboBox();
        btnNextPage = new Button();
        lblPageSize = new Label();
        cmbPageSize = new ComboBox();
        lblTotal = new Label();
        pnlStatus = new Panel();
        lblAuthStatus = new Label();
        lnkAuthAction = new LinkLabel();
        prgLoading = new ProgressBar();
        pnlTop.SuspendLayout();
        tabMain.SuspendLayout();
        tabAll.SuspendLayout();
        tabMine.SuspendLayout();
        pnlPagination.SuspendLayout();
        pnlStatus.SuspendLayout();
        SuspendLayout();
        // 
        // pnlTop
        // 
        pnlTop.BackColor = Color.FromArgb(248, 250, 252);
        pnlTop.Controls.Add(btnConnectApi);
        pnlTop.Controls.Add(cmbApiUrl);
        pnlTop.Controls.Add(lblApiUrl);
        pnlTop.Controls.Add(btnAddDish);
        pnlTop.Controls.Add(btnRandom);
        pnlTop.Controls.Add(lblTitle);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Name = "pnlTop";
        pnlTop.Size = new Size(720, 92);
        pnlTop.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(180, 52, 43);
        lblTitle.Location = new Point(16, 12);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(216, 32);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "HÔM NAY ĂN GÌ?";
        // 
        // btnRandom
        // 
        btnRandom.BackColor = Color.FromArgb(245, 194, 120);
        btnRandom.Cursor = Cursors.Hand;
        btnRandom.FlatStyle = FlatStyle.Flat;
        btnRandom.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnRandom.Location = new Point(450, 12);
        btnRandom.Name = "btnRandom";
        btnRandom.Size = new Size(116, 34);
        btnRandom.TabIndex = 1;
        btnRandom.Text = "Ăn gì giờ?";
        btnRandom.UseVisualStyleBackColor = false;
        btnRandom.Click += btnRandom_Click;
        // 
        // btnAddDish
        // 
        btnAddDish.BackColor = Color.FromArgb(250, 232, 168);
        btnAddDish.Cursor = Cursors.Hand;
        btnAddDish.FlatStyle = FlatStyle.Flat;
        btnAddDish.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnAddDish.Location = new Point(576, 12);
        btnAddDish.Name = "btnAddDish";
        btnAddDish.Size = new Size(124, 34);
        btnAddDish.TabIndex = 2;
        btnAddDish.Text = "Thêm món ăn";
        btnAddDish.UseVisualStyleBackColor = false;
        btnAddDish.Click += btnAddDish_Click;
        // 
        // lblApiUrl
        // 
        lblApiUrl.AutoSize = true;
        lblApiUrl.ForeColor = Color.FromArgb(71, 85, 105);
        lblApiUrl.Location = new Point(18, 59);
        lblApiUrl.Name = "lblApiUrl";
        lblApiUrl.Size = new Size(56, 15);
        lblApiUrl.TabIndex = 3;
        lblApiUrl.Text = "API URL:";
        // 
        // cmbApiUrl
        // 
        cmbApiUrl.FormattingEnabled = true;
        cmbApiUrl.Items.AddRange(["http://localhost:5077", "https://nt106.uitiot.vn"]);
        cmbApiUrl.Location = new Point(80, 56);
        cmbApiUrl.Name = "cmbApiUrl";
        cmbApiUrl.Size = new Size(524, 23);
        cmbApiUrl.TabIndex = 4;
        // 
        // btnConnectApi
        // 
        btnConnectApi.Location = new Point(612, 55);
        btnConnectApi.Name = "btnConnectApi";
        btnConnectApi.Size = new Size(88, 25);
        btnConnectApi.TabIndex = 5;
        btnConnectApi.Text = "Áp dụng";
        btnConnectApi.UseVisualStyleBackColor = true;
        btnConnectApi.Click += btnConnectApi_Click;
        // 
        // tabMain
        // 
        tabMain.Controls.Add(tabAll);
        tabMain.Controls.Add(tabMine);
        tabMain.Dock = DockStyle.Fill;
        tabMain.Location = new Point(0, 92);
        tabMain.Name = "tabMain";
        tabMain.SelectedIndex = 0;
        tabMain.Size = new Size(720, 408);
        tabMain.TabIndex = 1;
        tabMain.SelectedIndexChanged += tabMain_SelectedIndexChanged;
        // 
        // tabAll
        // 
        tabAll.Controls.Add(flpAllDishes);
        tabAll.Location = new Point(4, 24);
        tabAll.Name = "tabAll";
        tabAll.Padding = new Padding(6);
        tabAll.Size = new Size(712, 380);
        tabAll.TabIndex = 0;
        tabAll.Text = "All";
        tabAll.UseVisualStyleBackColor = true;
        // 
        // flpAllDishes
        // 
        flpAllDishes.AutoScroll = true;
        flpAllDishes.Dock = DockStyle.Fill;
        flpAllDishes.FlowDirection = FlowDirection.TopDown;
        flpAllDishes.Location = new Point(6, 6);
        flpAllDishes.Name = "flpAllDishes";
        flpAllDishes.Size = new Size(700, 368);
        flpAllDishes.TabIndex = 0;
        flpAllDishes.WrapContents = false;
        // 
        // tabMine
        // 
        tabMine.Controls.Add(flpMyDishes);
        tabMine.Location = new Point(4, 24);
        tabMine.Name = "tabMine";
        tabMine.Padding = new Padding(6);
        tabMine.Size = new Size(712, 380);
        tabMine.TabIndex = 1;
        tabMine.Text = "Tôi đóng góp";
        tabMine.UseVisualStyleBackColor = true;
        // 
        // flpMyDishes
        // 
        flpMyDishes.AutoScroll = true;
        flpMyDishes.Dock = DockStyle.Fill;
        flpMyDishes.FlowDirection = FlowDirection.TopDown;
        flpMyDishes.Location = new Point(6, 6);
        flpMyDishes.Name = "flpMyDishes";
        flpMyDishes.Size = new Size(700, 368);
        flpMyDishes.TabIndex = 0;
        flpMyDishes.WrapContents = false;
        // 
        // pnlPagination
        // 
        pnlPagination.BackColor = Color.FromArgb(248, 250, 252);
        pnlPagination.Controls.Add(lblTotal);
        pnlPagination.Controls.Add(cmbPageSize);
        pnlPagination.Controls.Add(lblPageSize);
        pnlPagination.Controls.Add(btnNextPage);
        pnlPagination.Controls.Add(cmbPage);
        pnlPagination.Controls.Add(lblPage);
        pnlPagination.Controls.Add(btnPrevPage);
        pnlPagination.Dock = DockStyle.Bottom;
        pnlPagination.Location = new Point(0, 500);
        pnlPagination.Name = "pnlPagination";
        pnlPagination.Size = new Size(720, 40);
        pnlPagination.TabIndex = 2;
        // 
        // btnPrevPage
        // 
        btnPrevPage.Location = new Point(360, 7);
        btnPrevPage.Name = "btnPrevPage";
        btnPrevPage.Size = new Size(34, 25);
        btnPrevPage.TabIndex = 0;
        btnPrevPage.Text = "<";
        btnPrevPage.UseVisualStyleBackColor = true;
        btnPrevPage.Click += btnPrevPage_Click;
        // 
        // lblPage
        // 
        lblPage.AutoSize = true;
        lblPage.Location = new Point(402, 12);
        lblPage.Name = "lblPage";
        lblPage.Size = new Size(33, 15);
        lblPage.TabIndex = 1;
        lblPage.Text = "Page";
        // 
        // cmbPage
        // 
        cmbPage.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPage.FormattingEnabled = true;
        cmbPage.Location = new Point(440, 8);
        cmbPage.Name = "cmbPage";
        cmbPage.Size = new Size(56, 23);
        cmbPage.TabIndex = 2;
        cmbPage.SelectedIndexChanged += cmbPage_SelectedIndexChanged;
        // 
        // btnNextPage
        // 
        btnNextPage.Location = new Point(502, 7);
        btnNextPage.Name = "btnNextPage";
        btnNextPage.Size = new Size(34, 25);
        btnNextPage.TabIndex = 3;
        btnNextPage.Text = ">";
        btnNextPage.UseVisualStyleBackColor = true;
        btnNextPage.Click += btnNextPage_Click;
        // 
        // lblPageSize
        // 
        lblPageSize.AutoSize = true;
        lblPageSize.Location = new Point(550, 12);
        lblPageSize.Name = "lblPageSize";
        lblPageSize.Size = new Size(55, 15);
        lblPageSize.TabIndex = 4;
        lblPageSize.Text = "Page size";
        // 
        // cmbPageSize
        // 
        cmbPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPageSize.FormattingEnabled = true;
        cmbPageSize.Items.AddRange(["5", "10", "20"]);
        cmbPageSize.Location = new Point(612, 8);
        cmbPageSize.Name = "cmbPageSize";
        cmbPageSize.Size = new Size(88, 23);
        cmbPageSize.TabIndex = 5;
        cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
        // 
        // lblTotal
        // 
        lblTotal.AutoSize = true;
        lblTotal.ForeColor = Color.FromArgb(71, 85, 105);
        lblTotal.Location = new Point(16, 12);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(92, 15);
        lblTotal.TabIndex = 6;
        lblTotal.Text = "Tổng: 0 món ăn";
        // 
        // pnlStatus
        // 
        pnlStatus.BackColor = Color.WhiteSmoke;
        pnlStatus.Controls.Add(prgLoading);
        pnlStatus.Controls.Add(lnkAuthAction);
        pnlStatus.Controls.Add(lblAuthStatus);
        pnlStatus.Dock = DockStyle.Bottom;
        pnlStatus.Location = new Point(0, 540);
        pnlStatus.Name = "pnlStatus";
        pnlStatus.Size = new Size(720, 30);
        pnlStatus.TabIndex = 3;
        // 
        // lblAuthStatus
        // 
        lblAuthStatus.AutoSize = true;
        lblAuthStatus.ForeColor = Color.Firebrick;
        lblAuthStatus.Location = new Point(14, 7);
        lblAuthStatus.Name = "lblAuthStatus";
        lblAuthStatus.Size = new Size(96, 15);
        lblAuthStatus.TabIndex = 0;
        lblAuthStatus.Text = "Unauthenticated";
        // 
        // lnkAuthAction
        // 
        lnkAuthAction.AutoSize = true;
        lnkAuthAction.Location = new Point(120, 7);
        lnkAuthAction.Name = "lnkAuthAction";
        lnkAuthAction.Size = new Size(37, 15);
        lnkAuthAction.TabIndex = 1;
        lnkAuthAction.TabStop = true;
        lnkAuthAction.Text = "Login";
        lnkAuthAction.LinkClicked += lnkAuthAction_LinkClicked;
        // 
        // prgLoading
        // 
        prgLoading.Location = new Point(560, 7);
        prgLoading.Name = "prgLoading";
        prgLoading.Size = new Size(140, 16);
        prgLoading.Style = ProgressBarStyle.Marquee;
        prgLoading.TabIndex = 2;
        prgLoading.Visible = false;
        // 
        // Bai07
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(720, 570);
        Controls.Add(tabMain);
        Controls.Add(pnlPagination);
        Controls.Add(pnlStatus);
        Controls.Add(pnlTop);
        MinimumSize = new Size(680, 520);
        Name = "Bai07";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Hôm nay ăn gì?";
        Load += Bai07_Load;
        FormClosed += Bai07_FormClosed;
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        tabMain.ResumeLayout(false);
        tabAll.ResumeLayout(false);
        tabMine.ResumeLayout(false);
        pnlPagination.ResumeLayout(false);
        pnlPagination.PerformLayout();
        pnlStatus.ResumeLayout(false);
        pnlStatus.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlTop = null!;
    private Label lblTitle = null!;
    private Button btnRandom = null!;
    private Button btnAddDish = null!;
    private Label lblApiUrl = null!;
    private ComboBox cmbApiUrl = null!;
    private Button btnConnectApi = null!;
    private TabControl tabMain = null!;
    private TabPage tabAll = null!;
    private FlowLayoutPanel flpAllDishes = null!;
    private TabPage tabMine = null!;
    private FlowLayoutPanel flpMyDishes = null!;
    private Panel pnlPagination = null!;
    private Button btnPrevPage = null!;
    private Label lblPage = null!;
    private ComboBox cmbPage = null!;
    private Button btnNextPage = null!;
    private Label lblPageSize = null!;
    private ComboBox cmbPageSize = null!;
    private Label lblTotal = null!;
    private Panel pnlStatus = null!;
    private Label lblAuthStatus = null!;
    private LinkLabel lnkAuthAction = null!;
    private ProgressBar prgLoading = null!;
}
