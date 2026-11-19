using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB04.Bai04
{
    public partial class Bai4 : Form
    {
        // ================= BẢNG MÀU HIỆN ĐẠI (MODERN CINEMA THEME) =================
        private static readonly Color ColorDarkHeader = Color.FromArgb(15, 23, 42);
        private static readonly Color ColorCanvasBg = Color.FromArgb(241, 245, 249);
        private static readonly Color ColorCardBg = Color.White;
        private static readonly Color ColorCardSelected = Color.FromArgb(255, 247, 237);
        private static readonly Color ColorCardHover = Color.FromArgb(248, 250, 252);
        private static readonly Color ColorAccentOrange = Color.FromArgb(234, 88, 12);
        private static readonly Color ColorTextPrimary = Color.FromArgb(15, 23, 42);
        private static readonly Color ColorTextMuted = Color.FromArgb(100, 116, 139);
        private static readonly Color ColorBorderSoft = Color.FromArgb(226, 232, 240);

        private static readonly Color ColorSeatVot = Color.FromArgb(219, 234, 254);
        private static readonly Color ColorSeatThuong = Color.FromArgb(241, 245, 249);
        private static readonly Color ColorSeatVip = Color.FromArgb(254, 243, 199);
        private static readonly Color ColorSeatSelected = Color.FromArgb(22, 163, 74);
        private static readonly Color ColorSeatSold = Color.FromArgb(148, 163, 184);

        private static readonly CultureInfo ViCulture = CultureInfo.GetCultureInfo("vi-VN");

        // ================= TRẠNG THÁI DỮ LIỆU & NGHIỆP VỤ =================
        private readonly BetaCinemaScraper _scraper = new BetaCinemaScraper();
        private CancellationTokenSource? _crawlCts;

        private List<Movie> _allMovies = new List<Movie>();
        private readonly Dictionary<int, Image> _posterCache = new Dictionary<int, Image>();
        private readonly Dictionary<int, Panel> _movieCards = new Dictionary<int, Panel>();

        private readonly Dictionary<string, HashSet<string>> _gheDaBan = new Dictionary<string, HashSet<string>>();
        private readonly HashSet<string> _gheDangChon = new HashSet<string>();
        private readonly Dictionary<string, Button> _seatButtons = new Dictionary<string, Button>();

        private Movie? _selectedMovie;
        private readonly ToolTip _toolTip = new ToolTip();

        // ================= CÁC ĐIỀU KHIỂN GIAO DIỆN (UI CONTROLS) =================
        private Panel pnlHeader = null!;
        private Button btnReloadWeb = null!;
        private Button btnXemJson = null!;

        private TextBox txtTimKiem = null!;
        private Label lblSoLuongPhim = null!;
        private FlowLayoutPanel flpMovies = null!;

        private Label lblPhimDangChon = null!;
        private Label lblThongTinGia = null!;
        private TextBox txtHoTenKhach = null!;
        private ComboBox cbbPhongChieu = null!;
        private Panel pnlSeatGrid = null!;
        private Label lblTamTinh = null!;
        private Button btnDatVe = null!;
        private RichTextBox rtbBienLai = null!;

        private ProgressBar prgCrawl = null!;
        private Label lblTrangThai = null!;

        public Bai4()
        {
            InitializeComponent();
            KhoiTaoGiaoDien();

            this.Load += async (s, e) => await TaiDuLieuPhimAsync();
            this.FormClosing += Bai4_FormClosing;
        }

        // =========================================================================
        // 1. DỰNG TOÀN BỘ GIAO DIỆN HIỆN ĐẠI (MODERN CINEMA UI)
        // =========================================================================
        private void KhoiTaoGiaoDien()
        {
            this.Text = "Quản Lý Phòng Vé (Phiên bản 4) - Beta Cinemas";
            this.Size = new Size(1260, 780);
            this.MinimumSize = new Size(1080, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorCanvasBg;
            this.Font = new Font("Segoe UI", 9.5F);

            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 68,
                BackColor = ColorDarkHeader,
                Padding = new Padding(20, 10, 20, 10)
            };

            var lblAppTitle = new Label
            {
                Text = "🎬 BETA CINEMAS — QUẢN LÝ PHÒNG VÉ",
                Font = new Font("Segoe UI", 14.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 10),
                AutoSize = true
            };

            var lblAppSub = new Label
            {
                Text = "Trích xuất dữ liệu phim trực tiếp từ betacinemas.vn • Lưu trữ JSON • Đặt vé trực quan",
                Font = new Font("Segoe UI", 8.8F),
                ForeColor = Color.FromArgb(148, 163, 184),
                Location = new Point(22, 39),
                AutoSize = true
            };

            btnReloadWeb = new Button
            {
                Text = "↻ Crawl lại từ Web",
                Size = new Size(156, 38),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlHeader.Width - 335, 15),
                BackColor = ColorAccentOrange,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReloadWeb.FlatAppearance.BorderSize = 0;
            btnReloadWeb.Click += async (s, e) => await TaiDuLieuPhimAsync();

            btnXemJson = new Button
            {
                Text = "📄 Xem movies.json",
                Size = new Size(150, 38),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlHeader.Width - 170, 15),
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnXemJson.FlatAppearance.BorderSize = 0;
            btnXemJson.Click += BtnXemJson_Click;

            pnlHeader.Controls.AddRange(new Control[] { lblAppTitle, lblAppSub, btnReloadWeb, btnXemJson });

            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                BackColor = Color.White,
                Padding = new Padding(16, 10, 16, 10)
            };
            pnlFooter.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorBorderSoft, 1);
                e.Graphics.DrawLine(pen, 0, 0, pnlFooter.Width, 0);
            };

            prgCrawl = new ProgressBar
            {
                Location = new Point(16, 12),
                Size = new Size(320, 20),
                Minimum = 0,
                Maximum = 100,
                Style = ProgressBarStyle.Continuous
            };

            lblTrangThai = new Label
            {
                Location = new Point(350, 12),
                Size = new Size(860, 22),
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                ForeColor = ColorTextPrimary,
                Font = new Font("Segoe UI", 9.2F, FontStyle.Regular),
                Text = "Đang chuẩn bị trích xuất dữ liệu phim..."
            };

            pnlFooter.Controls.Add(prgCrawl);
            pnlFooter.Controls.Add(lblTrangThai);

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(14, 12, 14, 12),
                BackColor = ColorCanvasBg
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel pnlLeft = TaoKhungDanhSachPhim();
            Panel pnlRight = TaoKhungDatVe();

            mainLayout.Controls.Add(pnlLeft, 0, 0);
            mainLayout.Controls.Add(pnlRight, 1, 0);

            this.Controls.Add(mainLayout);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlFooter);
        }

        private Panel TaoKhungDanhSachPhim()
        {
            var container = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorCardBg,
                Margin = new Padding(0, 0, 10, 0),
                Padding = new Padding(14)
            };

            var pnlTopBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 68,
                BackColor = ColorCardBg
            };

            var lblSectionTitle = new Label
            {
                Text = "PHIM ĐANG CHIẾU",
                Font = new Font("Segoe UI", 12.5F, FontStyle.Bold),
                ForeColor = ColorAccentOrange,
                Location = new Point(0, 2),
                AutoSize = true
            };

            lblSoLuongPhim = new Label
            {
                Text = "(Nhấn vào Banner phim để xem chi tiết trên Website)",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = ColorTextMuted,
                Location = new Point(170, 7),
                AutoSize = true
            };

            txtTimKiem = new TextBox
            {
                Location = new Point(0, 34),
                Width = 610,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 9.8F),
                PlaceholderText = "🔍 Tìm kiếm tên phim hoặc thể loại..."
            };
            txtTimKiem.TextChanged += (s, e) => HienThiDanhSachPhim(txtTimKiem.Text);

            pnlTopBar.Controls.AddRange(new Control[] { lblSectionTitle, lblSoLuongPhim, txtTimKiem });

            flpMovies = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(8)
            };
            flpMovies.SizeChanged += (s, e) => DieuChinhChieuRongCard();

            container.Controls.Add(flpMovies);
            container.Controls.Add(pnlTopBar);
            return container;
        }

        private Panel TaoKhungDatVe()
        {
            var container = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorCardBg,
                Margin = new Padding(6, 0, 0, 0),
                Padding = new Padding(16, 12, 16, 12),
                AutoScroll = true
            };

            var lblTitle = new Label
            {
                Text = "ĐẶT VÉ & THANH TOÁN",
                Font = new Font("Segoe UI", 12.5F, FontStyle.Bold),
                ForeColor = ColorDarkHeader,
                Location = new Point(16, 12),
                AutoSize = true
            };

            var pnlSelectedBox = new Panel
            {
                Location = new Point(16, 44),
                Size = new Size(495, 56),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = ColorCardSelected,
                Padding = new Padding(10, 6, 10, 6)
            };
            pnlSelectedBox.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(253, 186, 116), 1.5F);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlSelectedBox.Width - 1, pnlSelectedBox.Height - 1);
            };

            lblPhimDangChon = new Label
            {
                Text = "Chưa chọn phim (hãy chọn 1 phim ở danh sách bên trái)",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = ColorAccentOrange,
                Location = new Point(10, 7),
                Size = new Size(475, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AutoEllipsis = true
            };

            lblThongTinGia = new Label
            {
                Text = "Giá vé chuẩn: -- VNĐ | Vé vớt: 25% • Thường: 100% • VIP: 200%",
                Font = new Font("Segoe UI", 8.6F),
                ForeColor = ColorTextMuted,
                Location = new Point(10, 31),
                AutoSize = true
            };

            pnlSelectedBox.Controls.Add(lblPhimDangChon);
            pnlSelectedBox.Controls.Add(lblThongTinGia);

            var lblKhach = new Label { Text = "Họ tên khách hàng:", Location = new Point(16, 112), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            txtHoTenKhach = new TextBox
            {
                Location = new Point(16, 134),
                Size = new Size(315, 27),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                PlaceholderText = "Ví dụ: Nguyễn Văn A"
            };

            var lblPhong = new Label { Text = "Phòng chiếu:", Location = new Point(345, 112), Anchor = AnchorStyles.Top | AnchorStyles.Right, AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            cbbPhongChieu = new ComboBox
            {
                Location = new Point(345, 134),
                Size = new Size(166, 27),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbbPhongChieu.SelectedIndexChanged += (s, e) => CapNhatSoDoGhe();

            var lblSoDo = new Label
            {
                Text = "Chọn chỗ ngồi (Nhấn trực tiếp vào ghế):",
                Location = new Point(16, 170),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            pnlSeatGrid = new Panel
            {
                Location = new Point(16, 194),
                Size = new Size(495, 210),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.FixedSingle
            };
            KhoiTaoSoDo15Ghe();

            Panel pnlLegend = TaoChuThichMauGhe();
            pnlLegend.Location = new Point(16, 408);
            pnlLegend.Size = new Size(495, 28);
            pnlLegend.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            lblTamTinh = new Label
            {
                Text = "Ghế đang chọn: (chưa có)  |  Tạm tính: 0 VNĐ",
                Location = new Point(16, 442),
                Size = new Size(495, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 128, 61)
            };

            btnDatVe = new Button
            {
                Text = "🎫 ĐẶT VÉ && THANH TOÁN",
                Location = new Point(16, 470),
                Size = new Size(495, 42),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = ColorAccentOrange,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDatVe.FlatAppearance.BorderSize = 0;
            btnDatVe.Click += BtnDatVe_Click;

            var lblBienLai = new Label
            {
                Text = "Biên lai giao dịch gần nhất:",
                Location = new Point(16, 520),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            rtbBienLai = new RichTextBox
            {
                Location = new Point(16, 542),
                Size = new Size(495, 118),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(226, 232, 240),
                Font = new Font("Consolas", 9.2F),
                BorderStyle = BorderStyle.None,
                Text = "Sẵn sàng đặt vé cho khách hàng..."
            };

            container.Controls.AddRange(new Control[]
            {
                lblTitle, pnlSelectedBox, lblKhach, txtHoTenKhach, lblPhong, cbbPhongChieu,
                lblSoDo, pnlSeatGrid, pnlLegend, lblTamTinh, btnDatVe, lblBienLai, rtbBienLai
            });
            CapNhatSoDoGhe();
            return container;
        }

        private void KhoiTaoSoDo15Ghe()
        {
            var lblScreen = new Label
            {
                Text = "MÀN HÌNH CHIẾU",
                Dock = DockStyle.Top,
                Height = 26,
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 3,
                Padding = new Padding(12, 10, 12, 10)
            };

            for (int c = 0; c < 5; c++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            for (int r = 0; r < 3; r++) grid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            string[] rows = { "A", "B", "C" };
            for (int r = 0; r < rows.Length; r++)
            {
                for (int c = 1; c <= 5; c++)
                {
                    string seatCode = rows[r] + c;
                    var btnSeat = new Button
                    {
                        Text = seatCode,
                        Tag = seatCode,
                        Dock = DockStyle.Fill,
                        Margin = new Padding(5),
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        Cursor = Cursors.Hand
                    };
                    btnSeat.FlatAppearance.BorderSize = 1;
                    btnSeat.Click += BtnSeat_Click;

                    _seatButtons[seatCode] = btnSeat;
                    grid.Controls.Add(btnSeat, c - 1, r);
                }
            }

            pnlSeatGrid.Controls.Add(grid);
            pnlSeatGrid.Controls.Add(lblScreen);
        }

        private Panel TaoChuThichMauGhe()
        {
            var pnl = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 4, 0, 0)
            };

            void AddLegendItem(Color color, string text)
            {
                var box = new Panel { Size = new Size(14, 14), BackColor = color, Margin = new Padding(0, 3, 4, 0), BorderStyle = BorderStyle.FixedSingle };
                var lbl = new Label { Text = text, AutoSize = true, Font = new Font("Segoe UI", 8.2F), ForeColor = ColorTextMuted, Margin = new Padding(0, 2, 10, 0) };
                pnl.Controls.Add(box);
                pnl.Controls.Add(lbl);
            }

            AddLegendItem(ColorSeatVot, "Vớt (25%)");
            AddLegendItem(ColorSeatThuong, "Thường");
            AddLegendItem(ColorSeatVip, "VIP (200%)");
            AddLegendItem(ColorSeatSelected, "Đang chọn");
            AddLegendItem(ColorSeatSold, "Đã bán");

            return pnl;
        }

        // =========================================================================
        // 2. TRÍCH XUẤT DỮ LIỆU PHIM TỪ WEB & CẬP NHẬT PROGRESSBAR
        // =========================================================================
        private async Task TaiDuLieuPhimAsync()
        {
            _crawlCts?.Cancel();
            _crawlCts?.Dispose();
            _crawlCts = new CancellationTokenSource();
            CancellationToken token = _crawlCts.Token;

            btnReloadWeb.Enabled = false;
            btnReloadWeb.Text = "Đang tải...";
            prgCrawl.Value = 0;
            lblTrangThai.Text = "Đang kết nối tới Beta Cinemas...";

            var progress = new Progress<CrawlProgress>(p =>
            {
                if (!IsDisposed)
                {
                    prgCrawl.Value = p.Percent;
                    lblTrangThai.Text = $"{p.Status} ({p.Percent}%)";
                }
            });

            try
            {
                ScrapeResult result = await _scraper.ScrapeMoviesAsync(progress, token);
                if (IsDisposed || token.IsCancellationRequested) return;

                GiaiPhongCacheAnh();
                _allMovies = result.Movies;
                foreach (var kv in result.Posters)
                {
                    _posterCache[kv.Key] = kv.Value;
                }

                HienThiDanhSachPhim(txtTimKiem.Text);

                if (_allMovies.Count > 0)
                {
                    ChonPhimDeDatVe(_allMovies[0]);
                }

                lblTrangThai.Text = result.Message;
            }
            catch (OperationCanceledException)
            {
                // Bỏ qua khi đóng Form hoặc tải lại giữa chừng
            }
            catch (Exception ex)
            {
                lblTrangThai.Text = "Lỗi trích xuất dữ liệu: " + ex.Message;
                MessageBox.Show("Không thể tải danh sách phim:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnReloadWeb.Enabled = true;
                    btnReloadWeb.Text = "↻ Crawl lại từ Web";
                }
            }
        }

        private void HienThiDanhSachPhim(string? tuKhoa = null)
        {
            flpMovies.SuspendLayout();
            XoaCacCardHienTai();

            string keyword = (tuKhoa ?? string.Empty).Trim();
            var filtered = string.IsNullOrEmpty(keyword)
                ? _allMovies
                : _allMovies.Where(m =>
                    m.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    m.Genre.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            lblSoLuongPhim.Text = $"({filtered.Count}/{_allMovies.Count} phim • Nhấn vào Banner để xem chi tiết)";

            int cardWidth = Math.Max(420, flpMovies.ClientSize.Width - 26);
            foreach (Movie movie in filtered)
            {
                Panel card = TaoThePhimCard(movie, cardWidth);
                _movieCards[movie.Id] = card;
                flpMovies.Controls.Add(card);
            }

            flpMovies.ResumeLayout();
            CapNhatHighlightCard();
        }

        private Panel TaoThePhimCard(Movie movie, int width)
        {
            var card = new Panel
            {
                Width = width,
                Height = 148,
                BackColor = ColorCardBg,
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(10),
                Tag = movie
            };

            card.Paint += (s, e) =>
            {
                bool isSelected = _selectedMovie?.Id == movie.Id;
                using var pen = new Pen(isSelected ? ColorAccentOrange : ColorBorderSoft, isSelected ? 2F : 1F);
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            var picBanner = new PictureBox
            {
                Location = new Point(10, 10),
                Size = new Size(92, 128),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(241, 245, 249),
                Cursor = Cursors.Hand,
                Image = _posterCache.TryGetValue(movie.Id, out Image? img) ? img : null
            };
            _toolTip.SetToolTip(picBanner, "Nhấn vào banner để xem thông tin chi tiết phim trên Beta Cinemas");
            picBanner.Click += (s, e) => MoChiTietPhim(movie);

            var lblTitle = new Label
            {
                Text = movie.Title,
                Location = new Point(114, 10),
                Size = new Size(width - 128, 26),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorAccentOrange,
                AutoEllipsis = true,
                Cursor = Cursors.Hand
            };
            lblTitle.Click += (s, e) => MoChiTietPhim(movie);

            var lblUrl = new Label
            {
                Text = movie.DetailUrl,
                Location = new Point(114, 38),
                Size = new Size(width - 128, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Underline),
                ForeColor = Color.FromArgb(37, 99, 235),
                AutoEllipsis = true,
                Cursor = Cursors.Hand
            };
            lblUrl.Click += (s, e) => MoChiTietPhim(movie);

            var lblMeta = new Label
            {
                Text = $"Thể loại: {movie.Genre}  •  Thời lượng: {movie.Duration}",
                Location = new Point(114, 64),
                Size = new Size(width - 128, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 8.8F),
                ForeColor = ColorTextMuted,
                AutoEllipsis = true
            };

            var lblGia = new Label
            {
                Text = $"Giá chuẩn: {FormatVnd(movie.StandardPrice)}  |  Phòng: {string.Join(", ", movie.Rooms)}",
                Location = new Point(114, 88),
                Size = new Size(width - 250, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ColorTextPrimary
            };

            var btnChiTiet = new Button
            {
                Text = "🌐 Chi tiết Web",
                Size = new Size(116, 30),
                Location = new Point(114, 110),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = ColorTextPrimary,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnChiTiet.FlatAppearance.BorderColor = ColorBorderSoft;
            btnChiTiet.Click += (s, e) => MoChiTietPhim(movie);

            var btnChonDatVe = new Button
            {
                Text = "🎫 Chọn đặt vé",
                Size = new Size(124, 30),
                Location = new Point(238, 110),
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorAccentOrange,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnChonDatVe.FlatAppearance.BorderSize = 0;
            btnChonDatVe.Click += (s, e) => ChonPhimDeDatVe(movie);

            card.Click += (s, e) => ChonPhimDeDatVe(movie);
            lblMeta.Click += (s, e) => ChonPhimDeDatVe(movie);
            lblGia.Click += (s, e) => ChonPhimDeDatVe(movie);

            card.MouseEnter += (s, e) => { if (_selectedMovie?.Id != movie.Id) card.BackColor = ColorCardHover; };
            card.MouseLeave += (s, e) => { if (_selectedMovie?.Id != movie.Id) card.BackColor = ColorCardBg; };

            card.Controls.AddRange(new Control[] { picBanner, lblTitle, lblUrl, lblMeta, lblGia, btnChiTiet, btnChonDatVe });
            return card;
        }

        private void MoChiTietPhim(Movie movie)
        {
            using var frmDetail = new FormChiTietPhim(movie.Title, movie.DetailUrl);
            frmDetail.ShowDialog(this);
        }

        // =========================================================================
        // 3. LOGIC NGHIỆP VỤ ĐẶT VÉ & TÍNH TIỀN
        // =========================================================================
        private void ChonPhimDeDatVe(Movie movie)
        {
            _selectedMovie = movie;
            lblPhimDangChon.Text = $"🎞️ {movie.Title}";
            lblThongTinGia.Text = $"Giá chuẩn: {FormatVnd(movie.StandardPrice)} | Vớt: {FormatVnd(movie.StandardPrice * 0.25m)} • VIP: {FormatVnd(movie.StandardPrice * 2m)}";

            cbbPhongChieu.Items.Clear();
            foreach (int room in movie.Rooms)
            {
                cbbPhongChieu.Items.Add($"Phòng {room}");
            }

            if (cbbPhongChieu.Items.Count > 0)
            {
                cbbPhongChieu.SelectedIndex = 0;
            }

            CapNhatHighlightCard();
            CapNhatSoDoGhe();
        }

        private string GetSuatChieuKey()
        {
            if (_selectedMovie == null || cbbPhongChieu.SelectedItem == null)
                return string.Empty;

            return $"{_selectedMovie.Id}_{cbbPhongChieu.SelectedItem}";
        }

        private void CapNhatSoDoGhe()
        {
            _gheDangChon.Clear();
            string key = GetSuatChieuKey();
            bool hasMovie = !string.IsNullOrEmpty(key);
            HashSet<string>? soldSeats = hasMovie && _gheDaBan.TryGetValue(key, out var set) ? set : null;

            foreach (var kv in _seatButtons)
            {
                string seat = kv.Key;
                Button btn = kv.Value;

                if (!hasMovie)
                {
                    btn.Enabled = false;
                    btn.BackColor = Color.FromArgb(226, 232, 240);
                    btn.ForeColor = ColorTextMuted;
                    btn.FlatAppearance.BorderColor = ColorBorderSoft;
                    continue;
                }

                bool isSold = soldSeats != null && soldSeats.Contains(seat);
                if (isSold)
                {
                    btn.Enabled = false;
                    btn.BackColor = ColorSeatSold;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(100, 116, 139);
                    _toolTip.SetToolTip(btn, $"Ghế {seat}: Đã có người đặt");
                }
                else
                {
                    btn.Enabled = true;
                    ApDungMauGheTrong(btn, seat);
                    decimal price = TinhGiaMotGhe(seat, _selectedMovie!.StandardPrice);
                    _toolTip.SetToolTip(btn, $"Ghế {seat} ({LayTenHangGhe(seat)}): {FormatVnd(price)}");
                }
            }

            CapNhatTamTinh();
        }

        private void BtnSeat_Click(object? sender, EventArgs e)
        {
            if (_selectedMovie == null || sender is not Button btn || btn.Tag is not string seat)
                return;

            if (_gheDangChon.Contains(seat))
            {
                _gheDangChon.Remove(seat);
                ApDungMauGheTrong(btn, seat);
            }
            else
            {
                _gheDangChon.Add(seat);
                btn.BackColor = ColorSeatSelected;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(21, 128, 61);
            }

            CapNhatTamTinh();
        }

        private void CapNhatTamTinh()
        {
            if (lblTamTinh == null) return;
            if (_selectedMovie == null || _gheDangChon.Count == 0)
            {
                lblTamTinh.Text = "Ghế đang chọn: (chưa có)  |  Tạm tính: 0 VNĐ";
                return;
            }

            var sortedSeats = _gheDangChon.OrderBy(s => s).ToList();
            decimal total = sortedSeats.Sum(s => TinhGiaMotGhe(s, _selectedMovie.StandardPrice));
            lblTamTinh.Text = $"Ghế đang chọn: {string.Join(", ", sortedSeats)}  |  Tạm tính: {FormatVnd(total)}";
        }

        private void BtnDatVe_Click(object? sender, EventArgs e)
        {
            if (_selectedMovie == null)
            {
                MessageBox.Show("Vui lòng chọn một bộ phim trước khi đặt vé!", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string customerName = txtHoTenKhach.Text.Trim();
            if (string.IsNullOrWhiteSpace(customerName))
            {
                MessageBox.Show("Vui lòng nhập họ và tên khách hàng!", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTenKhach.Focus();
                return;
            }

            if (cbbPhongChieu.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn phòng chiếu!", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_gheDangChon.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 ghế ngồi trên sơ đồ!", "Chưa chọn ghế",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string roomName = cbbPhongChieu.SelectedItem.ToString()!;
            string key = GetSuatChieuKey();

            if (!_gheDaBan.TryGetValue(key, out HashSet<string>? soldSet))
            {
                soldSet = new HashSet<string>();
                _gheDaBan[key] = soldSet;
            }

            List<string> selectedSeats = _gheDangChon.OrderBy(s => s).ToList();
            foreach (string seat in selectedSeats)
            {
                if (soldSet.Contains(seat))
                {
                    MessageBox.Show($"Ghế [{seat}] đã có người mua. Vui lòng chọn ghế khác!",
                        "Trùng ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CapNhatSoDoGhe();
                    return;
                }
            }

            decimal totalAmount = 0m;
            var detailLines = new StringBuilder();
            foreach (string seat in selectedSeats)
            {
                decimal seatPrice = TinhGiaMotGhe(seat, _selectedMovie.StandardPrice);
                totalAmount += seatPrice;
                soldSet.Add(seat);
                detailLines.AppendLine($"   • Ghế {seat} ({LayTenHangGhe(seat)}): {FormatVnd(seatPrice)}");
            }

            string receipt =
                "================ BIÊN LAI ĐẶT VÉ ================\n" +
                $"Khách hàng : {customerName}\n" +
                $"Bộ phim    : {_selectedMovie.Title}\n" +
                $"Phòng chiếu: {roomName}\n" +
                $"Thời gian  : {DateTime.Now:dd/MM/yyyy HH:mm:ss}\n" +
                $"Vé đã chọn : {string.Join(", ", selectedSeats)} ({selectedSeats.Count} vé)\n" +
                detailLines.ToString() +
                "-------------------------------------------------\n" +
                $"TỔNG TIỀN THANH TOÁN: {FormatVnd(totalAmount)}\n" +
                "=================================================";

            rtbBienLai.Text = receipt;
            CapNhatSoDoGhe();

            MessageBox.Show(receipt, "Đặt vé thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =========================================================================
        // 4. HÀM PHỤ TRỢ & QUẢN LÝ TÀI NGUYÊN
        // =========================================================================
        private static decimal TinhGiaMotGhe(string seatCode, decimal standardPrice)
        {
            if (seatCode is "A1" or "A5" or "C1" or "C5")
                return standardPrice * 0.25m;
            if (seatCode is "B2" or "B3" or "B4")
                return standardPrice * 2.0m;
            return standardPrice;
        }

        private static string LayTenHangGhe(string seatCode)
        {
            if (seatCode is "A1" or "A5" or "C1" or "C5") return "Vé vớt";
            if (seatCode is "B2" or "B3" or "B4") return "Vé VIP";
            return "Vé thường";
        }

        private static void ApDungMauGheTrong(Button btn, string seatCode)
        {
            if (seatCode is "A1" or "A5" or "C1" or "C5")
            {
                btn.BackColor = ColorSeatVot;
                btn.ForeColor = Color.FromArgb(30, 64, 175);
                btn.FlatAppearance.BorderColor = Color.FromArgb(147, 197, 253);
            }
            else if (seatCode is "B2" or "B3" or "B4")
            {
                btn.BackColor = ColorSeatVip;
                btn.ForeColor = Color.FromArgb(180, 83, 9);
                btn.FlatAppearance.BorderColor = Color.FromArgb(252, 211, 77);
            }
            else
            {
                btn.BackColor = ColorSeatThuong;
                btn.ForeColor = ColorTextPrimary;
                btn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            }
        }

        private static string FormatVnd(decimal amount)
        {
            return string.Format(ViCulture, "{0:N0} VNĐ", amount);
        }

        private void BtnXemJson_Click(object? sender, EventArgs e)
        {
            if (!File.Exists(_scraper.JsonFilePath))
            {
                MessageBox.Show("Chưa có file movies.json! Vui lòng tải dữ liệu phim trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string jsonContent = File.ReadAllText(_scraper.JsonFilePath, Encoding.UTF8);

            using var dlg = new Form
            {
                Text = $"Nội dung file JSON — {_scraper.JsonFilePath}",
                Size = new Size(780, 560),
                StartPosition = FormStartPosition.CenterParent
            };

            var rtb = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 9.5F),
                BackColor = ColorDarkHeader,
                ForeColor = Color.FromArgb(226, 232, 240),
                Text = jsonContent
            };

            dlg.Controls.Add(rtb);
            dlg.ShowDialog(this);
        }

        private void CapNhatHighlightCard()
        {
            foreach (var kv in _movieCards)
            {
                bool isSelected = _selectedMovie != null && kv.Key == _selectedMovie.Id;
                kv.Value.BackColor = isSelected ? ColorCardSelected : ColorCardBg;
                kv.Value.Invalidate();
            }
        }

        private void DieuChinhChieuRongCard()
        {
            int newWidth = Math.Max(420, flpMovies.ClientSize.Width - 26);
            foreach (Control ctrl in flpMovies.Controls)
            {
                ctrl.Width = newWidth;
            }
        }

        private void XoaCacCardHienTai()
        {
            foreach (Control ctrl in flpMovies.Controls)
            {
                foreach (Control child in ctrl.Controls)
                {
                    if (child is PictureBox pic) pic.Image = null;
                }
                ctrl.Dispose();
            }
            flpMovies.Controls.Clear();
            _movieCards.Clear();
        }

        private void GiaiPhongCacheAnh()
        {
            XoaCacCardHienTai();
            foreach (Image img in _posterCache.Values)
            {
                img.Dispose();
            }
            _posterCache.Clear();
        }

        private void Bai4_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _crawlCts?.Cancel();
            _crawlCts?.Dispose();
            GiaiPhongCacheAnh();
            _toolTip.Dispose();
        }
    }
}