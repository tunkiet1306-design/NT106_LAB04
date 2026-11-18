namespace LAB04.Bai07;

public partial class LoginForm : Form
{
    private readonly FoodApiClient? _apiClient;

    public LoginForm()
    {
        InitializeComponent();
    }

    public LoginForm(FoodApiClient apiClient) : this()
    {
        _apiClient = apiClient;
    }

    private async void btnLogin_Click(object? sender, EventArgs e)
    {
        await PerformLoginAsync();
    }

    public async Task PerformLoginAsync()
    {
        if (_apiClient is null)
        {
            return;
        }

        var username = txtUsername.Text.Trim();
        var password = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show(this, "Vui lòng nhập đầy đủ Username và Password.", "Thiếu thông tin",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnLogin.Enabled = false;
        btnLogin.Text = "...";
        try
        {
            await _apiClient.LoginAsync(username, password);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Đăng nhập thất bại",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnLogin.Enabled = true;
            btnLogin.Text = "Login";
        }
    }

    private void lnkSignUp_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        if (_apiClient is null)
        {
            return;
        }

        using var signUp = new SignUpForm(_apiClient);
        if (signUp.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(signUp.RegisteredUsername))
        {
            txtUsername.Text = signUp.RegisteredUsername;
            txtPassword.Clear();
            txtPassword.Focus();
        }
    }
}
