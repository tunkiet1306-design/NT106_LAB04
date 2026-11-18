namespace LAB04.Bai07;

partial class SignUpForm
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
        lblSignUp = new Label();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        lblUserInfo = new Label();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblFirstName = new Label();
        txtFirstName = new TextBox();
        lblLastName = new Label();
        txtLastName = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblBirthday = new Label();
        dtpBirthday = new DateTimePicker();
        lblLanguage = new Label();
        cmbLanguage = new ComboBox();
        lblSex = new Label();
        rdoMale = new RadioButton();
        rdoFemale = new RadioButton();
        btnClear = new Button();
        btnSubmit = new Button();
        SuspendLayout();
        // 
        // lblHeader
        // 
        lblHeader.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblHeader.ForeColor = Color.FromArgb(180, 52, 43);
        lblHeader.Location = new Point(20, 16);
        lblHeader.Name = "lblHeader";
        lblHeader.Size = new Size(340, 36);
        lblHeader.TabIndex = 0;
        lblHeader.Text = "HÔM NAY ĂN GÌ?";
        lblHeader.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblSignUp
        // 
        lblSignUp.AutoSize = true;
        lblSignUp.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblSignUp.Location = new Point(24, 62);
        lblSignUp.Name = "lblSignUp";
        lblSignUp.Size = new Size(55, 17);
        lblSignUp.TabIndex = 1;
        lblSignUp.Text = "Sign Up";
        // 
        // lblUsername
        // 
        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(24, 92);
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(60, 15);
        lblUsername.TabIndex = 2;
        lblUsername.Text = "Username";
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(112, 89);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(238, 23);
        txtUsername.TabIndex = 3;
        // 
        // lblPassword
        // 
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(24, 124);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(57, 15);
        lblPassword.TabIndex = 4;
        lblPassword.Text = "Password";
        // 
        // txtPassword
        // 
        txtPassword.Location = new Point(112, 121);
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new Size(238, 23);
        txtPassword.TabIndex = 5;
        txtPassword.UseSystemPasswordChar = true;
        // 
        // lblUserInfo
        // 
        lblUserInfo.AutoSize = true;
        lblUserInfo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblUserInfo.Location = new Point(24, 160);
        lblUserInfo.Name = "lblUserInfo";
        lblUserInfo.Size = new Size(109, 17);
        lblUserInfo.TabIndex = 6;
        lblUserInfo.Text = "User Information";
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(24, 190);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(36, 15);
        lblEmail.TabIndex = 7;
        lblEmail.Text = "Email";
        // 
        // txtEmail
        // 
        txtEmail.Location = new Point(112, 187);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(238, 23);
        txtEmail.TabIndex = 8;
        // 
        // lblFirstName
        // 
        lblFirstName.AutoSize = true;
        lblFirstName.Location = new Point(24, 222);
        lblFirstName.Name = "lblFirstName";
        lblFirstName.Size = new Size(60, 15);
        lblFirstName.TabIndex = 9;
        lblFirstName.Text = "Firstname";
        // 
        // txtFirstName
        // 
        txtFirstName.Location = new Point(112, 219);
        txtFirstName.Name = "txtFirstName";
        txtFirstName.Size = new Size(238, 23);
        txtFirstName.TabIndex = 10;
        // 
        // lblLastName
        // 
        lblLastName.AutoSize = true;
        lblLastName.Location = new Point(24, 254);
        lblLastName.Name = "lblLastName";
        lblLastName.Size = new Size(58, 15);
        lblLastName.TabIndex = 11;
        lblLastName.Text = "Lastname";
        // 
        // txtLastName
        // 
        txtLastName.Location = new Point(112, 251);
        txtLastName.Name = "txtLastName";
        txtLastName.Size = new Size(238, 23);
        txtLastName.TabIndex = 12;
        // 
        // lblPhone
        // 
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(24, 286);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(41, 15);
        lblPhone.TabIndex = 13;
        lblPhone.Text = "Phone";
        // 
        // txtPhone
        // 
        txtPhone.Location = new Point(112, 283);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(238, 23);
        txtPhone.TabIndex = 14;
        // 
        // lblBirthday
        // 
        lblBirthday.AutoSize = true;
        lblBirthday.Location = new Point(24, 318);
        lblBirthday.Name = "lblBirthday";
        lblBirthday.Size = new Size(51, 15);
        lblBirthday.TabIndex = 15;
        lblBirthday.Text = "Birthday";
        // 
        // dtpBirthday
        // 
        dtpBirthday.Format = DateTimePickerFormat.Short;
        dtpBirthday.Location = new Point(112, 315);
        dtpBirthday.Name = "dtpBirthday";
        dtpBirthday.Size = new Size(238, 23);
        dtpBirthday.TabIndex = 16;
        // 
        // lblLanguage
        // 
        lblLanguage.AutoSize = true;
        lblLanguage.Location = new Point(24, 350);
        lblLanguage.Name = "lblLanguage";
        lblLanguage.Size = new Size(59, 15);
        lblLanguage.TabIndex = 17;
        lblLanguage.Text = "Language";
        // 
        // cmbLanguage
        // 
        cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbLanguage.FormattingEnabled = true;
        cmbLanguage.Items.AddRange(["vi", "en"]);
        cmbLanguage.Location = new Point(112, 347);
        cmbLanguage.Name = "cmbLanguage";
        cmbLanguage.Size = new Size(238, 23);
        cmbLanguage.TabIndex = 18;
        // 
        // lblSex
        // 
        lblSex.AutoSize = true;
        lblSex.Location = new Point(24, 384);
        lblSex.Name = "lblSex";
        lblSex.Size = new Size(25, 15);
        lblSex.TabIndex = 19;
        lblSex.Text = "Sex";
        // 
        // rdoMale
        // 
        rdoMale.AutoSize = true;
        rdoMale.Checked = true;
        rdoMale.Location = new Point(112, 382);
        rdoMale.Name = "rdoMale";
        rdoMale.Size = new Size(51, 19);
        rdoMale.TabIndex = 20;
        rdoMale.TabStop = true;
        rdoMale.Text = "Male";
        rdoMale.UseVisualStyleBackColor = true;
        // 
        // rdoFemale
        // 
        rdoFemale.AutoSize = true;
        rdoFemale.Location = new Point(200, 382);
        rdoFemale.Name = "rdoFemale";
        rdoFemale.Size = new Size(63, 19);
        rdoFemale.TabIndex = 21;
        rdoFemale.Text = "Female";
        rdoFemale.UseVisualStyleBackColor = true;
        // 
        // btnClear
        // 
        btnClear.Location = new Point(168, 420);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(85, 30);
        btnClear.TabIndex = 22;
        btnClear.Text = "Clear";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;
        // 
        // btnSubmit
        // 
        btnSubmit.Location = new Point(265, 420);
        btnSubmit.Name = "btnSubmit";
        btnSubmit.Size = new Size(85, 30);
        btnSubmit.TabIndex = 23;
        btnSubmit.Text = "Submit";
        btnSubmit.UseVisualStyleBackColor = true;
        btnSubmit.Click += btnSubmit_Click;
        // 
        // SignUpForm
        // 
        AcceptButton = btnSubmit;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(380, 470);
        Controls.Add(btnSubmit);
        Controls.Add(btnClear);
        Controls.Add(rdoFemale);
        Controls.Add(rdoMale);
        Controls.Add(lblSex);
        Controls.Add(cmbLanguage);
        Controls.Add(lblLanguage);
        Controls.Add(dtpBirthday);
        Controls.Add(lblBirthday);
        Controls.Add(txtPhone);
        Controls.Add(lblPhone);
        Controls.Add(txtLastName);
        Controls.Add(lblLastName);
        Controls.Add(txtFirstName);
        Controls.Add(lblFirstName);
        Controls.Add(txtEmail);
        Controls.Add(lblEmail);
        Controls.Add(lblUserInfo);
        Controls.Add(txtPassword);
        Controls.Add(lblPassword);
        Controls.Add(txtUsername);
        Controls.Add(lblUsername);
        Controls.Add(lblSignUp);
        Controls.Add(lblHeader);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SignUpForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Hôm nay ăn gì? - Signup";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblHeader = null!;
    private Label lblSignUp = null!;
    private Label lblUsername = null!;
    private TextBox txtUsername = null!;
    private Label lblPassword = null!;
    private TextBox txtPassword = null!;
    private Label lblUserInfo = null!;
    private Label lblEmail = null!;
    private TextBox txtEmail = null!;
    private Label lblFirstName = null!;
    private TextBox txtFirstName = null!;
    private Label lblLastName = null!;
    private TextBox txtLastName = null!;
    private Label lblPhone = null!;
    private TextBox txtPhone = null!;
    private Label lblBirthday = null!;
    private DateTimePicker dtpBirthday = null!;
    private Label lblLanguage = null!;
    private ComboBox cmbLanguage = null!;
    private Label lblSex = null!;
    private RadioButton rdoMale = null!;
    private RadioButton rdoFemale = null!;
    private Button btnClear = null!;
    private Button btnSubmit = null!;
}
