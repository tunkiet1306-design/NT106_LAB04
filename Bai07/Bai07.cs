using System.Text.Json;
using System.Text.Json.Serialization;

namespace LAB04.Bai07;

public partial class Bai07 : Form
{
    private const string MealDbSearchUrl = "https://www.themealdb.com/api/json/v1/1/search.php?s=";
    private const string MealDbRandomUrl = "https://www.themealdb.com/api/json/v1/1/random.php";

    private static readonly HttpClient SharedHttpClient = new() { Timeout = TimeSpan.FromSeconds(15) };
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly Dictionary<string, AccountRecord> _accounts = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<DishRecord> _communityDishes = [];
    private readonly List<DishRecord> _personalDishes = [];
    private string? _currentUser;
    private bool _suppressPageEvents;
    private int _currentPage = 1;
    private int _totalPages = 1;

    public Bai07()
    {
        InitializeComponent();
        SeedDefaultData();

        _suppressPageEvents = true;
        cmbPageSize.SelectedIndex = 0;
        _suppressPageEvents = false;
    }

    private void SeedDefaultData()
    {
        _accounts["demo"] = new AccountRecord("demo", "123456", "demo@uit.edu.vn", "Sinh viên UIT");
        _personalDishes.Add(new DishRecord(
            Guid.NewGuid().ToString("N")[..8],
            "Phở Bò Tái Nạm",
            "40,000 VNĐ",
            "45 Võ Văn Ngân, Thủ Đức",
            string.Empty,
            "Phở truyền thống, nước dùng trong đậm vị.",
            "demo"));
        _personalDishes.Add(new DishRecord(
            Guid.NewGuid().ToString("N")[..8],
            "Cơm Tấm Sườn Bì",
            "35,000 VNĐ",
            "Linh Trung, Thủ Đức",
            string.Empty,
            "Sườn nướng mật ong kèm nước mắm kẹo.",
            "demo"));
    }

    private async void Bai07_Load(object? sender, EventArgs e)
    {
        UpdateAuthUi();
        await LoadCommunityDishesAsync();
    }

    private async void bthSearch_Click(object? sender, EventArgs e)
    {
        _currentPage = 1;
        if (tabMain.SelectedTab != tabAll)
        {
            tabMain.SelectedTab = tabAll;
        }
        await LoadCommunityDishesAsync();
    }

    private void tabMain_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _currentPage = 1;
        if (tabMain.SelectedTab == tabAll)
        {
            RenderCurrentPage(_communityDishes, dgvAllDishes);
        }
        else if (tabMain.SelectedTab == tabMine)
        {
            RenderCurrentPage(GetMyDishes(), dgvMyDishes);
        }
        else
        {
            lblTotal.Text = "Quản lý tài khoản";
        }
    }

    private void cmbPage_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressPageEvents || cmbPage.SelectedItem is null)
        {
            return;
        }

        if (int.TryParse(cmbPage.SelectedItem.ToString(), out var page))
        {
            _currentPage = page;
            RefreshActiveTabGrid();
        }
    }

    private void cmbPageSize_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressPageEvents)
        {
            return;
        }

        _currentPage = 1;
        RefreshActiveTabGrid();
    }

    private void bthPrevPage_Click(object? sender, EventArgs e)
    {
        if (_currentPage > 1)
        {
            _currentPage--;
            RefreshActiveTabGrid();
        }
    }

    private void bthNextPage_Click(object? sender, EventArgs e)
    {
        if (_currentPage < _totalPages)
        {
            _currentPage++;
            RefreshActiveTabGrid();
        }
    }

    private async void bthRandom_Click(object? sender, EventArgs e)
    {
        if (tabMain.SelectedTab == tabMine)
        {
            var myDishes = GetMyDishes();
            if (myDishes.Count == 0)
            {
                MessageBox.Show(this, "Bạn chưa có món ăn nào trong danh sách cá nhân (hãy đăng nhập và thêm món).",
                    "Danh sách trống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var pickedMine = myDishes[Random.Shared.Next(myDishes.Count)];
            await ShowDishDetailAsync(pickedMine);
            MessageBox.Show(this, $"Hôm nay ăn: {pickedMine.TenMonAn}!\n{pickedMine.GiaOrCategory} - {pickedMine.DiaChiOrArea}",
                $"Ăn {pickedMine.TenMonAn} đi!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        SetBusy(true);
        try
        {
            var picked = await FetchRandomMealFromApiAsync();
            if (picked is null && _communityDishes.Count > 0)
            {
                picked = _communityDishes[Random.Shared.Next(_communityDishes.Count)];
            }

            if (picked is null)
            {
                MessageBox.Show(this, "Không tìm thấy món ăn nào để chọn ngẫu nhiên.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await ShowDishDetailAsync(picked);
            MessageBox.Show(this, $"Hôm nay ăn: {picked.TenMonAn}!\nDanh mục: {picked.GiaOrCategory}\nKhu vực: {picked.DiaChiOrArea}",
                $"Ăn {picked.TenMonAn} đi!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Lỗi gọi Public API: {ex.Message}",
                "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void bthLogin_Click(object? sender, EventArgs e)
    {
        var username = txtUsername.Text.Trim();
        var password = txtPassword.Text;

        if (!_accounts.TryGetValue(username, out var account) || account.Password != password)
        {
            MessageBox.Show(this, "Sai tên đăng nhập hoặc mật khẩu (tài khoản mẫu: demo / 123456).",
                "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        _currentUser = account.Username;
        UpdateAuthUi();
        MessageBox.Show(this, $"Đăng nhập thành công! Xin chào {account.Username}.",
            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void bthSignUp_Click(object? sender, EventArgs e)
    {
        var username = txtUsername.Text.Trim();
        var password = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show(this, "Vui lòng nhập đầy đủ Username và Password.",
                "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_accounts.ContainsKey(username))
        {
            MessageBox.Show(this, "Tên tài khoản đã tồn tại.",
                "Đăng ký thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _accounts[username] = new AccountRecord(username, password, txtEmail.Text.Trim(), txtFullName.Text.Trim());
        _currentUser = username;
        UpdateAuthUi();
        MessageBox.Show(this, $"Tạo tài khoản '{username}' thành công và đã đăng nhập!",
            "Đăng ký thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void bthLogout_Click(object? sender, EventArgs e)
    {
        _currentUser = null;
        UpdateAuthUi();
        if (tabMain.SelectedTab == tabMine)
        {
            RenderCurrentPage([], dgvMyDishes);
        }
    }

    private void bthThemMon_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_currentUser))
        {
            MessageBox.Show(this, "Vui lòng đăng nhập ở tab Tài khoản trước khi thêm món ăn.",
                "Chưa đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tabMain.SelectedTab = tabAccount;
            return;
        }

        var name = txtTenMon.Text.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show(this, "Vui lòng nhập tên món ăn.",
                "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var price = string.IsNullOrWhiteSpace(txtGia.Text) ? "Đang cập nhật" : txtGia.Text.Trim() + " VNĐ";
        var address = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? "Đang cập nhật" : txtDiaChi.Text.Trim();

        _personalDishes.Insert(0, new DishRecord(
            Guid.NewGuid().ToString("N")[..8],
            name,
            price,
            address,
            txtHinhAnh.Text.Trim(),
            txtMoTa.Text.Trim(),
            _currentUser));

        txtTenMon.Clear();
        txtGia.Clear();
        txtDiaChi.Clear();
        txtHinhAnh.Clear();
        txtMoTa.Clear();

        _currentPage = 1;
        RenderCurrentPage(GetMyDishes(), dgvMyDishes);
    }

    private void bthXoaMon_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_currentUser))
        {
            MessageBox.Show(this, "Vui lòng đăng nhập để xóa món ăn.",
                "Chưa đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (dgvMyDishes.CurrentRow?.DataBoundItem is not DishGridRow selected)
        {
            MessageBox.Show(this, "Vui lòng chọn một món ăn cần xóa.",
                "Chưa chọn món", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show(this, $"Bạn có chắc muốn xóa món \"{selected.TenMonAn}\"?",
            "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes)
        {
            return;
        }

        _personalDishes.RemoveAll(d => d.Id == selected.Id && string.Equals(d.NguoiDongGop, _currentUser, StringComparison.OrdinalIgnoreCase));
        RenderCurrentPage(GetMyDishes(), dgvMyDishes);
    }

    private async void dgvAllDishes_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvAllDishes.CurrentRow?.DataBoundItem is DishGridRow row)
        {
            var match = _communityDishes.FirstOrDefault(d => d.Id == row.Id);
            if (match is not null)
            {
                await ShowDishDetailAsync(match);
            }
        }
    }

    private async void dgvMyDishes_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvMyDishes.CurrentRow?.DataBoundItem is DishGridRow row)
        {
            var match = _personalDishes.FirstOrDefault(d => d.Id == row.Id);
            if (match is not null)
            {
                await ShowDishDetailAsync(match);
            }
        }
    }

    public async Task LoadCommunityDishesAsync()
    {
        SetBusy(true);
        try
        {
            var keyword = Uri.EscapeDataString(txtSearch.Text.Trim());
            var json = await SharedHttpClient.GetStringAsync(MealDbSearchUrl + keyword);
            var parsed = JsonSerializer.Deserialize<MealDbResponse>(json, JsonOptions);

            _communityDishes.Clear();
            if (parsed?.Meals is not null)
            {
                foreach (var meal in parsed.Meals)
                {
                    _communityDishes.Add(new DishRecord(
                        meal.IdMeal ?? Guid.NewGuid().ToString("N")[..8],
                        meal.StrMeal ?? "Unknown",
                        meal.StrCategory ?? "Khác",
                        meal.StrArea ?? meal.StrCountry ?? "Quốc tế",
                        meal.StrMealThumb ?? string.Empty,
                        meal.StrInstructions ?? string.Empty,
                        "TheMealDB"));
                }
            }

            RenderCurrentPage(_communityDishes, dgvAllDishes);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Không thể tải dữ liệu từ TheMealDB API: {ex.Message}",
                "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    public async Task<DishRecord?> FetchRandomMealFromApiAsync()
    {
        var json = await SharedHttpClient.GetStringAsync(MealDbRandomUrl);
        var parsed = JsonSerializer.Deserialize<MealDbResponse>(json, JsonOptions);
        var meal = parsed?.Meals?.FirstOrDefault();
        if (meal is null)
        {
            return null;
        }

        return new DishRecord(
            meal.IdMeal ?? Guid.NewGuid().ToString("N")[..8],
            meal.StrMeal ?? "Unknown",
            meal.StrCategory ?? "Khác",
            meal.StrArea ?? meal.StrCountry ?? "Quốc tế",
            meal.StrMealThumb ?? string.Empty,
            meal.StrInstructions ?? string.Empty,
            "TheMealDB");
    }

    private void RefreshActiveTabGrid()
    {
        if (tabMain.SelectedTab == tabAll)
        {
            RenderCurrentPage(_communityDishes, dgvAllDishes);
        }
        else if (tabMain.SelectedTab == tabMine)
        {
            RenderCurrentPage(GetMyDishes(), dgvMyDishes);
        }
    }

    private List<DishRecord> GetMyDishes() =>
        string.IsNullOrEmpty(_currentUser)
            ? []
            : _personalDishes.Where(d => string.Equals(d.NguoiDongGop, _currentUser, StringComparison.OrdinalIgnoreCase)).ToList();

    private void RenderCurrentPage(List<DishRecord> source, DataGridView grid)
    {
        var pageSize = int.TryParse(cmbPageSize.SelectedItem?.ToString(), out var size) ? size : 5;
        _totalPages = Math.Max(1, (int)Math.Ceiling(source.Count / (double)pageSize));
        if (_currentPage > _totalPages)
        {
            _currentPage = _totalPages;
        }

        var rows = source
            .Skip((_currentPage - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DishGridRow
            {
                Id = d.Id,
                TenMonAn = d.TenMonAn,
                GiaHoacDanhMuc = d.GiaOrCategory,
                DiaChiHoacKhuVuc = d.DiaChiOrArea,
                NguoiDongGop = d.NguoiDongGop
            })
            .ToList();

        grid.DataSource = rows;
        if (grid.Columns.Contains(nameof(DishGridRow.Id)))
        {
            grid.Columns[nameof(DishGridRow.Id)]!.Visible = false;
        }

        lblTotal.Text = $"Tổng: {source.Count} món ăn (Trang {_currentPage}/{_totalPages})";

        _suppressPageEvents = true;
        cmbPage.Items.Clear();
        for (var i = 1; i <= _totalPages; i++)
        {
            cmbPage.Items.Add(i.ToString());
        }
        cmbPage.SelectedIndex = Math.Clamp(_currentPage - 1, 0, Math.Max(0, cmbPage.Items.Count - 1));
        bthPrevPage.Enabled = _currentPage > 1;
        bthNextPage.Enabled = _currentPage < _totalPages;
        _suppressPageEvents = false;
    }

    private async Task ShowDishDetailAsync(DishRecord dish)
    {
        lblDetailName.Text = dish.TenMonAn;
        lblDetailMeta.Text = $"{dish.GiaOrCategory}  |  {dish.DiaChiOrArea}\nĐóng góp: {dish.NguoiDongGop}";
        txtDetailDesc.Text = string.IsNullOrWhiteSpace(dish.MoTa) ? "Không có mô tả." : dish.MoTa;

        if (Uri.TryCreate(dish.HinhAnh, UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            try
            {
                using var stream = await SharedHttpClient.GetStreamAsync(uri);
                if (!picDish.IsDisposed)
                {
                    picDish.Image = Image.FromStream(stream);
                    return;
                }
            }
            catch
            {
                // Fall back to placeholder image if remote thumbnail fails.
            }
        }

        if (!picDish.IsDisposed)
        {
            picDish.Image = CreatePlaceholderImage(dish.TenMonAn);
        }
    }

    private static Bitmap CreatePlaceholderImage(string dishName)
    {
        var bmp = new Bitmap(290, 180);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.FromArgb(255, 243, 224));
        var initial = string.IsNullOrWhiteSpace(dishName) ? "?" : dishName.Trim()[..1].ToUpperInvariant();
        using var font = new Font("Segoe UI", 42F, FontStyle.Bold);
        using var brush = new SolidBrush(Color.FromArgb(180, 52, 43));
        var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(initial, font, brush, new RectangleF(0, 0, 290, 180), format);
        return bmp;
    }

    private void UpdateAuthUi()
    {
        if (string.IsNullOrEmpty(_currentUser))
        {
            lblAuthStatus.ForeColor = Color.Firebrick;
            lblAuthStatus.Text = "Unauthenticated";
            bthLogout.Enabled = false;
        }
        else
        {
            lblAuthStatus.ForeColor = Color.SeaGreen;
            lblAuthStatus.Text = $"Welcome, {_currentUser}";
            bthLogout.Enabled = true;
        }
    }

    private void SetBusy(bool busy)
    {
        prgLoading.Visible = busy;
        bthSearch.Enabled = !busy;
        bthRandom.Enabled = !busy;
    }

    private sealed record AccountRecord(string Username, string Password, string Email, string FullName);

    public sealed record DishRecord(
        string Id,
        string TenMonAn,
        string GiaOrCategory,
        string DiaChiOrArea,
        string HinhAnh,
        string MoTa,
        string NguoiDongGop);

    private sealed class DishGridRow
    {
        public string Id { get; set; } = string.Empty;
        public string TenMonAn { get; set; } = string.Empty;
        public string GiaHoacDanhMuc { get; set; } = string.Empty;
        public string DiaChiHoacKhuVuc { get; set; } = string.Empty;
        public string NguoiDongGop { get; set; } = string.Empty;
    }

    private sealed class MealDbResponse
    {
        [JsonPropertyName("meals")]
        public List<MealDbItem>? Meals { get; set; }
    }

    private sealed class MealDbItem
    {
        [JsonPropertyName("idMeal")]
        public string? IdMeal { get; set; }

        [JsonPropertyName("strMeal")]
        public string? StrMeal { get; set; }

        [JsonPropertyName("strCategory")]
        public string? StrCategory { get; set; }

        [JsonPropertyName("strArea")]
        public string? StrArea { get; set; }

        [JsonPropertyName("strCountry")]
        public string? StrCountry { get; set; }

        [JsonPropertyName("strInstructions")]
        public string? StrInstructions { get; set; }

        [JsonPropertyName("strMealThumb")]
        public string? StrMealThumb { get; set; }
    }
}
