using System.Windows.Forms;

namespace Loan_Amortization
{
    public partial class DatabaseConnectionForm : Form
    {
        public string ConnectionString { get; private set; } = string.Empty;

        private TextBox serverTextBox;
        private TextBox databaseTextBox;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button okButton;
        private Button cancelButton;

        public DatabaseConnectionForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Database Connection";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 244, 248);

            CreateControls();
        }

        private void CreateControls()
        {
            // Server
            var serverLabel = new Label
            {
                Text = "Server:",
                Location = new Point(20, 30),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10)
            };
            serverTextBox = new TextBox
            {
                Location = new Point(110, 28),
                Size = new Size(250, 25),
                Text = "localhost",
                Font = new Font("Segoe UI", 10)
            };

            // Database
            var databaseLabel = new Label
            {
                Text = "Database:",
                Location = new Point(20, 70),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10)
            };
            databaseTextBox = new TextBox
            {
                Location = new Point(110, 68),
                Size = new Size(250, 25),
                Text = "loan_db",
                Font = new Font("Segoe UI", 10)
            };

            // Username
            var usernameLabel = new Label
            {
                Text = "Username:",
                Location = new Point(20, 110),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10)
            };
            usernameTextBox = new TextBox
            {
                Location = new Point(110, 108),
                Size = new Size(250, 25),
                Text = "root",
                Font = new Font("Segoe UI", 10)
            };

            // Password
            var passwordLabel = new Label
            {
                Text = "Password:",
                Location = new Point(20, 150),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10)
            };
            passwordTextBox = new TextBox
            {
                Location = new Point(110, 148),
                Size = new Size(250, 25),
                UseSystemPasswordChar = true,
                Font = new Font("Segoe UI", 10)
            };

            // Buttons
            okButton = new Button
            {
                Text = "OK",
                Location = new Point(200, 200),
                Size = new Size(75, 30),
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            okButton.FlatAppearance.BorderSize = 0;
            okButton.Click += OkButton_Click;

            cancelButton = new Button
            {
                Text = "Cancel",
                Location = new Point(285, 200),
                Size = new Size(75, 30),
                DialogResult = DialogResult.Cancel,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            cancelButton.FlatAppearance.BorderSize = 0;

            this.Controls.AddRange(new Control[] {
                serverLabel, serverTextBox,
                databaseLabel, databaseTextBox,
                usernameLabel, usernameTextBox,
                passwordLabel, passwordTextBox,
                okButton, cancelButton
            });
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(serverTextBox.Text) ||
                string.IsNullOrWhiteSpace(databaseTextBox.Text) ||
                string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ConnectionString = $"Server={serverTextBox.Text};Database={databaseTextBox.Text};" +
                             $"Uid={usernameTextBox.Text};Pwd={passwordTextBox.Text};";
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 300);
            this.Name = "DatabaseConnectionForm";
            this.Text = "Database Connection";
            this.ResumeLayout(false);
        }
    }
}