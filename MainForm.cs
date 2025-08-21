using MySql.Data.MySqlClient;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;

namespace Loan_Amortization
{
    public partial class MainForm : Form
    {
        private List<AmortizationEntry> currentSchedule = new();

        public MainForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Set form properties
            this.Text = "Loan Amortization Calculator";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 600);
            this.BackColor = Color.FromArgb(240, 244, 248);

            // Create main layout
            CreateControls();
            SetupLayout();
        }

        private void CreateControls()
        {
            // Input Panel
            var inputPanel = new Panel
            {
                Name = "inputPanel",
                BackColor = Color.White,
                Size = new Size(350, 300),
                Location = new Point(20, 20)
            };
            inputPanel.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, inputPanel.ClientRectangle,
                    Color.FromArgb(200, 200, 200), ButtonBorderStyle.Solid);
            };

            // Title Label
            var titleLabel = new Label
            {
                Text = "Loan Parameters",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 20),
                Size = new Size(200, 30)
            };

            // Principal Amount
            var principalLabel = new Label
            {
                Text = "Loan Amount ($):",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 70),
                Size = new Size(120, 25)
            };
            var principalTextBox = new TextBox
            {
                Name = "principalTextBox",
                Font = new Font("Segoe UI", 10),
                Location = new Point(150, 68),
                Size = new Size(150, 25),
                Text = "100000"
            };

            // Annual Interest Rate
            var rateLabel = new Label
            {
                Text = "Annual Rate (%):",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 110),
                Size = new Size(120, 25)
            };
            var rateTextBox = new TextBox
            {
                Name = "rateTextBox",
                Font = new Font("Segoe UI", 10),
                Location = new Point(150, 108),
                Size = new Size(150, 25),
                Text = "12"
            };

            // Loan Term
            var termLabel = new Label
            {
                Text = "Term (months):",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 150),
                Size = new Size(120, 25)
            };
            var termTextBox = new TextBox
            {
                Name = "termTextBox",
                Font = new Font("Segoe UI", 10),
                Location = new Point(150, 148),
                Size = new Size(150, 25),
                Text = "36"
            };

            // Calculate Button
            var calculateButton = new Button
            {
                Name = "calculateButton",
                Text = "Calculate",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 200),
                Size = new Size(280, 40),
                Cursor = Cursors.Hand
            };
            calculateButton.FlatAppearance.BorderSize = 0;
            calculateButton.Click += CalculateButton_Click;

            // Export Buttons Panel
            var exportPanel = new Panel
            {
                Location = new Point(20, 250),
                Size = new Size(280, 40)
            };

            var exportCsvButton = new Button
            {
                Name = "exportCsvButton",
                Text = "Export CSV",
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(0, 0),
                Size = new Size(135, 35),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            exportCsvButton.FlatAppearance.BorderSize = 0;
            exportCsvButton.Click += ExportCsvButton_Click;

            var saveDatabaseButton = new Button
            {
                Name = "saveDatabaseButton",
                Text = "Save to DB",
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(145, 0),
                Size = new Size(135, 35),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            saveDatabaseButton.FlatAppearance.BorderSize = 0;
            saveDatabaseButton.Click += SaveDatabaseButton_Click;

            exportPanel.Controls.AddRange(new Control[] { exportCsvButton, saveDatabaseButton });

            // Add controls to input panel
            inputPanel.Controls.AddRange(new Control[] {
                titleLabel, principalLabel, principalTextBox,
                rateLabel, rateTextBox, termLabel, termTextBox,
                calculateButton, exportPanel
            });

            // Results Panel
            var resultsPanel = new Panel
            {
                Name = "resultsPanel",
                BackColor = Color.White,
                Location = new Point(390, 20),
                Size = new Size(780, 300)
            };
            resultsPanel.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, resultsPanel.ClientRectangle,
                    Color.FromArgb(200, 200, 200), ButtonBorderStyle.Solid);
            };

            var resultsTitle = new Label
            {
                Text = "Loan Summary",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 20),
                Size = new Size(200, 30)
            };

            var summaryLabel = new Label
            {
                Name = "summaryLabel",
                Text = "Enter loan parameters and click Calculate to see results.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(20, 60),
                Size = new Size(740, 220),
                AutoSize = false
            };

            resultsPanel.Controls.AddRange(new Control[] { resultsTitle, summaryLabel });

            // DataGridView for schedule
            var scheduleDataGrid = new DataGridView
            {
                Name = "scheduleDataGrid",
                Location = new Point(20, 340),
                Size = new Size(1150, 400),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 58, 64),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 9),
                    BackColor = Color.White,
                    SelectionBackColor = Color.FromArgb(0, 123, 255, 50)
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(248, 249, 250)
                }
            };

            // Add all controls to form
            this.Controls.AddRange(new Control[] { inputPanel, resultsPanel, scheduleDataGrid });
        }

        private void SetupLayout()
        {
            // Setup DataGridView columns
            var dataGrid = this.Controls.Find("scheduleDataGrid", true)[0] as DataGridView;
            dataGrid.Columns.Add("Month", "Month");
            dataGrid.Columns.Add("Payment", "Payment");
            dataGrid.Columns.Add("Interest", "Interest");
            dataGrid.Columns.Add("Principal", "Principal");
            dataGrid.Columns.Add("Balance", "Balance");

            // Format currency columns
            for (int i = 1; i < dataGrid.Columns.Count; i++)
            {
                dataGrid.Columns[i].DefaultCellStyle.Format = "C2";
                dataGrid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dataGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input values
                var principalText = (this.Controls.Find("principalTextBox", true)[0] as TextBox).Text;
                var rateText = (this.Controls.Find("rateTextBox", true)[0] as TextBox).Text;
                var termText = (this.Controls.Find("termTextBox", true)[0] as TextBox).Text;

                if (!decimal.TryParse(principalText, out decimal principal) || principal <= 0)
                {
                    MessageBox.Show("Please enter a valid loan amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(rateText, out decimal annualRate) || annualRate <= 0)
                {
                    MessageBox.Show("Please enter a valid annual interest rate.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(termText, out int months) || months <= 0)
                {
                    MessageBox.Show("Please enter a valid loan term in months.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Convert rate to decimal
                annualRate = annualRate / 100;

                // Calculate schedule
                currentSchedule = CalculateAmortizationSchedule(principal, annualRate, months);

                // Update UI
                UpdateSummary(principal, annualRate, months);
                UpdateDataGrid();
                EnableExportButtons(true);

                MessageBox.Show("Calculation completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during calculation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary(decimal principal, decimal annualRate, int months)
        {
            var summaryLabel = this.Controls.Find("summaryLabel", true)[0] as Label;
            var totalPayments = currentSchedule.Sum(e => e.Payment);
            var totalInterest = currentSchedule.Sum(e => e.Interest);
            var monthlyPayment = currentSchedule.First().Payment;

            summaryLabel.Text = $"Principal Amount: {principal:C}\n" +
                               $"Annual Interest Rate: {annualRate:P2}\n" +
                               $"Loan Term: {months} months\n\n" +
                               $"Monthly Payment: {monthlyPayment:C}\n" +
                               $"Total Payments: {totalPayments:C}\n" +
                               $"Total Interest: {totalInterest:C}\n" +
                               $"Total Cost: {totalPayments:C}";
        }

        private void UpdateDataGrid()
        {
            var dataGrid = this.Controls.Find("scheduleDataGrid", true)[0] as DataGridView;
            dataGrid.Rows.Clear();

            foreach (var entry in currentSchedule)
            {
                dataGrid.Rows.Add(entry.Month, entry.Payment, entry.Interest, entry.Principal, entry.Balance);
            }
        }

        private void EnableExportButtons(bool enabled)
        {
            var exportCsvButton = this.Controls.Find("exportCsvButton", true)[0] as Button;
            var saveDatabaseButton = this.Controls.Find("saveDatabaseButton", true)[0] as Button;
            exportCsvButton.Enabled = enabled;
            saveDatabaseButton.Enabled = enabled;
        }

        private void ExportCsvButton_Click(object sender, EventArgs e)
        {
            if (currentSchedule.Count == 0)
            {
                MessageBox.Show("No data to export. Please calculate first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveDialog.DefaultExt = "csv";
                saveDialog.FileName = "amortization_schedule.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportScheduleToCsv(currentSchedule, saveDialog.FileName);
                        MessageBox.Show($"Schedule exported successfully to {saveDialog.FileName}", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting to CSV: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveDatabaseButton_Click(object sender, EventArgs e)
        {
            if (currentSchedule.Count == 0)
            {
                MessageBox.Show("No data to save. Please calculate first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var connectionForm = new DatabaseConnectionForm();
            if (connectionForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SaveScheduleToMySql(currentSchedule, connectionForm.ConnectionString);
                    MessageBox.Show("Schedule saved to database successfully!", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving to database: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Existing calculation methods from console app
        public static List<AmortizationEntry> CalculateAmortizationSchedule(decimal principal, decimal annualRate, int months)
        {
            var schedule = new List<AmortizationEntry>();
            decimal monthlyRate = annualRate / 12;
            decimal payment = principal * monthlyRate / (1 - (decimal)Math.Pow((double)(1 + monthlyRate), -months));
            decimal balance = principal;

            for (int month = 1; month <= months; month++)
            {
                decimal interest = Math.Round(balance * monthlyRate, 2);
                decimal principalPaid = Math.Round(payment - interest, 2);
                if (month == months)
                {
                    // Final payment adjustment
                    principalPaid = balance;
                    payment = interest + principalPaid;
                }
                balance = Math.Round(balance - principalPaid, 2);
                schedule.Add(new AmortizationEntry
                {
                    Month = month,
                    Payment = payment,
                    Interest = interest,
                    Principal = principalPaid,
                    Balance = balance
                });
            }
            return schedule;
        }

        public static void SaveScheduleToMySql(List<AmortizationEntry> schedule, string connStr)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS amortization (
                id INT AUTO_INCREMENT PRIMARY KEY,
                month INT,
                payment DECIMAL(15,2),
                interest DECIMAL(15,2),
                principal DECIMAL(15,2),
                balance DECIMAL(15,2)
            )";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM amortization";
            cmd.ExecuteNonQuery();
            foreach (var entry in schedule)
            {
                cmd.CommandText = "INSERT INTO amortization (month, payment, interest, principal, balance) VALUES (@month, @payment, @interest, @principal, @balance)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@month", MySqlDbType.Int32).Value = entry.Month;
                cmd.Parameters.Add("@payment", MySqlDbType.Decimal).Value = entry.Payment;
                cmd.Parameters.Add("@interest", MySqlDbType.Decimal).Value = entry.Interest;
                cmd.Parameters.Add("@principal", MySqlDbType.Decimal).Value = entry.Principal;
                cmd.Parameters.Add("@balance", MySqlDbType.Decimal).Value = entry.Balance;
                cmd.ExecuteNonQuery();
            }
        }

        public static void ExportScheduleToCsv(List<AmortizationEntry> schedule, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            writer.WriteLine("Month,Payment,Interest,Principal,Balance");
            foreach (var entry in schedule)
            {
                writer.WriteLine($"{entry.Month},{entry.Payment},{entry.Interest},{entry.Principal},{entry.Balance}");
            }
        }
    }
}