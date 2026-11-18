namespace LAB04.Bai07;

public partial class Bai07 : Form
{
    private static readonly HttpClient ImageHttpClient = new() { Timeout = TimeSpan.FromSeconds(5) };

    private LocalFoodApiServer? _localServer;
    private FoodApiClient _apiClient = new();
    private bool _suppressPageEvents;
    private int _currentPage = 1;
    private int _totalPages = 1;

    public Bai07()
    {
        InitializeComponent();
        _suppressPageEvents = true;
        cmbApiUrl.SelectedIndex = 0;
        cmbPageSize.SelectedIndex = 0;
        _suppressPageEvents = false;
    }

    private bool IsMyDishesTab => tabMain.SelectedTab == tabMine;

    private async void Bai07_Load(object? sender, EventArgs e)
    {
        try
        {
            _localServer = new LocalFoodApiServer();
            _localServer.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this,
                $"Không thể khởi chạy Local REST API ({ex.Message}). Bạn có thể nhập URL server khác ở thanh API URL.",
                "Cảnh báo Local API", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        UpdateAuthStatusUi();
        await LoadDishesAsync();
    }

    private void Bai07_FormClosed(object? sender, FormClosedEventArgs e)
    {
        _apiClient.Dispose();
        _localServer?.Dispose();
    }

    private async void btnConnectApi_Click(object? sender, EventArgs e)
    {
        var url = cmbApiUrl.Text.Trim();
        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
        {
            MessageBox.Show(this, "API URL không hợp lệ.", "Lỗi URL",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _apiClient.SetBaseUrl(url);
        _currentPage = 1;
        UpdateAuthStatusUi();
        await LoadDishesAsync();
    }

    private async void tabMain_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _currentPage = 1;
        await LoadDishesAsync();
    }

    private async void cmbPage_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressPageEvents || cmbPage.SelectedItem is null)
        {
            return;
        }

        if (int.TryParse(cmbPage.SelectedItem.ToString(), out var page))
        {
            _currentPage = page;
            await LoadDishesAsync();
        }
    }

    private async void cmbPageSize_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressPageEvents)
        {
            return;
        }

        _currentPage = 1;
        await LoadDishesAsync();
    }

    private async void btnPrevPage_Click(object? sender, EventArgs e)
    {
        if (_currentPage > 1)
        {
            _currentPage--;
            await LoadDishesAsync();
        }
    }

    private async void btnNextPage_Click(object? sender, EventArgs e)
    {
        if (_currentPage < _totalPages)
        {
            _currentPage++;
            await LoadDishesAsync();
        }
    }

    private async void lnkAuthAction_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        if (_apiClient.IsAuthenticated)
        {
            _apiClient.Logout();
            UpdateAuthStatusUi();
            if (IsMyDishesTab)
            {
                flpMyDishes.Controls.Clear();
            }
            await LoadDishesAsync();
            return;
        }

        if (ShowLoginDialog())
        {
            await LoadDishesAsync();
        }
    }

    private async void btnAddDish_Click(object? sender, EventArgs e)
    {
        if (!_apiClient.IsAuthenticated && !ShowLoginDialog())
        {
            return;
        }

        using var addForm = new ThemMonAnForm(_apiClient);
        if (addForm.ShowDialog(this) == DialogResult.OK)
        {
            _currentPage = 1;
            await LoadDishesAsync();
        }
    }

    private async void btnRandom_Click(object? sender, EventArgs e)
    {
        if (IsMyDishesTab && !_apiClient.IsAuthenticated && !ShowLoginDialog())
        {
            return;
        }

        SetBusy(true);
        try
        {
            var pool = await _apiClient.GetDishesAsync(IsMyDishesTab, 1, 50);
            if (pool.Data.Count == 0)
            {
                MessageBox.Show(this, "Chưa có món ăn nào trong danh sách để chọn ngẫu nhiên.",
                    "Danh sách trống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var picked = pool.Data[Random.Shared.Next(pool.Data.Count)];
            var thumbnail = await LoadDishImageAsync(picked);
            using var resultForm = new DishResultForm(picked, thumbnail);
            resultForm.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Không thể chọn món ngẫu nhiên",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    public async Task LoadDishesAsync()
    {
        var panel = IsMyDishesTab ? flpMyDishes : flpAllDishes;
        panel.Controls.Clear();

        if (IsMyDishesTab && !_apiClient.IsAuthenticated)
        {
            lblTotal.Text = "Vui lòng đăng nhập để xem món ăn bạn đóng góp";
            UpdatePaginationControls(1, 1);
            return;
        }

        var pageSize = GetSelectedPageSize();
        SetBusy(true);
        try
        {
            var result = await _apiClient.GetDishesAsync(IsMyDishesTab, _currentPage, pageSize);
            var total = result.Pagination.Total;
            _totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
            if (_currentPage > _totalPages)
            {
                _currentPage = _totalPages;
            }

            lblTotal.Text = $"Tổng: {total} món ăn (Trang {_currentPage}/{_totalPages})";
            UpdatePaginationControls(_currentPage, _totalPages);

            if (result.Data.Count == 0)
            {
                panel.Controls.Add(new Label
                {
                    AutoSize = true,
                    ForeColor = Color.DimGray,
                    Margin = new Padding(12),
                    Text = "Chưa có món ăn nào."
                });
                return;
            }

            foreach (var dish in result.Data)
            {
                var card = CreateDishCard(dish, panel.ClientSize.Width - 28);
                panel.Controls.Add(card);
            }
        }
        catch (Exception ex)
        {
            lblTotal.Text = "Lỗi tải dữ liệu";
            MessageBox.Show(this, ex.Message, "Lỗi kết nối API",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private Panel CreateDishCard(DishItem dish, int cardWidth)
    {
        var card = new Panel
        {
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(4, 4, 4, 8),
            Size = new Size(Math.Max(600, cardWidth), 108)
        };

        var pic = new PictureBox
        {
            BackColor = Color.FromArgb(241, 245, 249),
            Location = new Point(8, 8),
            Size = new Size(90, 90),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = CreatePlaceholderImage(dish.TenMonAn)
        };

        var lblName = new Label
        {
            AutoEllipsis = true,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(180, 52, 43),
            Location = new Point(108, 8),
            Size = new Size(card.Width - 210, 26),
            Text = dish.TenMonAn
        };

        var lblPrice = new Label
        {
            Location = new Point(108, 38),
            Size = new Size(card.Width - 210, 20),
            Text = $"Giá: {dish.Gia:N0} VNĐ"
        };

        var lblAddress = new Label
        {
            AutoEllipsis = true,
            Location = new Point(108, 59),
            Size = new Size(card.Width - 210, 20),
            Text = $"Địa chỉ: {(string.IsNullOrWhiteSpace(dish.DiaChi) ? "Đang cập nhật" : dish.DiaChi)}"
        };

        var lblContributor = new Label
        {
            ForeColor = Color.SeaGreen,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Location = new Point(108, 80),
            Size = new Size(card.Width - 210, 20),
            Text = $"Đóng góp: {dish.NguoiDongGop}"
        };

        card.Controls.Add(pic);
        card.Controls.Add(lblName);
        card.Controls.Add(lblPrice);
        card.Controls.Add(lblAddress);
        card.Controls.Add(lblContributor);

        var canDelete = _apiClient.IsAuthenticated &&
                        string.Equals(dish.NguoiDongGop, _apiClient.CurrentUser?.Username, StringComparison.OrdinalIgnoreCase);

        if (canDelete)
        {
            var btnDelete = new Button
            {
                BackColor = Color.FromArgb(254, 226, 226),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Firebrick,
                Location = new Point(card.Width - 88, 36),
                Size = new Size(74, 30),
                Text = "Xóa",
                UseVisualStyleBackColor = false
            };
            btnDelete.Click += async (_, _) => await DeleteDishWithConfirmAsync(dish);
            card.Controls.Add(btnDelete);
        }

        _ = PopulateThumbnailAsync(pic, dish);
        return card;
    }

    private async Task DeleteDishWithConfirmAsync(DishItem dish)
    {
        var confirm = MessageBox.Show(this,
            $"Bạn có chắc muốn xóa món \"{dish.TenMonAn}\"?",
            "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        SetBusy(true);
        try
        {
            await _apiClient.DeleteDishAsync(dish.Id);
            await LoadDishesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Xóa món ăn thất bại",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task PopulateThumbnailAsync(PictureBox pic, DishItem dish)
    {
        var img = await LoadDishImageAsync(dish);
        if (!pic.IsDisposed && img is not null)
        {
            pic.Image = img;
        }
    }

    private static async Task<Image?> LoadDishImageAsync(DishItem dish)
    {
        if (Uri.TryCreate(dish.HinhAnh, UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            try
            {
                using var stream = await ImageHttpClient.GetStreamAsync(uri);
                return Image.FromStream(stream);
            }
            catch
            {
                // Fall back to generated placeholder image.
            }
        }

        return CreatePlaceholderImage(dish.TenMonAn);
    }

    private static Bitmap CreatePlaceholderImage(string dishName)
    {
        var bmp = new Bitmap(90, 90);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.FromArgb(255, 243, 224));
        using var borderPen = new Pen(Color.FromArgb(245, 194, 120), 2);
        g.DrawRectangle(borderPen, 1, 1, 87, 87);

        var initial = string.IsNullOrWhiteSpace(dishName) ? "?" : dishName.Trim()[..1].ToUpperInvariant();
        using var font = new Font("Segoe UI", 26F, FontStyle.Bold);
        using var brush = new SolidBrush(Color.FromArgb(180, 52, 43));
        var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(initial, font, brush, new RectangleF(0, 0, 90, 90), format);
        return bmp;
    }

    private bool ShowLoginDialog()
    {
        using var loginForm = new LoginForm(_apiClient);
        if (loginForm.ShowDialog(this) == DialogResult.OK)
        {
            UpdateAuthStatusUi();
            return true;
        }

        return false;
    }

    private void UpdateAuthStatusUi()
    {
        if (_apiClient.IsAuthenticated && _apiClient.CurrentUser is not null)
        {
            lblAuthStatus.ForeColor = Color.SeaGreen;
            lblAuthStatus.Text = $"Welcome, {_apiClient.CurrentUser.Username}";
            lnkAuthAction.Text = "Logout";
        }
        else
        {
            lblAuthStatus.ForeColor = Color.Firebrick;
            lblAuthStatus.Text = "Unauthenticated";
            lnkAuthAction.Text = "Login";
        }

        lnkAuthAction.Left = lblAuthStatus.Right + 10;
    }

    private void UpdatePaginationControls(int currentPage, int totalPages)
    {
        _suppressPageEvents = true;
        cmbPage.Items.Clear();
        for (var i = 1; i <= totalPages; i++)
        {
            cmbPage.Items.Add(i.ToString());
        }
        cmbPage.SelectedIndex = Math.Clamp(currentPage - 1, 0, Math.Max(0, cmbPage.Items.Count - 1));
        btnPrevPage.Enabled = currentPage > 1;
        btnNextPage.Enabled = currentPage < totalPages;
        _suppressPageEvents = false;
    }

    private int GetSelectedPageSize() =>
        int.TryParse(cmbPageSize.SelectedItem?.ToString(), out var size) ? size : 5;

    private void SetBusy(bool busy)
    {
        prgLoading.Visible = busy;
        btnRandom.Enabled = !busy;
        btnAddDish.Enabled = !busy;
        btnConnectApi.Enabled = !busy;
    }
}
