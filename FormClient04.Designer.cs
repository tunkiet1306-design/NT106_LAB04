using System.Drawing;
using System.Windows.Forms;

namespace LAB04
{
    partial class FormClient04
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblIP;
        private Label lblPort;
        private Label lblName;
        private Label lblMovie;

        private TextBox txtIP;
        private TextBox txtPort;
        private TextBox txtName;

        private ComboBox cboMovie;

        private Button btnConnect;
        private Button btnDatVe;
        private Button btnXoa;
        private Button btnThoat;

        private FlowLayoutPanel flpSeat;
        private ListBox lstSelectedSeat;

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
            lblIP = new Label();
            lblPort = new Label();
            lblName = new Label();
            lblMovie = new Label();

            txtIP = new TextBox();
            txtPort = new TextBox();
            txtName = new TextBox();

            cboMovie = new ComboBox();

            btnConnect = new Button();
            btnDatVe = new Button();
            btnXoa = new Button();
            btnThoat = new Button();

            flpSeat = new FlowLayoutPanel();
            lstSelectedSeat = new ListBox();

            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font(
                "Segoe UI",
                16F,
                FontStyle.Bold);
            lblTitle.Location = new Point(180, 20);
            lblTitle.Text = "QUẢN LÝ PHÒNG VÉ CLIENT";

            // lblPort
            lblPort.AutoSize = true;
            lblPort.Location = new Point(30, 90);
            lblPort.Text = "Port";

            // txtPort
            txtPort.Location = new Point(120, 87);
            txtPort.Size = new Size(100, 27);
            txtPort.Text = "8080";

            // lblIP
            lblIP.AutoSize = true;
            lblIP.Location = new Point(250, 90);
            lblIP.Text = "IP Server:";

            // txtIP
            txtIP.Location = new Point(330, 87);
            txtIP.Size = new Size(180, 27);
            txtIP.Text = "127.0.0.1";

            // btnConnect
            btnConnect.Location = new Point(530, 85);
            btnConnect.Size = new Size(120, 30);
            btnConnect.Text = "Kết Nối";
            btnConnect.Click += btnConnect_Click;

            // lblName
            lblName.AutoSize = true;
            lblName.Location = new Point(30, 140);
            lblName.Text = "Họ tên:";

            // txtName
            txtName.Location = new Point(120, 137);
            txtName.Size = new Size(300, 27);

            // lblMovie
            lblMovie.AutoSize = true;
            lblMovie.Location = new Point(30, 190);
            lblMovie.Text = "Phim:";

            // cboMovie
            cboMovie.Location = new Point(120, 187);
            cboMovie.Size = new Size(250, 28);

            // flpSeat
            flpSeat.BorderStyle = BorderStyle.FixedSingle;
            flpSeat.Location = new Point(30, 240);
            flpSeat.Size = new Size(500, 210);

            // lstSelectedSeat
            lstSelectedSeat.Location = new Point(560, 240);
            lstSelectedSeat.Size = new Size(180, 204);

            // btnDatVe
            btnDatVe.Location = new Point(30, 500);
            btnDatVe.Size = new Size(120, 40);
            btnDatVe.Text = "Đặt Vé";
            btnDatVe.Click += btnDatVe_Click;

            // btnXoa
            btnXoa.Location = new Point(170, 500);
            btnXoa.Size = new Size(120, 40);
            btnXoa.Text = "Xóa Chọn";
            btnXoa.Click += btnXoa_Click;

            // btnThoat
            btnThoat.Location = new Point(310, 500);
            btnThoat.Size = new Size(120, 40);
            btnThoat.Text = "Thoát";
            btnThoat.Click += btnThoat_Click;

            // FormClient04
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);

            Controls.Add(lblTitle);

            Controls.Add(lblPort);
            Controls.Add(txtPort);

            Controls.Add(lblIP);
            Controls.Add(txtIP);

            Controls.Add(btnConnect);

            Controls.Add(lblName);
            Controls.Add(txtName);

            Controls.Add(lblMovie);
            Controls.Add(cboMovie);

            Controls.Add(flpSeat);
            Controls.Add(lstSelectedSeat);

            Controls.Add(btnDatVe);
            Controls.Add(btnXoa);
            Controls.Add(btnThoat);

            Text = "Client";

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}