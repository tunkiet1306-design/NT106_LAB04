namespace LAB04.Bai07;

public partial class SignUpForm : Form
{
    private readonly FoodApiClient? _apiClient;

    public SignUpForm()
    {
        InitializeComponent();
        cmbLanguage.SelectedIndex = 0;
    }

    public SignUpForm(FoodApiClient apiClient) : this()
    {
        _apiClient = apiClient;
    }

    public string RegisteredUsername { get; private set; } = string.Empty;

    private void btnClear_Click(object? sender, EventArgs e)
    {
        txtUsername.Clear();
        txtPassword.Clear();
        txtEmail.Clear();
        txtFirstName.Clear();
        txtLastName.Clear();
        txtPhone.Clear();
        dtpBirthday.Value = DateTime.Today;
        cmbLanguage.SelectedIndex = 0;
        rdoMale.Checked = true;
        txtUsername.Focus();
    }

    private async void btnSubmit_Click(object? sender, EventArgs e)
    {
        await SubmitSignUpAsync();
    }

    public async Task SubmitSignUpAsync()
    {
        if (_apiClient is null)
        {
            return;
        }

        var username = txtUsername.Text.Trim();
        var password = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show(this, "Username và Password không được để trống.", "Thiếu thông tin",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnSubmit.Enabled = false;
        btnSubmit.Text = "...";
        try
        {
            var request = new SignUpRequest
            {
                Username = username,
                Password = password,
                Email = txtEmail.Text.Trim(),
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Birthday = dtpBirthday.Value.ToString("yyyy-MM-dd"),
                Language = cmbLanguage.SelectedItem?.ToString() ?? "vi",
                Sex = rdoMale.Checked ? 0 : 1
            };

            await _apiClient.SignUpAsync(request);
            RegisteredUsername = username;
            MessageBox.Show(this, $"Tạo tài khoản '{username}' thành công!", "Đăng ký thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Đăng ký thất bại",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSubmit.Enabled = true;
            btnSubmit.Text = "Submit";
        }
    }
}
