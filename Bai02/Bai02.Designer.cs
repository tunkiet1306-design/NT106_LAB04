namespace LAB04.Bai02
{
    partial class Bai02
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.RichTextBox rtbHtmlContent;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.rtbHtmlContent = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();

            // txtUrl
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtUrl.Location = new System.Drawing.Point(16, 15);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(520, 27);
            this.txtUrl.TabIndex = 0;
            this.txtUrl.Text = "http://uit.edu.vn";

            // txtFilePath
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilePath.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtFilePath.Location = new System.Drawing.Point(16, 52);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(520, 27);
            this.txtFilePath.TabIndex = 1;
            this.txtFilePath.Text = @"E:\uit.html";

            // btnDownload
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDownload.Location = new System.Drawing.Point(550, 14);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(110, 30);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);

            // rtbHtmlContent
            this.rtbHtmlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbHtmlContent.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.rtbHtmlContent.Location = new System.Drawing.Point(16, 92);
            this.rtbHtmlContent.Name = "rtbHtmlContent";
            this.rtbHtmlContent.ReadOnly = true;
            this.rtbHtmlContent.Size = new System.Drawing.Size(644, 380);
            this.rtbHtmlContent.TabIndex = 3;
            this.rtbHtmlContent.Text = "";

            // Bai2 Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 488);
            this.Controls.Add(this.rtbHtmlContent);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.txtUrl);
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "Bai2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bai2";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

