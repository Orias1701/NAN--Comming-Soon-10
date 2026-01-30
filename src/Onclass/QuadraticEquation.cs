using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsAss
{
    public class QuadraticEquation : Form
    {
        private Label lblTitle = null!;
        private Label lblA = null!, lblB = null!, lblC = null!;
        private TextBox txtA = null!, txtB = null!, txtC = null!, txtKetQua = null!;
        private Button btnTinh = null!, btnTiepTuc = null!, btnThoat = null!;
        private Label lblResultTitle = null!;

        public QuadraticEquation()
        {
            InitializeComponent(); 
            SetupCustomUX();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(400, 450);
            this.Text = "Quadratic Equation";
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTitle = new Label();
            lblTitle.Text = "Giải Phương Trình bậc 2";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(120, 20);

            lblA = new Label() { Text = "a=", Location = new Point(50, 70), AutoSize = true };
            txtA = new TextBox() { Location = new Point(100, 67), Width = 200 };

            lblB = new Label() { Text = "b=", Location = new Point(50, 110), AutoSize = true };
            txtB = new TextBox() { Location = new Point(100, 107), Width = 200 };

            lblC = new Label() { Text = "c=", Location = new Point(50, 150), AutoSize = true };
            txtC = new TextBox() { Location = new Point(100, 147), Width = 200 };

            lblResultTitle = new Label() { Text = "Kết quả", Location = new Point(50, 190), ForeColor = Color.Blue };

            txtKetQua = new TextBox();
            txtKetQua.Location = new Point(50, 220);
            txtKetQua.Size = new Size(280, 80);
            txtKetQua.Multiline = true;
            txtKetQua.ReadOnly = true; 
            txtKetQua.BackColor = Color.White;

            btnTinh = new Button() { Text = "Tính nghiệm", Location = new Point(40, 320), Size = new Size(100, 30), Enabled = false };
            btnTiepTuc = new Button() { Text = "Tiếp tục", Location = new Point(150, 320), Size = new Size(90, 30), Enabled = false };
            btnThoat = new Button() { Text = "Thoát", Location = new Point(250, 320), Size = new Size(90, 30), Enabled = false };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblA); this.Controls.Add(txtA);
            this.Controls.Add(lblB); this.Controls.Add(txtB);
            this.Controls.Add(lblC); this.Controls.Add(txtC);
            this.Controls.Add(lblResultTitle);
            this.Controls.Add(txtKetQua);
            this.Controls.Add(btnTinh);
            this.Controls.Add(btnTiepTuc);
            this.Controls.Add(btnThoat);
        }

        private void SetupCustomUX()
        {
            this.ActiveControl = txtA;

            txtKetQua.TabStop = false;
            btnTinh.TabStop = false;
            btnTiepTuc.TabStop = false;
            btnThoat.TabStop = false;

            txtA.TextChanged += Input_TextChanged;
            txtB.TextChanged += Input_TextChanged;
            txtC.TextChanged += Input_TextChanged;

            btnTinh.Click += BtnTinh_Click;
            btnTiepTuc.Click += BtnTiepTuc_Click;
            btnThoat.Click += BtnThoat_Click;

            this.AcceptButton = btnTinh;
        }

        private void Input_TextChanged(object? sender, EventArgs e)
        {
            bool isAValid = double.TryParse(txtA.Text, out _);
            bool isBValid = double.TryParse(txtB.Text, out _);
            bool isCValid = double.TryParse(txtC.Text, out _);
            bool allValid = isAValid && isBValid && isCValid;

            btnTinh.Enabled = allValid;
            btnTiepTuc.Enabled = allValid;
            btnThoat.Enabled = allValid;
        }

        private void BtnTinh_Click(object? sender, EventArgs e)
        {
            double a = double.Parse(txtA.Text);
            double b = double.Parse(txtB.Text);
            double c = double.Parse(txtC.Text);
            string ketQua = "";

            if (a == 0)
            {
                if (b == 0) ketQua = (c == 0) ? "Vô số nghiệm" : "Vô nghiệm";
                else ketQua = $"Nghiệm đơn x = {-c / b}";
            }
            else
            {
                double delta = b * b - 4 * a * c;
                if (delta < 0) ketQua = "Vô nghiệm";
                else if (delta == 0) ketQua = $"Nghiệm kép x = {-b / (2 * a)}";
                else
                {
                    double x1 = (-b + Math.Sqrt(delta)) / (2 * a);
                    double x2 = (-b - Math.Sqrt(delta)) / (2 * a);
                    ketQua = $"x1 = {x1}\r\nx2 = {x2}";
                }
            }
            txtKetQua.Text = ketQua;
        }

        private void BtnTiepTuc_Click(object? sender, EventArgs e)
        {
            txtA.Clear(); txtB.Clear(); txtC.Clear(); txtKetQua.Clear();
            txtA.Focus();
        }

        private void BtnThoat_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        public static void Run()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new QuadraticEquation());
        }
    }
}