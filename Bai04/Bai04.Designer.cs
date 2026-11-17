namespace LAB04.Bai04
{
    partial class Bai04
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private Button btnLoadMovie;
        private ProgressBar progressBar1;
        private ListBox lstMovie;
        private TextBox txtCustomer;
        private NumericUpDown numTicket;
        private Button btnBook;
        private Label lblResult;
        private PictureBox picPoster;

        private void InitializeComponent()
        {
            btnLoadMovie = new Button();
            progressBar1 = new ProgressBar();
            lstMovie = new ListBox();
            picPoster = new PictureBox();
            SuspendLayout();

            // btnLoadMovie
            btnLoadMovie.Location = new Point(20, 20);
            btnLoadMovie.Name = "btnLoadMovie";
            btnLoadMovie.Size = new Size(180, 40);
            btnLoadMovie.Text = "Tải Danh Sách Phim";
            btnLoadMovie.UseVisualStyleBackColor = true;
            btnLoadMovie.Click += btnLoadMovie_Click;

            // progressBar1
            progressBar1.Location = new Point(20, 90);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1020, 25);

            // lstMovie
            // lstMovie
            lstMovie.Location = new Point(20, 140);
            lstMovie.Name = "lstMovie";
            lstMovie.Size = new Size(620, 350);
            lstMovie.HorizontalScrollbar = true;

            // Bai04
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);


            // picPoster
            picPoster.Location = new Point(700, 140);
            picPoster.Name = "picPoster";
            picPoster.Size = new Size(300, 350);
            picPoster.SizeMode = PictureBoxSizeMode.StretchImage;
            picPoster.BorderStyle = BorderStyle.FixedSingle;

            txtCustomer = new TextBox();
            numTicket = new NumericUpDown();
            btnBook = new Button();
            lblResult = new Label();

            txtCustomer.Location = new Point(20, 520);
            txtCustomer.Size = new Size(250, 30);
            txtCustomer.PlaceholderText = "Tên khách hàng";

            numTicket.Location = new Point(300, 520);

            numTicket.Minimum = 1;
            numTicket.Maximum = 20;
            numTicket.Value = 1;

            btnBook.Location = new Point(450, 515);
            btnBook.Size = new Size(120, 40);
            btnBook.Text = "Đặt Vé";

            lblResult.Location = new Point(20, 580);
            lblResult.Size = new Size(1020, 80);
            lblResult.BorderStyle = BorderStyle.FixedSingle;

            btnBook.Click += btnBook_Click;
            Controls.Add(btnLoadMovie);
            Controls.Add(progressBar1);
            Controls.Add(lstMovie);
            Controls.Add(picPoster);
            Controls.Add(txtCustomer);
            Controls.Add(numTicket);
            Controls.Add(btnBook);
            Controls.Add(lblResult);
            Name = "Bai04";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản Lý Phòng Vé - Web Scraping";
            lstMovie.SelectedIndexChanged +=
                        lstMovie_SelectedIndexChanged;
            ResumeLayout(false);
        }

        #endregion
    }
}