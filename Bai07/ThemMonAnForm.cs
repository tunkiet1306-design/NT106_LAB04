namespace LAB04.Bai07;

public partial class ThemMonAnForm : Form
{
    private readonly FoodApiClient? _apiClient;

    public ThemMonAnForm()
    {
        InitializeComponent();
    }

    public ThemMonAnForm(FoodApiClient apiClient) : this()
    {
        _apiClient = apiClient;
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        txtTenMonAn.Clear();
        txtGia.Clear();
        txtDiaChi.Clear();
        txtHinhAnh.Clear();
        txtMoTa.Clear();
        txtTenMonAn.Focus();
    }

    private async void btnThem_Click(object? sender, EventArgs e)
    {
        await AddDishAsync();
    }

    public async Task AddDishAsync()
    {
        if (_apiClient is null)
        {
            return;
        }

        var name = txtTenMonAn.Text.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show(this, "Vui lòng nhập tên món ăn.", "Thiếu thông tin",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!decimal.TryParse(txtGia.Text.Trim(), out var price) || price < 0)
        {
            MessageBox.Show(this, "Giá món ăn phải là số hợp lệ.", "Sai định dạng",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnThem.Enabled = false;
        btnThem.Text = "...";
        try
        {
            await _apiClient.AddDishAsync(new CreateDishRequest
            {
                TenMonAn = name,
                Gia = price,
                DiaChi = txtDiaChi.Text.Trim(),
                HinhAnh = txtHinhAnh.Text.Trim(),
                MoTa = txtMoTa.Text.Trim()
            });

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Thêm món ăn thất bại",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnThem.Enabled = true;
            btnThem.Text = "Thêm";
        }
    }
}
