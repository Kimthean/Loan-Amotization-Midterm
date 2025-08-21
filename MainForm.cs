
namespace Loan_Amortization
{
    // MainForm is the main window for the Loan Amortization Calculator application
    public partial class MainForm : Form
    {
        private List<AmortizationEntry> currentSchedule = new();

        public MainForm()
        {
            InitializeComponent(); // Initialize UI components
            SetupForm(); // Set up the form layout and controls
        }

        // Sets up the main form's properties and layout
        private void SetupForm()
        {
            // Set form properties (title, size, position, background color)
            this.Text = "Loan Amortization Calculator";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 600);
            this.BackColor = Color.FromArgb(240, 244, 248);

            // Create and arrange controls on the form
            CreateControls();
            SetupLayout();
        }

    // Creates all UI controls (input fields, buttons, panels, etc.)
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

            exportPanel.Controls.Add(exportCsvButton);

            // Add controls to input panel
            inputPanel.Controls.AddRange([
                titleLabel, principalLabel, principalTextBox,
                rateLabel, rateTextBox, termLabel, termTextBox,
                calculateButton, exportPanel
            ]);

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

            resultsPanel.Controls.AddRange([resultsTitle, summaryLabel]);

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
                    SelectionBackColor = Color.FromArgb(0, 123, 255),
                    SelectionForeColor = Color.White
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(248, 249, 250),
                    SelectionBackColor = Color.FromArgb(0, 123, 255),
                    SelectionForeColor = Color.White
                }
            };

            // Add all controls to form
        Controls.AddRange([inputPanel, resultsPanel, scheduleDataGrid]);
        }

    // Sets up the DataGridView columns and formatting
    private void SetupLayout()
        {
            // Setup DataGridView columns
            var dataGridObj = this.Controls.Find("scheduleDataGrid", true).FirstOrDefault();
            if (dataGridObj is DataGridView dataGrid)
            {
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
        }

    // Handles the Calculate button click event
    // Validates input, calculates the amortization schedule, and updates the UI
    private void CalculateButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input values
                var principalObj = this.Controls.Find("principalTextBox", true).FirstOrDefault();
                var rateObj = this.Controls.Find("rateTextBox", true).FirstOrDefault();
                var termObj = this.Controls.Find("termTextBox", true).FirstOrDefault();
                if (principalObj is not TextBox principalTextBox || rateObj is not TextBox rateTextBox || termObj is not TextBox termTextBox)
                {
                    MessageBox.Show("Input controls not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var principalText = principalTextBox.Text;
                var rateText = rateTextBox.Text;
                var termText = termTextBox.Text;

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
                annualRate /= 100;

                // Calculate schedule
                currentSchedule = CalculateAmortizationSchedule(principal, annualRate, months);

                // Update UI
                UpdateSummary(principal, annualRate, months);
                UpdateDataGrid();
                EnableExportButtons(true);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during calculation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    // Updates the summary label with loan details and results
    private void UpdateSummary(decimal principal, decimal annualRate, int months)
        {
            var summaryObj = Controls.Find("summaryLabel", true).FirstOrDefault();
            if (summaryObj is not Label summaryLabel)
                return;
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

    // Populates the DataGridView with the amortization schedule
    private void UpdateDataGrid()
        {
            var dataGridObj = Controls.Find("scheduleDataGrid", true).FirstOrDefault();
            if (dataGridObj is DataGridView dataGrid)
            {
                dataGrid.Rows.Clear();
                foreach (var entry in currentSchedule)
                {
                    dataGrid.Rows.Add(entry.Month, entry.Payment, entry.Interest, entry.Principal, entry.Balance);
                }
            }
        }

    // Enables or disables the export buttons based on calculation state
    private void EnableExportButtons(bool enabled)
        {
            var exportCsvObj = this.Controls.Find("exportCsvButton", true).FirstOrDefault();
            if (exportCsvObj is Button exportCsvButton)
                exportCsvButton.Enabled = enabled;
        }

    // Handles the Export CSV button click event
    // Exports the amortization schedule to a CSV file
    private void ExportCsvButton_Click(object? sender, EventArgs? e)
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
        
    // Calculates the amortization schedule for the given loan parameters
        // Returns a list of AmortizationEntry objects
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

    // Exports the amortization schedule to a CSV file
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