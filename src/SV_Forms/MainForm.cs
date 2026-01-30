using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsAss.src.SV_Forms
{
    /// <summary>Điểm vào: chạy ứng dụng Quản lý sinh viên MDI từ menu chính.</summary>
    public static class SV_FormsRunner
    {
        public static void Run()
        {
            Application.Run(new MainForm());
        }
    }

    /// <summary>Form chính MDI: ToolStrip với các nút mở Form con (Sinh viên, Khoa, Môn học, Nhập điểm, Xem điểm, Thống kê), Thoát.</summary>
    public class MainForm : Form
    {
        private ToolStrip _toolStrip = null!;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý sinh viên - MDI";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;
            this.BackColor = Color.FromArgb(240, 240, 240);

            _toolStrip = new ToolStrip
            {
                GripStyle = ToolStripGripStyle.Hidden,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                ImageScalingSize = new Size(24, 24),
                Padding = new Padding(4, 2, 4, 2)
            };

            AddButton("Sinh viên", "Nhập thông tin SV", ShowFrmSinhVien);
            AddButton("Khoa", "Nhập khoa", ShowFrmKhoa);
            AddButton("Môn học", "Nhập môn học", ShowFrmMonHoc);
            AddButton("Nhập điểm", "Nhập điểm cho SV", ShowFrmNhapDiem);
            AddButton("Xem điểm", "Tra cứu điểm", ShowFrmXemDiem);
            AddButton("Thống kê Khoa", "Tra cứu SV theo khoa", ShowFrmThongKe);
            _toolStrip.Items.Add(new ToolStripSeparator());
            AddButton("Thoát", "Đóng ứng dụng", (s, e) => Application.Exit());

            this.Controls.Add(_toolStrip);
        }

        private void AddButton(string text, string tooltip, EventHandler click)
        {
            var btn = new ToolStripButton(text)
            {
                ToolTipText = tooltip,
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            btn.Click += click;
            _toolStrip.Items.Add(btn);
        }

        private void ShowChild(Form child)
        {
            child.MdiParent = this;
            child.WindowState = FormWindowState.Maximized;
            child.Show();
        }

        private void ShowFrmSinhVien(object? sender, EventArgs e) => ShowChild(new frmSinhVien());
        private void ShowFrmKhoa(object? sender, EventArgs e) => ShowChild(new frmKhoa());
        private void ShowFrmMonHoc(object? sender, EventArgs e) => ShowChild(new frmMonHoc());
        private void ShowFrmNhapDiem(object? sender, EventArgs e) => ShowChild(new frmNhapDiem());
        private void ShowFrmXemDiem(object? sender, EventArgs e) => ShowChild(new frmXemDiem());
        private void ShowFrmThongKe(object? sender, EventArgs e) => ShowChild(new frmThongKe());
    }
}
