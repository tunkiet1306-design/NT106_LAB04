using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace LAB04.Bai04
{
    public partial class FormChiTietPhim : Form
    {
        private readonly string _movieTitle;
        private readonly string _detailUrl;

        private Panel pnlToolbar = null!;
        private Button btnBack = null!;
        private Button btnForward = null!;
        private Button btnReload = null!;
        private Button btnOpenExternal = null!;
        private TextBox txtUrl = null!;
        private Label lblLoading = null!;
        private WebView2 webView = null!;

        public FormChiTietPhim(string movieTitle, string detailUrl)
        {
            InitializeComponent();
            _movieTitle = movieTitle;
            _detailUrl = detailUrl;

            KhoiTaoGiaoDien();
            this.Load += FormChiTietPhim_Load;
        }

        private void KhoiTaoGiaoDien()
        {
            this.Text = $"{_movieTitle} - {_detailUrl}";
            this.Size = new Size(1080, 720);
            this.MinimumSize = new Size(760, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5F);

            pnlToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.FromArgb(15, 23, 42),
                Padding = new Padding(10, 7, 10, 7)
            };

            btnBack = TaoNutDieuHuong("◀", 10);
            btnBack.Click += (s, e) => { if (webView.CoreWebView2?.CanGoBack == true) webView.CoreWebView2.GoBack(); };

            btnForward = TaoNutDieuHuong("▶", 50);
            btnForward.Click += (s, e) => { if (webView.CoreWebView2?.CanGoForward == true) webView.CoreWebView2.GoForward(); };

            btnReload = TaoNutDieuHuong("↻", 90);
            btnReload.Click += (s, e) => webView.CoreWebView2?.Reload();

            btnOpenExternal = new Button
            {
                Text = "Mở trình duyệt ↗",
                Size = new Size(138, 30),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(234, 88, 12),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnOpenExternal.FlatAppearance.BorderSize = 0;
            btnOpenExternal.Click += (s, e) => MoTrinhDuyetNgoai();

            txtUrl = new TextBox
            {
                Location = new Point(136, 9),
                Width = 770,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.FromArgb(226, 232, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 9.5F),
                Text = _detailUrl
            };

            pnlToolbar.Controls.Add(btnBack);
            pnlToolbar.Controls.Add(btnForward);
            pnlToolbar.Controls.Add(btnReload);
            pnlToolbar.Controls.Add(txtUrl);
            pnlToolbar.Controls.Add(btnOpenExternal);

            lblLoading = new Label
            {
                Text = $"Đang tải trang chi tiết phim \"{_movieTitle}\"...",
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(255, 247, 237),
                ForeColor = Color.FromArgb(194, 65, 12),
                Font = new Font("Segoe UI", 9F, FontStyle.Italic)
            };

            webView = new WebView2
            {
                Dock = DockStyle.Fill
            };

            this.Controls.Add(webView);
            this.Controls.Add(lblLoading);
            this.Controls.Add(pnlToolbar);
        }

        private static Button TaoNutDieuHuong(string text, int x)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, 8),
                Size = new Size(34, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private async void FormChiTietPhim_Load(object? sender, EventArgs e)
        {
            try
            {
                string userDataFolder = Path.Combine(Path.GetTempPath(), "Lab04_Bai4_WebView2");
                var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
                await webView.EnsureCoreWebView2Async(env);

                webView.CoreWebView2.NavigationStarting += (s, args) =>
                {
                    if (!IsDisposed)
                    {
                        txtUrl.Text = args.Uri;
                        lblLoading.Visible = true;
                        lblLoading.Text = "Đang tải nội dung trang web...";
                    }
                };

                webView.CoreWebView2.NavigationCompleted += (s, args) =>
                {
                    if (!IsDisposed)
                    {
                        lblLoading.Visible = false;
                        btnBack.Enabled = webView.CoreWebView2.CanGoBack;
                        btnForward.Enabled = webView.CoreWebView2.CanGoForward;
                    }
                };

                webView.CoreWebView2.Navigate(_detailUrl);
            }
            catch (Exception ex)
            {
                lblLoading.Text = "Không thể khởi tạo WebView2 Runtime. Đang mở bằng trình duyệt mặc định...";
                MessageBox.Show(
                    $"Máy tính chưa cài Microsoft Edge WebView2 Runtime hoặc không thể khởi tạo:\n{ex.Message}\n\nỨng dụng sẽ mở đường dẫn bằng trình duyệt mặc định.",
                    "Thông báo WebView2", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MoTrinhDuyetNgoai();
            }
        }

        private void MoTrinhDuyetNgoai()
        {
            try
            {
                Process.Start(new ProcessStartInfo(_detailUrl) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở trình duyệt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}