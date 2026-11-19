namespace LAB04
{
    partial class FormServer04
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Button btnReadFile;
        private Button btnStartServer;
        private DataGridView dgvMovie;
        private RichTextBox rtbLog;
        private ProgressBar progressBar1;
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
            btnReadFile = new Button();
            btnStartServer = new Button();
            dgvMovie = new DataGridView();
            rtbLog = new RichTextBox();
            progressBar1 = new ProgressBar();

            ((System.ComponentModel.ISupportInitialize)dgvMovie).BeginInit();

            SuspendLayout();

            // lblTitle

            lblTitle.AutoSize = true;
            lblTitle.Font = new Font(
                "Segoe UI",
                16F,
                FontStyle.Bold,
                GraphicsUnit.Point);

            lblTitle.Location = new Point(180, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(340, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ PHÒNG VÉ SERVER";

            // btnReadFile

            btnReadFile.Location = new Point(30, 80);
            btnReadFile.Name = "btnReadFile";
            btnReadFile.Size = new Size(120, 40);
            btnReadFile.TabIndex = 1;
            btnReadFile.Text = "Đọc File";
            btnReadFile.UseVisualStyleBackColor = true;
            btnReadFile.Click += btnReadFile_Click;

            // btnStartServer

            btnStartServer.Location = new Point(180, 80);
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new Size(180, 40);
            btnStartServer.TabIndex = 2;
            btnStartServer.Text = "Khởi Động Server";
            btnStartServer.UseVisualStyleBackColor = true;
            btnStartServer.Click += btnStartServer_Click;

            // dgvMovie

            dgvMovie.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgvMovie.Location = new Point(30, 140);
            dgvMovie.Name = "dgvMovie";
            dgvMovie.RowHeadersWidth = 51;
            dgvMovie.Size = new Size(660, 220);
            dgvMovie.TabIndex = 3;

            // progressBar1

            progressBar1.Location = new Point(30, 380);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(660, 25);
            progressBar1.TabIndex = 4;

            // rtbLog

            rtbLog.Location = new Point(30, 430);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(660, 150);
            rtbLog.TabIndex = 5;
            rtbLog.Text = "";

            // FormServer04

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;

            ClientSize = new Size(726, 621);

            Controls.Add(lblTitle);
            Controls.Add(btnReadFile);
            Controls.Add(btnStartServer);
            Controls.Add(dgvMovie);
            Controls.Add(progressBar1);
            Controls.Add(rtbLog);

            Name = "FormServer04";
            Text = "Server";

            Load += FormServer04_Load;

            ((System.ComponentModel.ISupportInitialize)dgvMovie).EndInit();

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}