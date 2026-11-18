namespace LAB04.Bai07;

partial class LoginForm
{
    private System.ComponentModel.IContainer? components = null;

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
        lblHeader = new Label();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        btnLogin = new Button();
        lblPrompt = new Label();
        lnkSignUp = new LinkLabel();
        lblHint = new Label();
        SuspendLayout();
        // 
        // lblHeader
        // 
        lblHeader.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblHeader.ForeColor = Color.FromArgb(180, 52, 43);
        lblHeader.Location = new Point(24, 20);
        lblHeader.Name = "lblHeader";
        lblHeader.Size = new Size(340, 36);
        lblHeader.TabIndex = 0;
        lblHeader.Text = "HÔM NAY ĂN GÌ?";
        lblHeader.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblUsername
        // 
        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(28, 76);
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(60, 15);
        lblUsername.TabIndex = 1;
        lblUsername.Text = "Username";
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(105, 73);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(170, 23);
        txtUsername.TabIndex = 2;
        txtUsername.Text = "demo";
        // 
        // lblPassword
        // 
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(28, 114);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(57, 15);
        lblPassword.TabIndex = 3;
        lblPassword.Text = "Password";
        // 
        // txtPassword
        // 
        txtPassword.Location = new Point(105, 111);
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new Size(170, 23);
        txtPassword.TabIndex = 4;
        txtPassword.Text = "123456";
        txtPassword.UseSystemPasswordChar = true;
        // 
        // btnLogin
        // 
        btnLogin.Location = new Point(288, 73);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(76, 61);
        btnLogin.TabIndex = 5;
        btnLogin.Text = "Login";
        btnLogin.UseVisualStyleBackColor = true;
        btnLogin.Click += btnLogin_Click;
        // 
        // lblPrompt
        // 
        lblPrompt.AutoSize = true;
        lblPrompt.Location = new Point(84, 155);
        lblPrompt.Name = "lblPrompt";
        lblPrompt.Size = new Size(141, 15);
        lblPrompt.TabIndex = 6;
        lblPrompt.Text = "Don't have an account yet?";
        // 
        // lnkSignUp
        // 
        lnkSignUp.AutoSize = true;
        lnkSignUp.Location = new Point(228, 155);
        lnkSignUp.Name = "lnkSignUp";
        lnkSignUp.Size = new Size(47, 15);
        lnkSignUp.TabIndex = 7;
        lnkSignUp.TabStop = true;
        lnkSignUp.Text = "Sign up";
        lnkSignUp.LinkClicked += lnkSignUp_LinkClicked;
        // 
        // lblHint
        // 
        lblHint.ForeColor = Color.DimGray;
        lblHint.Location = new Point(28, 182);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(336, 20);
        lblHint.TabIndex = 8;
        lblHint.Text = "Tài khoản mẫu: demo / 123456";
        lblHint.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // LoginForm
        // 
        AcceptButton = btnLogin;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(388, 218);
        Controls.Add(lblHint);
        Controls.Add(lnkSignUp);
        Controls.Add(lblPrompt);
        Controls.Add(btnLogin);
        Controls.Add(txtPassword);
        Controls.Add(lblPassword);
        Controls.Add(txtUsername);
        Controls.Add(lblUsername);
        Controls.Add(lblHeader);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Hôm nay ăn gì? - Login";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblHeader = null!;
    private Label lblUsername = null!;
    private TextBox txtUsername = null!;
    private Label lblPassword = null!;
    private TextBox txtPassword = null!;
    private Button btnLogin = null!;
    private Label lblPrompt = null!;
    private LinkLabel lnkSignUp = null!;
    private Label lblHint = null!;
}
