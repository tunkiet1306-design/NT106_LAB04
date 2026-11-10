namespace LAB04.Bai03
{
    partial class Bai3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null!;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelTop = new Panel();
            btnLoad = new Button();
            txtUrl = new TextBox();
            btnReload = new Button();
            lblStatus = new Label();
            btnViewSource = new Button();
            btnDownFiles = new Button();
            btnDownResources = new Button();
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = SystemColors.Control;
            panelTop.Controls.Add(btnLoad);
            panelTop.Controls.Add(txtUrl);
            panelTop.Controls.Add(btnReload);
            panelTop.Controls.Add(lblStatus);
            panelTop.Controls.Add(btnViewSource);
            panelTop.Controls.Add(btnDownFiles);
            panelTop.Controls.Add(btnDownResources);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(920, 76);
            panelTop.TabIndex = 0;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(10, 8);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(80, 28);
            btnLoad.TabIndex = 0;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // txtUrl
            // 
            txtUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUrl.Font = new Font("Segoe UI", 10F);
            txtUrl.Location = new Point(98, 9);
            txtUrl.Name = "txtUrl";
            txtUrl.PlaceholderText = "Nhập URL (vd: https://uit.edu.vn/)";
            txtUrl.Size = new Size(718, 25);
            txtUrl.TabIndex = 1;
            txtUrl.Text = "https://uit.edu.vn/";
            txtUrl.KeyDown += txtUrl_KeyDown;
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReload.Location = new Point(824, 8);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(84, 28);
            btnReload.TabIndex = 2;
            btnReload.Text = "reload";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.AutoEllipsis = true;
            lblStatus.ForeColor = Color.DimGray;
            lblStatus.Location = new Point(10, 46);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(494, 20);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Sẵn sàng";
            // 
            // btnViewSource
            // 
            btnViewSource.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnViewSource.Location = new Point(512, 42);
            btnViewSource.Name = "btnViewSource";
            btnViewSource.Size = new Size(110, 28);
            btnViewSource.TabIndex = 4;
            btnViewSource.Text = "View Source";
            btnViewSource.UseVisualStyleBackColor = true;
            btnViewSource.Click += btnViewSource_Click;
            // 
            // btnDownFiles
            // 
            btnDownFiles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDownFiles.Location = new Point(628, 42);
            btnDownFiles.Name = "btnDownFiles";
            btnDownFiles.Size = new Size(125, 28);
            btnDownFiles.TabIndex = 5;
            btnDownFiles.Text = "Down Files";
            btnDownFiles.UseVisualStyleBackColor = true;
            btnDownFiles.Click += btnDownFiles_Click;
            // 
            // btnDownResources
            // 
            btnDownResources.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDownResources.Location = new Point(759, 42);
            btnDownResources.Name = "btnDownResources";
            btnDownResources.Size = new Size(149, 28);
            btnDownResources.TabIndex = 6;
            btnDownResources.Text = "Down Resources";
            btnDownResources.UseVisualStyleBackColor = true;
            btnDownResources.Click += btnDownResources_Click;
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Dock = DockStyle.Fill;
            webView.Location = new Point(0, 76);
            webView.Name = "webView";
            webView.Size = new Size(920, 524);
            webView.TabIndex = 1;
            webView.ZoomFactor = 1D;
            // 
            // Bai3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(920, 600);
            Controls.Add(webView);
            Controls.Add(panelTop);
            MinimumSize = new Size(700, 450);
            Name = "Bai3";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bai3";
            Load += Bai3_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop = null!;
        private Button btnLoad = null!;
        private TextBox txtUrl = null!;
        private Button btnReload = null!;
        private Label lblStatus = null!;
        private Button btnViewSource = null!;
        private Button btnDownFiles = null!;
        private Button btnDownResources = null!;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView = null!;
    }
}
