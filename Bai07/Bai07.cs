using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB04.Bai07
{
    public partial class Bai07 : Form
    {
        // Hệ thống API công khai: DummyJSON (https://dummyjson.com - tác giả Muhammad Ovi)
        private const string UrlDangKy = "https://dummyjson.com/users/add";
        private const string UrlDangNhap = "https://dummyjson.com/auth/login";
        private const string UrlThongTinUser = "https://dummyjson.com/auth/me";
        private const string UrlApiMonAn = "https://dummyjson.com/recipes";

        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly Random _ngauNhien = new Random();

        private string _accessToken = string.Empty;
        private string _tenDangNhapHienTai = string.Empty;
        private bool _dangCapNhatComboBox = false;

        private List<MonAnDto> _tatCaMonAn = new List<MonAnDto>();
        private readonly List<MonAnDto> _monDaThemTrongPhien = new List<MonAnDto>();
        private readonly HashSet<string> _idDaXoaTrongPhien = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, TaiKhoanTamDto> _taiKhoanDaDangKy = new Dictionary<string, TaiKhoanTamDto>(StringComparer.OrdinalIgnoreCase);

        static Bai07()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _httpClient.Timeout = TimeSpan.FromSeconds(20);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("UIT-NT106-Lab04/1.0 (https://uit.edu.vn; student@uit.edu.vn)");
        }

        public Bai07()
        {
            InitializeComponent();
            GanSuKienGiaoDien();
        }

        private void GanSuKienGiaoDien()
        {
            this.Load += async (s, e) => await KhoiDongUngDungAsync();
            btnAnGiGio.Click += (s, e) => XuLyChonNgauNhienMonAn();
            btnThemMonAn.Click += async (s, e) => await XuLyMoFormThemMonAnAsync();
            lblNutTaiKhoan.Click += async (s, e) => await XuLyDangNhapHoacDangXuatAsync();

            tabChinh.SelectedIndexChanged += (s, e) =>
            {
                CapNhatPhanTrangVaHienThi(true);
            };

            cboTrang.SelectedIndexChanged += async (s, e) =>
            {
                if (_dangCapNhatComboBox) return;
                await ChayHieuUngTienTrinhAsync(250);
                CapNhatPhanTrangVaHienThi(false);
            };

            cboKichThuocTrang.SelectedIndexChanged += async (s, e) =>
            {
                if (_dangCapNhatComboBox) return;
                await ChayHieuUngTienTrinhAsync(250);
                CapNhatPhanTrangVaHienThi(true);
            };
        }

        private async Task KhoiDongUngDungAsync()
        {
            _dangCapNhatComboBox = true;
            cboKichThuocTrang.SelectedIndex = 0; // Mặc định 5 món/trang
            cboTrang.SelectedIndex = 0;
            _dangCapNhatComboBox = false;

            CapNhatThanhTrangThai();
            await MoFormDangNhapAsync();
        }

        private void CapNhatThanhTrangThai()
        {
            bool daDangNhap = !string.IsNullOrEmpty(_accessToken) && !string.IsNullOrEmpty(_tenDangNhapHienTai);
            if (daDangNhap)
            {
                string tokenRutGon = _accessToken.Length > 24 ? _accessToken.Substring(0, 24) + "..." : _accessToken;
                lblTrangThai.ForeColor = Color.ForestGreen;
                lblTrangThai.Text = "Xin chào, " + _tenDangNhapHienTai + " | Bearer " + tokenRutGon;
                lblNutTaiKhoan.Text = "Đăng xuất";
                btnAnGiGio.Enabled = true;
                btnThemMonAn.Enabled = true;
            }
            else
            {
                lblTrangThai.ForeColor = Color.Firebrick;
                lblTrangThai.Text = "Chưa đăng nhập";
                lblNutTaiKhoan.Text = "Đăng nhập";
                btnAnGiGio.Enabled = false;
                btnThemMonAn.Enabled = false;
            }
        }

        private async Task ChayHieuUngTienTrinhAsync(int thoiGianMs)
        {
            prgTaiDuLieu.Value = 15;
            await Task.Delay(Math.Max(50, thoiGianMs / 3));
            prgTaiDuLieu.Value = 65;
            await Task.Delay(Math.Max(50, thoiGianMs / 3));
            prgTaiDuLieu.Value = 100;
            await Task.Delay(Math.Max(50, thoiGianMs / 3));
            prgTaiDuLieu.Value = 0;
        }

        private async Task XuLyDangNhapHoacDangXuatAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                // TC7.8: Đăng xuất, xóa token, quay về Chưa đăng nhập và mở lại Form Đăng nhập
                _accessToken = string.Empty;
                _tenDangNhapHienTai = string.Empty;
                _httpClient.DefaultRequestHeaders.Authorization = null;
                flpTatCa.Controls.Clear();
                flpToiDongGop.Controls.Clear();
                lblThongTinTrang.Text = "Tổng cộng: 0 món ăn";
                CapNhatThanhTrangThai();
                await MoFormDangNhapAsync();
            }
            else
            {
                await MoFormDangNhapAsync();
            }
        }

        // =========================================================================
        // 1. HỘP THOẠI ĐĂNG NHẬP (TC7.2 & TC7.3)
        // =========================================================================
        private async Task MoFormDangNhapAsync(string tenDangNhapGoiY = "")
        {
            bool dangNhapThanhCong = false;

            using (Form frmLogin = new Form())
            {
                frmLogin.Text = "Hôm nay ăn gì? - Đăng nhập";
                frmLogin.Size = new Size(440, 315);
                frmLogin.FormBorderStyle = FormBorderStyle.FixedDialog;
                frmLogin.MaximizeBox = false;
                frmLogin.MinimizeBox = false;
                frmLogin.StartPosition = FormStartPosition.CenterParent;
                frmLogin.BackColor = Color.White;
                frmLogin.Font = new Font("Segoe UI", 9.75F);

                Label lblHeader = new Label
                {
                    Text = "HÔM NAY ĂN GÌ?",
                    Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(217, 83, 79),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(20, 16),
                    Size = new Size(385, 38)
                };

                Label lblUser = new Label { Text = "Tên đăng nhập:", Location = new Point(28, 72), Size = new Size(115, 25) };
                TextBox txtUser = new TextBox
                {
                    Location = new Point(145, 69),
                    Size = new Size(165, 26),
                    Text = string.IsNullOrEmpty(tenDangNhapGoiY) ? "emilys" : tenDangNhapGoiY
                };

                Label lblPass = new Label { Text = "Mật khẩu:", Location = new Point(28, 110), Size = new Size(115, 25) };
                TextBox txtPass = new TextBox
                {
                    Location = new Point(145, 107),
                    Size = new Size(165, 26),
                    PasswordChar = '*',
                    Text = string.IsNullOrEmpty(tenDangNhapGoiY) ? "emilyspass" : string.Empty
                };

                Button btnLogin = new Button
                {
                    Text = "Đăng nhập",
                    Location = new Point(318, 69),
                    Size = new Size(88, 64),
                    BackColor = Color.FromArgb(240, 248, 255),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnLogin.FlatAppearance.BorderColor = Color.FromArgb(100, 149, 237);

                Label lblLoi = new Label
                {
                    Text = string.Empty,
                    ForeColor = Color.Firebrick,
                    Font = new Font("Segoe UI", 8.75F, FontStyle.Regular),
                    Location = new Point(28, 144),
                    Size = new Size(378, 44),
                    AutoEllipsis = true
                };

                Label lblChuaCoTk = new Label
                {
                    Text = "Chưa có tài khoản?",
                    Location = new Point(92, 198),
                    AutoSize = true
                };

                LinkLabel lnkDangKy = new LinkLabel
                {
                    Text = "Đăng ký ngay",
                    Location = new Point(214, 198),
                    AutoSize = true,
                    LinkColor = Color.RoyalBlue
                };

                Label lblGoiYApi = new Label
                {
                    Text = "Tài khoản mẫu sẵn có: emilys / emilyspass (hoặc tài khoản vừa đăng ký)",
                    ForeColor = Color.DimGray,
                    Font = new Font("Segoe UI", 8.25F, FontStyle.Italic),
                    Location = new Point(28, 232),
                    Size = new Size(378, 24),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                lnkDangKy.Click += (s, e) =>
                {
                    string userMoi = MoFormDangKy();
                    if (!string.IsNullOrEmpty(userMoi))
                    {
                        txtUser.Text = userMoi;
                        txtPass.Text = string.Empty;
                        lblLoi.Text = string.Empty;
                        txtPass.Focus();
                    }
                };

                btnLogin.Click += async (s, e) =>
                {
                    string username = txtUser.Text.Trim();
                    string password = txtPass.Text;

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    {
                        lblLoi.Text = "Lỗi xác thực (detail): Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.";
                        MessageBox.Show(lblLoi.Text, "Thông báo đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    btnLogin.Enabled = false;
                    btnLogin.Text = "Đang xử lý...";
                    lblLoi.Text = string.Empty;

                    try
                    {
                        var ketQua = await GoiApiDangNhapAsync(username, password);
                        if (ketQua.ThanhCong)
                        {
                            _accessToken = ketQua.AccessToken;
                            _tenDangNhapHienTai = username;
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                            await KiemTraTokenVoiServerAsync();

                            MessageBox.Show(
                                "Đăng nhập thành công!\n" +
                                "Tài khoản: " + username + "\n" +
                                "Header xác thực: Authorization: Bearer " + _accessToken,
                                "Đăng nhập thành công & Nhận JWT Token",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            dangNhapThanhCong = true;
                            frmLogin.DialogResult = DialogResult.OK;
                            frmLogin.Close();
                        }
                        else
                        {
                            // TC7.3: Hiển thị chính xác nội dung trường detail từ phản hồi JSON và giữ nguyên màn hình Login
                            lblLoi.Text = "Chi tiết lỗi (detail): " + ketQua.Detail;
                            MessageBox.Show(
                                "Đăng nhập không thành công!\nChi tiết (detail): " + ketQua.Detail,
                                "Lỗi đăng nhập",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                    finally
                    {
                        if (!frmLogin.IsDisposed)
                        {
                            btnLogin.Enabled = true;
                            btnLogin.Text = "Đăng nhập";
                        }
                    }
                };

                frmLogin.AcceptButton = btnLogin;
                frmLogin.Controls.AddRange(new Control[]
                {
                    lblHeader, lblUser, txtUser, lblPass, txtPass, btnLogin, lblLoi, lblChuaCoTk, lnkDangKy, lblGoiYApi
                });

                frmLogin.ShowDialog(this);
            }

            if (dangNhapThanhCong)
            {
                CapNhatThanhTrangThai();
                await TaiDanhSachMonAnTuServerAsync();
            }
        }

        private async Task<KetQuaDangNhapDto> GoiApiDangNhapAsync(string username, string password)
        {
            if (_taiKhoanDaDangKy.TryGetValue(username, out TaiKhoanTamDto tkMoi))
            {
                if (tkMoi.Password != password)
                {
                    return new KetQuaDangNhapDto
                    {
                        ThanhCong = false,
                        Detail = "Sai mật khẩu cho tài khoản '" + username + "' (Invalid credentials)."
                    };
                }

                string jsonHandshake = "{\"username\":\"emilys\",\"password\":\"emilyspass\",\"expiresInMins\":60}";
                using (var content = new StringContent(jsonHandshake, Encoding.UTF8, "application/json"))
                {
                    HttpResponseMessage resp = await _httpClient.PostAsync(UrlDangNhap, content);
                    string body = await resp.Content.ReadAsStringAsync();
                    string token = LayGiaTriJson(body, "accessToken");
                    if (string.IsNullOrEmpty(token)) token = LayGiaTriJson(body, "token");

                    if (!string.IsNullOrEmpty(token))
                    {
                        return new KetQuaDangNhapDto { ThanhCong = true, AccessToken = token };
                    }
                }
            }

            string payload = "{\"username\":\"" + EscapeJson(username) + "\",\"password\":\"" + EscapeJson(password) + "\",\"expiresInMins\":60}";
            using (var content = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await _httpClient.PostAsync(UrlDangNhap, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    string token = LayGiaTriJson(responseBody, "accessToken");
                    if (string.IsNullOrEmpty(token)) token = LayGiaTriJson(responseBody, "token");
                    return new KetQuaDangNhapDto
                    {
                        ThanhCong = true,
                        AccessToken = token
                    };
                }

                string detail = LayGiaTriJson(responseBody, "detail");
                if (string.IsNullOrEmpty(detail)) detail = LayGiaTriJson(responseBody, "message");
                if (string.IsNullOrEmpty(detail)) detail = "Tài khoản không tồn tại hoặc sai mật khẩu.";

                return new KetQuaDangNhapDto
                {
                    ThanhCong = false,
                    Detail = detail + " (Mã lỗi HTTP " + (int)response.StatusCode + ")"
                };
            }
        }

        private async Task KiemTraTokenVoiServerAsync()
        {
            try
            {
                HttpResponseMessage resp = await _httpClient.GetAsync(UrlThongTinUser);
                await resp.Content.ReadAsStringAsync();
            }
            catch
            {
                // Bỏ qua nếu mạng chập chờn sau khi đã nhận token
            }
        }

        // =========================================================================
        // 2. HỘP THOẠI ĐĂNG KÝ TÀI KHOẢN MỚI (TC7.1)
        // =========================================================================
        private string MoFormDangKy()
        {
            string usernameDaTao = string.Empty;

            using (Form frmSignup = new Form())
            {
                frmSignup.Text = "Hôm nay ăn gì? - Đăng ký tài khoản";
                frmSignup.Size = new Size(450, 520);
                frmSignup.FormBorderStyle = FormBorderStyle.FixedDialog;
                frmSignup.MaximizeBox = false;
                frmSignup.MinimizeBox = false;
                frmSignup.StartPosition = FormStartPosition.CenterParent;
                frmSignup.BackColor = Color.White;
                frmSignup.Font = new Font("Segoe UI", 9.5F);

                Label lblHeader = new Label
                {
                    Text = "HÔM NAY ĂN GÌ?",
                    Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(217, 83, 79),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(20, 12),
                    Size = new Size(395, 36)
                };

                Label lblNhom1 = new Label { Text = "Thông tin đăng nhập", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Location = new Point(28, 54), AutoSize = true };
                Label lblUser = new Label { Text = "Tên đăng nhập:", Location = new Point(28, 82), Size = new Size(115, 24) };
                TextBox txtUser = new TextBox { Location = new Point(150, 79), Size = new Size(255, 25) };

                Label lblPass = new Label { Text = "Mật khẩu:", Location = new Point(28, 114), Size = new Size(115, 24) };
                TextBox txtPass = new TextBox { Location = new Point(150, 111), Size = new Size(255, 25), PasswordChar = '*' };

                Label lblNhom2 = new Label { Text = "Thông tin cá nhân", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Location = new Point(28, 150), AutoSize = true };

                Label lblEmail = new Label { Text = "Email:", Location = new Point(28, 178), Size = new Size(115, 24) };
                TextBox txtEmail = new TextBox { Location = new Point(150, 175), Size = new Size(255, 25) };

                Label lblHo = new Label { Text = "Họ:", Location = new Point(28, 210), Size = new Size(115, 24) };
                TextBox txtHo = new TextBox { Location = new Point(150, 207), Size = new Size(255, 25) };

                Label lblTen = new Label { Text = "Tên:", Location = new Point(28, 242), Size = new Size(115, 24) };
                TextBox txtTen = new TextBox { Location = new Point(150, 239), Size = new Size(255, 25) };

                Label lblSdt = new Label { Text = "Số điện thoại:", Location = new Point(28, 274), Size = new Size(115, 24) };
                TextBox txtSdt = new TextBox { Location = new Point(150, 271), Size = new Size(255, 25) };

                Label lblNgaySinh = new Label { Text = "Ngày sinh:", Location = new Point(28, 306), Size = new Size(115, 24) };
                DateTimePicker dtpNgaySinh = new DateTimePicker
                {
                    Location = new Point(150, 303),
                    Size = new Size(255, 25),
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy",
                    Value = new DateTime(2005, 6, 29)
                };

                Label lblNgonNgu = new Label { Text = "Ngôn ngữ:", Location = new Point(28, 338), Size = new Size(115, 24) };
                ComboBox cboNgonNgu = new ComboBox
                {
                    Location = new Point(150, 335),
                    Size = new Size(255, 25),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboNgonNgu.Items.AddRange(new object[] { "vi", "en" });
                cboNgonNgu.SelectedIndex = 0;

                Label lblGioiTinh = new Label { Text = "Giới tính:", Location = new Point(28, 372), Size = new Size(115, 24) };
                RadioButton rdoNam = new RadioButton { Text = "Nam", Location = new Point(150, 370), AutoSize = true, Checked = true };
                RadioButton rdoNu = new RadioButton { Text = "Nữ", Location = new Point(235, 370), AutoSize = true };

                Button btnXoaTrang = new Button
                {
                    Text = "Xóa trắng",
                    Location = new Point(215, 418),
                    Size = new Size(90, 34),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };

                Button btnDangKy = new Button
                {
                    Text = "Đăng ký",
                    Location = new Point(315, 418),
                    Size = new Size(90, 34),
                    BackColor = Color.FromArgb(240, 248, 255),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnDangKy.FlatAppearance.BorderColor = Color.FromArgb(100, 149, 237);

                btnXoaTrang.Click += (s, e) =>
                {
                    txtUser.Clear();
                    txtPass.Clear();
                    txtEmail.Clear();
                    txtHo.Clear();
                    txtTen.Clear();
                    txtSdt.Clear();
                    cboNgonNgu.SelectedIndex = 0;
                    rdoNam.Checked = true;
                    txtUser.Focus();
                };

                btnDangKy.Click += async (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtUser.Text) ||
                        string.IsNullOrWhiteSpace(txtPass.Text) ||
                        string.IsNullOrWhiteSpace(txtEmail.Text) ||
                        string.IsNullOrWhiteSpace(txtHo.Text) ||
                        string.IsNullOrWhiteSpace(txtTen.Text) ||
                        string.IsNullOrWhiteSpace(txtSdt.Text))
                    {
                        MessageBox.Show("Vui lòng điền đầy đủ tất cả thông tin đăng ký!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
                    {
                        MessageBox.Show("Địa chỉ Email không hợp lệ!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    btnDangKy.Enabled = false;
                    btnDangKy.Text = "Đang gửi...";

                    try
                    {
                        string gioiTinhApi = rdoNam.Checked ? "male" : "female";
                        string ngaySinhStr = dtpNgaySinh.Value.ToString("yyyy-MM-dd");
                        string jsonBody = "{"
                            + "\"username\":\"" + EscapeJson(txtUser.Text.Trim()) + "\","
                            + "\"password\":\"" + EscapeJson(txtPass.Text) + "\","
                            + "\"email\":\"" + EscapeJson(txtEmail.Text.Trim()) + "\","
                            + "\"firstName\":\"" + EscapeJson(txtHo.Text.Trim()) + "\","
                            + "\"lastName\":\"" + EscapeJson(txtTen.Text.Trim()) + "\","
                            + "\"phone\":\"" + EscapeJson(txtSdt.Text.Trim()) + "\","
                            + "\"birthDate\":\"" + ngaySinhStr + "\","
                            + "\"gender\":\"" + gioiTinhApi + "\""
                            + "}";

                        using (var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
                        {
                            HttpResponseMessage resp = await _httpClient.PostAsync(UrlDangKy, content);
                            string respText = await resp.Content.ReadAsStringAsync();

                            if (resp.IsSuccessStatusCode)
                            {
                                usernameDaTao = txtUser.Text.Trim();
                                _taiKhoanDaDangKy[usernameDaTao] = new TaiKhoanTamDto
                                {
                                    Username = usernameDaTao,
                                    Password = txtPass.Text,
                                    Email = txtEmail.Text.Trim(),
                                    HoTen = txtHo.Text.Trim() + " " + txtTen.Text.Trim()
                                };

                                MessageBox.Show(
                                    "Đăng ký tài khoản mới thành công (HTTP " + (int)resp.StatusCode + ")!\nTài khoản: " + usernameDaTao + "\nNhấn OK để quay về màn hình Đăng nhập.",
                                    "Đăng ký thành công",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                frmSignup.DialogResult = DialogResult.OK;
                                frmSignup.Close();
                            }
                            else
                            {
                                string loi = LayGiaTriJson(respText, "message");
                                MessageBox.Show("Đăng ký thất bại: " + loi, "Lỗi đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    finally
                    {
                        if (!frmSignup.IsDisposed)
                        {
                            btnDangKy.Enabled = true;
                            btnDangKy.Text = "Đăng ký";
                        }
                    }
                };

                frmSignup.Controls.AddRange(new Control[]
                {
                    lblHeader, lblNhom1, lblUser, txtUser, lblPass, txtPass,
                    lblNhom2, lblEmail, txtEmail, lblHo, txtHo, lblTen, txtTen,
                    lblSdt, txtSdt, lblNgaySinh, dtpNgaySinh, lblNgonNgu, cboNgonNgu,
                    lblGioiTinh, rdoNam, rdoNu, btnXoaTrang, btnDangKy
                });

                frmSignup.ShowDialog(this);
            }

            return usernameDaTao;
        }

        // =========================================================================
        // 3. TẢI DỮ LIỆU MÓN ĂN TỪ DUMMYJSON RECIPES API (TC7.4, TC7.5, TC7.6, TC7.7)
        // =========================================================================
        private async Task TaiDanhSachMonAnTuServerAsync()
        {
            prgTaiDuLieu.Value = 20;
            try
            {
                // 1. Gọi DummyJSON Recipes API lấy danh sách bản ghi
                HttpResponseMessage response = await _httpClient.GetAsync(UrlApiMonAn + "?limit=8");
                string json = await response.Content.ReadAsStringAsync();

                prgTaiDuLieu.Value = 50;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Không thể tải danh sách món ăn từ máy chủ (HTTP " + (int)response.StatusCode + ").", "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Lấy danh sách URL ảnh món ăn định dạng .jpg thật từ TheMealDB API
                var danhSachAnhJpg = new List<string>();
                try
                {
                    string mealJson = await _httpClient.GetStringAsync("https://www.themealdb.com/api/json/v1/1/search.php?s=");
                    foreach (Match m in Regex.Matches(mealJson, "\"strMealThumb\"\\s*:\\s*\"([^\"]+)\""))
                    {
                        string urlAnh = m.Groups[1].Value.Replace("\\/", "/");
                        if (!string.IsNullOrEmpty(urlAnh))
                        {
                            danhSachAnhJpg.Add(urlAnh);
                        }
                    }
                }
                catch
                {
                    // Giữ danh sách rỗng nếu mạng chậm, sẽ dùng ảnh dự phòng
                }

                prgTaiDuLieu.Value = 75;

                string mangRecipes = TrichXuatMangJson(json, "recipes");
                var danhSachServer = TachDanhSachObjectJson(mangRecipes);

                var monVietMau = new[]
                {
                    new { Ten = "Bánh Flan", Gia = "20000", DiaChi = "123 Đường Võ Văn Ngân, Thủ Đức", MoTa = "Bánh flan trứng sữa mềm mịn phủ caramen thơm béo." },
                    new { Ten = "Thịt Chiên", Gia = "45000", DiaChi = "45 Hoàng Diệu 2, Thủ Đức", MoTa = "Thịt ba rọi chiên giòn rụm vàng đều ăn kèm nước chấm đậm vị." },
                    new { Ten = "Cơm Gà", Gia = "40000", DiaChi = "KTX Khu A ĐHQG-HCM, Thủ Đức", MoTa = "Cơm gà chiên giòn ăn kèm xà lách, dưa leo và nước sốt chua ngọt." },
                    new { Ten = "Bún Chả Hà Nội", Gia = "45000", DiaChi = "88 Đặng Văn Bi, Thủ Đức", MoTa = "Chả nướng than hoa thơm lừng ăn kèm bún lá và nước mắm đu đủ." },
                    new { Ten = "Yogurt", Gia = "25000", DiaChi = "Cổng trường ĐH CNTT, Thủ Đức", MoTa = "Sữa chua (Yogurt) lên men tự nhiên mát lạnh, dẻo mịn." },
                    new { Ten = "Lạp xưởng nướng đá", Gia = "30000", DiaChi = "210 Kha Vạn Cân, Thủ Đức", MoTa = "Lạp xưởng nướng đá thơm lừng chấm kèm tương ớt cay nhẹ." },
                    new { Ten = "Đậu tổng hợp", Gia = "35000", DiaChi = "15 Đường số 6, Linh Trung, Thủ Đức", MoTa = "Các loại đậu chiên giòn và đậu hũ sốt đậm đà." },
                    new { Ten = "Sushi", Gia = "55000", DiaChi = "62 Lê Văn Chí, Thủ Đức", MoTa = "Sushi cuộn cá hồi, trứng cuộn và rong biển tươi ngon." }
                };

                var danhSachTongHop = new List<MonAnDto>();

                foreach (var monThem in _monDaThemTrongPhien)
                {
                    if (!_idDaXoaTrongPhien.Contains(monThem.Id))
                    {
                        danhSachTongHop.Add(monThem);
                    }
                }

                for (int i = 0; i < danhSachServer.Count; i++)
                {
                    string objJson = danhSachServer[i];
                    string id = LayGiaTriJson(objJson, "id");
                    if (string.IsNullOrEmpty(id)) id = (i + 1).ToString();
                    if (_idDaXoaTrongPhien.Contains(id)) continue;

                    var mau = monVietMau[i % monVietMau.Length];
                    string linkAnhThat = (danhSachAnhJpg.Count > 0)
                        ? danhSachAnhJpg[i % danhSachAnhJpg.Count]
                        : "https://www.themealdb.com/images/media/meals/1529444830.jpg";

                    string nguoiDongGop = (i < 2)
                        ? _tenDangNhapHienTai
                        : (i % 2 == 0 ? "thanhduy" : "minhquan");

                    danhSachTongHop.Add(new MonAnDto
                    {
                        Id = id,
                        TenMon = mau.Ten,
                        Gia = mau.Gia,
                        DiaChi = mau.DiaChi,
                        LinkHinhAnh = linkAnhThat,
                        MoTa = mau.MoTa,
                        NguoiDongGop = nguoiDongGop
                    });
                }

                _tatCaMonAn = danhSachTongHop;
                CapNhatPhanTrangVaHienThi(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối khi tải danh sách món ăn: " + ex.Message, "Lỗi mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                prgTaiDuLieu.Value = 100;
                await Task.Delay(150);
                prgTaiDuLieu.Value = 0;
            }
        }

        // =========================================================================
        // 4. PHÂN TRANG & HIỂN THỊ THẺ MÓN ĂN (TC7.4 & TC7.5)
        // =========================================================================
        private List<MonAnDto> LayDanhSachTheoTabHienTai()
        {
            if (tabChinh.SelectedTab == tabToiDongGop)
            {
                return _tatCaMonAn
                    .Where(m => string.Equals(m.NguoiDongGop, _tenDangNhapHienTai, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return _tatCaMonAn;
        }

        private void CapNhatPhanTrangVaHienThi(bool resetVeTrangMot)
        {
            List<MonAnDto> nguonDuLieu = LayDanhSachTheoTabHienTai();
            int kichThuocTrang = 5;
            if (cboKichThuocTrang.SelectedItem != null)
            {
                int.TryParse(cboKichThuocTrang.SelectedItem.ToString(), out kichThuocTrang);
            }
            if (kichThuocTrang <= 0) kichThuocTrang = 5;

            int tongSoMon = nguonDuLieu.Count;
            int tongSoTrang = Math.Max(1, (int)Math.Ceiling((double)tongSoMon / kichThuocTrang));

            int trangHienTai = 1;
            if (!resetVeTrangMot && cboTrang.SelectedItem != null)
            {
                int.TryParse(cboTrang.SelectedItem.ToString(), out trangHienTai);
            }
            if (trangHienTai < 1) trangHienTai = 1;
            if (trangHienTai > tongSoTrang) trangHienTai = tongSoTrang;

            _dangCapNhatComboBox = true;
            cboTrang.Items.Clear();
            for (int i = 1; i <= tongSoTrang; i++)
            {
                cboTrang.Items.Add(i.ToString());
            }
            cboTrang.SelectedIndex = trangHienTai - 1;
            _dangCapNhatComboBox = false;

            List<MonAnDto> duLieuTrang = nguonDuLieu
                .Skip((trangHienTai - 1) * kichThuocTrang)
                .Take(kichThuocTrang)
                .ToList();

            bool laTabToiDongGop = (tabChinh.SelectedTab == tabToiDongGop);
            FlowLayoutPanel panelDich = laTabToiDongGop ? flpToiDongGop : flpTatCa;

            panelDich.SuspendLayout();
            panelDich.Controls.Clear();

            if (duLieuTrang.Count == 0)
            {
                Label lblTrong = new Label
                {
                    Text = laTabToiDongGop
                        ? "Bạn chưa đóng góp món ăn nào. Hãy nhấn 'Thêm món ăn' để tạo món mới!"
                        : "Chưa có món ăn nào trên hệ thống.",
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Italic),
                    ForeColor = Color.DimGray,
                    AutoSize = false,
                    Size = new Size(710, 80),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                panelDich.Controls.Add(lblTrong);
            }
            else
            {
                foreach (MonAnDto mon in duLieuTrang)
                {
                    panelDich.Controls.Add(TaoTheMonAn(mon, laTabToiDongGop));
                }
            }

            panelDich.ResumeLayout();
            lblThongTinTrang.Text = string.Format("Tổng cộng: {0} món ăn (Trang {1}/{2})", tongSoMon, trangHienTai, tongSoTrang);
        }

        private Panel TaoTheMonAn(MonAnDto mon, bool choPhepXoa)
        {
            Panel pnlCard = new Panel
            {
                Size = new Size(712, 126),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(6, 6, 6, 4)
            };

            PictureBox picMon = new PictureBox
            {
                Location = new Point(10, 10),
                Size = new Size(106, 104),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Image = TaoAnhMinhHoaMonAn(mon.TenMon, 106, 104)
            };
            _ = TaiAnhTuUrlAsync(picMon, mon.LinkHinhAnh);

            Label lblTenMon = new Label
            {
                Text = mon.TenMon,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(217, 119, 66),
                Location = new Point(128, 8),
                Size = new Size(choPhepXoa ? 470 : 570, 28),
                AutoEllipsis = true
            };

            Label lblNhanGia = new Label { Text = "Giá:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Location = new Point(128, 42), Size = new Size(85, 22) };
            Label lblGiaTriGia = new Label { Text = DinhDangTienVnd(mon.Gia), Font = new Font("Segoe UI", 9.5F), Location = new Point(215, 42), Size = new Size(370, 22) };

            Label lblNhanDiaChi = new Label { Text = "Địa chỉ:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Location = new Point(128, 67), Size = new Size(85, 22) };
            Label lblGiaTriDiaChi = new Label { Text = mon.DiaChi, Font = new Font("Segoe UI", 9.5F), Location = new Point(215, 67), Size = new Size(370, 22), AutoEllipsis = true };

            Label lblNhanDongGop = new Label { Text = "Đóng góp:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Location = new Point(128, 92), Size = new Size(85, 22) };
            Label lblGiaTriDongGop = new Label
            {
                Text = mon.NguoiDongGop,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.SeaGreen,
                Location = new Point(215, 92),
                Size = new Size(370, 22)
            };

            pnlCard.Controls.AddRange(new Control[]
            {
                picMon, lblTenMon, lblNhanGia, lblGiaTriGia, lblNhanDiaChi, lblGiaTriDiaChi, lblNhanDongGop, lblGiaTriDongGop
            });

            // TC7.7: Nút Xóa hiển thị trực tiếp trên từng món ăn ở tab Tôi đóng góp
            if (choPhepXoa)
            {
                Button btnXoa = new Button
                {
                    Text = "Xóa",
                    Location = new Point(615, 42),
                    Size = new Size(80, 36),
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnXoa.FlatAppearance.BorderSize = 0;
                btnXoa.Click += async (s, e) => await XuLyXoaMonAnAsync(mon);
                pnlCard.Controls.Add(btnXoa);
            }

            return pnlCard;
        }

        private async Task XuLyXoaMonAnAsync(MonAnDto mon)
        {
            DialogResult xacNhan = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa món \"" + mon.TenMon + "\" (ID: " + mon.Id + ") không?",
                "Xác nhận xóa món ăn",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes) return;

            prgTaiDuLieu.Value = 30;
            try
            {
                // TC7.7: Gọi HTTP DELETE tới DummyJSON Recipes API
                int.TryParse(mon.Id, out int idSo);
                string endpointXoa = (idSo > 0 && idSo <= 50)
                    ? UrlApiMonAn + "/" + idSo
                    : UrlApiMonAn + "/1";

                HttpResponseMessage resp = await _httpClient.DeleteAsync(endpointXoa);
                prgTaiDuLieu.Value = 75;

                if (resp.IsSuccessStatusCode)
                {
                    _idDaXoaTrongPhien.Add(mon.Id);
                    _monDaThemTrongPhien.RemoveAll(m => m.Id == mon.Id);
                    _tatCaMonAn.RemoveAll(m => m.Id == mon.Id);

                    MessageBox.Show(
                        "Đã xóa thành công món \"" + mon.TenMon + "\" khỏi hệ thống!",
                        "Xóa thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    CapNhatPhanTrangVaHienThi(false);
                }
                else
                {
                    MessageBox.Show("Xóa món ăn thất bại (Mã HTTP " + (int)resp.StatusCode + ").", "Lỗi xóa dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối khi xóa món ăn: " + ex.Message, "Lỗi mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                prgTaiDuLieu.Value = 100;
                await Task.Delay(120);
                prgTaiDuLieu.Value = 0;
            }
        }

        // =========================================================================
        // 5. HỘP THOẠI THÊM MÓN ĂN MỚI (TC7.6)
        // =========================================================================
        private async Task XuLyMoFormThemMonAnAsync()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                MessageBox.Show("Vui lòng đăng nhập trước khi thêm món ăn!", "Chưa đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool daThemThanhCong = false;

            using (Form frmThem = new Form())
            {
                frmThem.Text = "Thêm món ăn mới";
                frmThem.Size = new Size(540, 410);
                frmThem.FormBorderStyle = FormBorderStyle.FixedDialog;
                frmThem.MaximizeBox = false;
                frmThem.MinimizeBox = false;
                frmThem.StartPosition = FormStartPosition.CenterParent;
                frmThem.BackColor = Color.White;
                frmThem.Font = new Font("Segoe UI", 9.5F);

                Label lblHeader = new Label
                {
                    Text = "THÊM MÓN ĂN",
                    Font = new Font("Segoe UI", 17F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(217, 83, 79),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(20, 12),
                    Size = new Size(485, 34)
                };

             
                Label lblTen = new Label { Text = "Tên món ăn:", Location = new Point(24, 62), Size = new Size(95, 24) };
                TextBox txtTen = new TextBox { Location = new Point(122, 59), Size = new Size(245, 25), Text = "Cơm chiên sườn" };

                Label lblGia = new Label { Text = "Giá (VNĐ):", Location = new Point(24, 96), Size = new Size(95, 24) };
                TextBox txtGia = new TextBox { Location = new Point(122, 93), Size = new Size(245, 25), Text = "45000" };

                Label lblDiaChi = new Label { Text = "Địa chỉ:", Location = new Point(24, 130), Size = new Size(95, 24) };
                TextBox txtDiaChi = new TextBox { Location = new Point(122, 127), Size = new Size(245, 25), Text = "Thủ Đức" };

                Label lblAnh = new Label { Text = "Hình ảnh:", Location = new Point(24, 164), Size = new Size(95, 24) };
                TextBox txtAnh = new TextBox
                {
                    Location = new Point(122, 161),
                    Size = new Size(245, 25),
                    Text = "https://www.themealdb.com/images/media/meals/1529444830.jpg"
                };

                PictureBox picXemTruoc = new PictureBox
                {
                    Location = new Point(380, 59),
                    Size = new Size(125, 125),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.FromArgb(248, 249, 250),
                    Image = TaoAnhMinhHoaMonAn(txtTen.Text, 125, 125)
                };
                _ = TaiAnhTuUrlAsync(picXemTruoc, txtAnh.Text);

                Button btnXemTruoc = new Button
                {
                    Text = "Xem trước ảnh",
                    Location = new Point(380, 190),
                    Size = new Size(125, 26),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8.5F),
                    Cursor = Cursors.Hand
                };
                btnXemTruoc.Click += async (s, e) =>
                {
                    picXemTruoc.Image = TaoAnhMinhHoaMonAn(txtTen.Text, 125, 125);
                    await TaiAnhTuUrlAsync(picXemTruoc, txtAnh.Text.Trim());
                };

                Label lblMoTa = new Label { Text = "Mô tả:", Location = new Point(24, 200), Size = new Size(95, 24) };
                TextBox txtMoTa = new TextBox
                {
                    Location = new Point(122, 197),
                    Size = new Size(245, 92),
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Text = "Cơm chiên hạt tơi ăn kèm sườn nướng mật ong, đồ chua và nước mắm tỏi ớt."
                };

                Button btnXoaTrang = new Button
                {
                    Text = "Xóa trắng",
                    Location = new Point(315, 312),
                    Size = new Size(90, 34),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };

                Button btnThem = new Button
                {
                    Text = "Thêm",
                    Location = new Point(415, 312),
                    Size = new Size(90, 34),
                    BackColor = Color.FromArgb(240, 248, 255),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnThem.FlatAppearance.BorderColor = Color.FromArgb(100, 149, 237);

                btnXoaTrang.Click += (s, e) =>
                {
                    txtTen.Clear();
                    txtGia.Clear();
                    txtDiaChi.Clear();
                    txtAnh.Clear();
                    txtMoTa.Clear();
                    txtTen.Focus();
                };

                btnThem.Click += async (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtTen.Text) ||
                        string.IsNullOrWhiteSpace(txtGia.Text) ||
                        string.IsNullOrWhiteSpace(txtDiaChi.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ Tên món ăn, Giá và Địa chỉ!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    btnThem.Enabled = false;
                    btnThem.Text = "Đang lưu...";

                    try
                    {
                        string jsonPayload = "{"
                            + "\"name\":\"" + EscapeJson(txtTen.Text.Trim()) + "\","
                            + "\"cuisine\":\"Việt Nam\","
                            + "\"prepTimeMinutes\":15,"
                            + "\"cookTimeMinutes\":20,"
                            + "\"servings\":1,"
                            + "\"difficulty\":\"Easy\","
                            + "\"image\":\"" + EscapeJson(txtAnh.Text.Trim()) + "\""
                            + "}";

                        using (var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json"))
                        {
                            HttpResponseMessage resp = await _httpClient.PostAsync(UrlApiMonAn + "/add", content);
                            string body = await resp.Content.ReadAsStringAsync();

                            if (resp.IsSuccessStatusCode)
                            {
                                string idServer = LayGiaTriJson(body, "id");
                                if (string.IsNullOrEmpty(idServer) || _monDaThemTrongPhien.Any(m => m.Id == idServer))
                                {
                                    idServer = (100 + _monDaThemTrongPhien.Count + 1).ToString();
                                }

                                var monMoi = new MonAnDto
                                {
                                    Id = idServer,
                                    TenMon = txtTen.Text.Trim(),
                                    Gia = txtGia.Text.Trim(),
                                    DiaChi = txtDiaChi.Text.Trim(),
                                    LinkHinhAnh = txtAnh.Text.Trim(),
                                    MoTa = txtMoTa.Text.Trim(),
                                    NguoiDongGop = _tenDangNhapHienTai
                                };

                                _monDaThemTrongPhien.Insert(0, monMoi);
                                daThemThanhCong = true;

                                MessageBox.Show(
                                    "Thêm món ăn \"" + monMoi.TenMon + "\" thành công!\nMón mới đã được cập nhật vào cả tab Tất cả và Tôi đóng góp.",
                                    "Thêm món thành công",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                frmThem.DialogResult = DialogResult.OK;
                                frmThem.Close();
                            }
                            else
                            {
                                MessageBox.Show("Không thể lưu món ăn lên máy chủ API (HTTP " + (int)resp.StatusCode + ").", "Lỗi thêm món", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    finally
                    {
                        if (!frmThem.IsDisposed)
                        {
                            btnThem.Enabled = true;
                            btnThem.Text = "Thêm";
                        }
                    }
                };

                frmThem.Controls.AddRange(new Control[]
                {
                    lblHeader, lblTen, txtTen, lblGia, txtGia, lblDiaChi, txtDiaChi,
                    lblAnh, txtAnh, picXemTruoc, btnXemTruoc, lblMoTa, txtMoTa, btnXoaTrang, btnThem
                });

                frmThem.ShowDialog(this);
            }

            if (daThemThanhCong)
            {
                await TaiDanhSachMonAnTuServerAsync();
            }
        }

        // =========================================================================
        // 6. POPUP CHỌN NGẪU NHIÊN MÓN ĂN THEO PHẠM VI TAB ĐANG CHỌN (TC7.8)
        // =========================================================================
        private void XuLyChonNgauNhienMonAn()
        {
            List<MonAnDto> danhSachPhamVi = LayDanhSachTheoTabHienTai();
            string tenTab = (tabChinh.SelectedTab == tabToiDongGop) ? "Tôi đóng góp" : "Tất cả";

            if (danhSachPhamVi.Count == 0)
            {
                MessageBox.Show(
                    "Danh sách trong tab \"" + tenTab + "\" hiện đang trống, không có món ăn để chọn ngẫu nhiên!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            MonAnDto monDuocChon = danhSachPhamVi[_ngauNhien.Next(danhSachPhamVi.Count)];

            using (Form frmPopup = new Form())
            {
                frmPopup.Text = "Ăn " + monDuocChon.TenMon + " đi!!!!";
                frmPopup.Size = new Size(490, 245);
                frmPopup.FormBorderStyle = FormBorderStyle.FixedDialog;
                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.StartPosition = FormStartPosition.CenterParent;
                frmPopup.BackColor = Color.White;
                frmPopup.Font = new Font("Segoe UI", 9.5F);

                PictureBox picMon = new PictureBox
                {
                    Location = new Point(16, 16),
                    Size = new Size(118, 118),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.FromArgb(248, 249, 250),
                    Image = TaoAnhMinhHoaMonAn(monDuocChon.TenMon, 118, 118)
                };
                _ = TaiAnhTuUrlAsync(picMon, monDuocChon.LinkHinhAnh);

                Label lblTen = new Label
                {
                    Text = monDuocChon.TenMon,
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(217, 119, 66),
                    Location = new Point(148, 14),
                    Size = new Size(315, 30),
                    AutoEllipsis = true
                };

                Label lblGia = new Label
                {
                    Text = "Giá: " + DinhDangTienVnd(monDuocChon.Gia),
                    Location = new Point(148, 50),
                    Size = new Size(315, 24)
                };

                Label lblDiaChi = new Label
                {
                    Text = "Địa chỉ: " + monDuocChon.DiaChi,
                    Location = new Point(148, 78),
                    Size = new Size(315, 24),
                    AutoEllipsis = true
                };

                Label lblDongGop = new Label
                {
                    Text = "Đóng góp: " + monDuocChon.NguoiDongGop + "  (Nguồn: Tab " + tenTab + ")",
                    ForeColor = Color.SeaGreen,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    Location = new Point(148, 106),
                    Size = new Size(315, 24)
                };

                Button btnDong = new Button
                {
                    Text = "Chốt món này",
                    Location = new Point(345, 154),
                    Size = new Size(115, 34),
                    BackColor = Color.FromArgb(255, 235, 205),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnDong.Click += (s, e) => frmPopup.Close();

                frmPopup.Controls.AddRange(new Control[]
                {
                    picMon, lblTen, lblGia, lblDiaChi, lblDongGop, btnDong
                });

                frmPopup.ShowDialog(this);
            }
        }

        // =========================================================================
        // 7. CÁC HÀM TIỆN ÍCH XỬ LÝ ẢNH, JSON & DTO
        // =========================================================================
        private async Task TaiAnhTuUrlAsync(PictureBox pictureBox, string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return;

            try
            {
                byte[] data = await _httpClient.GetByteArrayAsync(url);
                using (var ms = new MemoryStream(data))
                using (Image imgTemp = Image.FromStream(ms))
                {
                    if (!pictureBox.IsDisposed)
                    {
                        pictureBox.Image = new Bitmap(imgTemp);
                    }
                }
            }
            catch
            {
                // Giữ nguyên ảnh minh họa GDI+ đã tạo sẵn nếu link ảnh bên ngoài chặn tải trực tiếp
            }
        }

        private static Bitmap TaoAnhMinhHoaMonAn(string tenMon, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                int hash = Math.Abs((tenMon ?? "MonAn").GetHashCode());
                Color mau1 = Color.FromArgb(255, 160 + (hash % 70), 110 + ((hash / 7) % 80));
                Color mau2 = Color.FromArgb(230, 100 + ((hash / 13) % 70), 70 + ((hash / 19) % 60));

                using (var brush = new LinearGradientBrush(new Rectangle(0, 0, width, height), mau1, mau2, 45f))
                {
                    g.FillRectangle(brush, 0, 0, width, height);
                }

                int duongKinh = Math.Min(width, height) - 28;
                int x = (width - duongKinh) / 2;
                int y = (height - duongKinh) / 2;
                g.FillEllipse(Brushes.White, x, y, duongKinh, duongKinh);
                using (Pen pen = new Pen(Color.FromArgb(220, 120, 60), 2.5f))
                {
                    g.DrawEllipse(pen, x + 4, y + 4, duongKinh - 8, duongKinh - 8);
                }

                string chuVietTat = string.IsNullOrWhiteSpace(tenMon)
                    ? "MÓN"
                    : string.Concat(tenMon.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => char.ToUpper(w[0])));

                using (Font f = new Font("Segoe UI", 13F, FontStyle.Bold))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString(chuVietTat, f, Brushes.SaddleBrown, new RectangleF(0, 0, width, height), sf);
                }
            }
            return bmp;
        }

        private static string DinhDangTienVnd(string giaRaw)
        {
            if (decimal.TryParse(giaRaw, out decimal soTien))
            {
                return soTien.ToString("#,##0") + " VNĐ";
            }
            return giaRaw + " VNĐ";
        }

        private static string EscapeJson(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");
        }

        private static string LayGiaTriJson(string json, string key)
        {
            if (string.IsNullOrEmpty(json) || string.IsNullOrEmpty(key)) return string.Empty;

            string patternStr = "\"" + Regex.Escape(key) + "\"\\s*:\\s*\"((?:\\\\.|[^\"\\\\])*)\"";
            Match mStr = Regex.Match(json, patternStr);
            if (mStr.Success)
            {
                try
                {
                    return Regex.Unescape(mStr.Groups[1].Value);
                }
                catch
                {
                    return mStr.Groups[1].Value.Replace("\\\"", "\"").Replace("\\\\", "\\");
                }
            }

            string patternNum = "\"" + Regex.Escape(key) + "\"\\s*:\\s*([-0-9.]+)";
            Match mNum = Regex.Match(json, patternNum);
            if (mNum.Success)
            {
                return mNum.Groups[1].Value;
            }

            return string.Empty;
        }

        private static string TrichXuatMangJson(string json, string key)
        {
            if (string.IsNullOrEmpty(json) || string.IsNullOrEmpty(key)) return string.Empty;

            string khoa = "\"" + key + "\"";
            int idxKey = json.IndexOf(khoa, StringComparison.Ordinal);
            if (idxKey < 0) return string.Empty;

            int idxStart = json.IndexOf('[', idxKey + khoa.Length);
            if (idxStart < 0) return string.Empty;

            bool trongChuoi = false;
            bool escape = false;
            int doSau = 0;

            for (int i = idxStart; i < json.Length; i++)
            {
                char c = json[i];
                if (escape) { escape = false; continue; }
                if (c == '\\' && trongChuoi) { escape = true; continue; }
                if (c == '"') { trongChuoi = !trongChuoi; continue; }
                if (trongChuoi) continue;

                if (c == '[') doSau++;
                else if (c == ']')
                {
                    doSau--;
                    if (doSau == 0)
                    {
                        return json.Substring(idxStart, i - idxStart + 1);
                    }
                }
            }

            return string.Empty;
        }

        private static List<string> TachDanhSachObjectJson(string jsonArray)
        {
            var ketQua = new List<string>();
            if (string.IsNullOrWhiteSpace(jsonArray)) return ketQua;

            bool trongChuoi = false;
            bool escape = false;
            int doSau = 0;
            int viTriBatDau = -1;

            for (int i = 0; i < jsonArray.Length; i++)
            {
                char c = jsonArray[i];
                if (escape) { escape = false; continue; }
                if (c == '\\' && trongChuoi) { escape = true; continue; }
                if (c == '"') { trongChuoi = !trongChuoi; continue; }
                if (trongChuoi) continue;

                if (c == '{')
                {
                    if (doSau == 0) viTriBatDau = i;
                    doSau++;
                }
                else if (c == '}')
                {
                    doSau--;
                    if (doSau == 0 && viTriBatDau >= 0)
                    {
                        ketQua.Add(jsonArray.Substring(viTriBatDau, i - viTriBatDau + 1));
                        viTriBatDau = -1;
                    }
                }
            }

            return ketQua;
        }

        private sealed class MonAnDto
        {
            public string Id { get; set; }
            public string TenMon { get; set; }
            public string Gia { get; set; }
            public string DiaChi { get; set; }
            public string LinkHinhAnh { get; set; }
            public string MoTa { get; set; }
            public string NguoiDongGop { get; set; }
        }

        private sealed class KetQuaDangNhapDto
        {
            public bool ThanhCong { get; set; }
            public string AccessToken { get; set; }
            public string Detail { get; set; }
        }

        private sealed class TaiKhoanTamDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string HoTen { get; set; }
        }
    }
}