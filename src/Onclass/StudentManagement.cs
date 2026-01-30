using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsAss.src.Onclass
{
    public class StudentRunner
    {
        public static void Run()
        {
            Application.Run(new StudentManagementForm());
        }
    }

    public class StudentManagementForm : Form
    {
        private TextBox txtHoTen = null!;
        private Button btnCapNhat = null!, btnSangPhai = null!, btnSangTrai = null!, btnTatCaSangPhai = null!, btnTatCaSangTrai = null!, btnXoa = null!, btnKetThuc = null!;
        private ListBox lstLopA = null!, lstLopB = null!;
        private Label lblTieuDe = null!, lblHoTen = null!;
        private GroupBox grpLopA = null!, grpLopB = null!;

        public StudentManagementForm()
        {
            InitializeComponent();
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            lstLopA.Items.Add("Nguyễn Văn An");
            lstLopA.Items.Add("Trần Thị Bích");
            lstLopA.Items.Add("Lê Hoàng Nam");

            lstLopB.Items.Add("Phạm Minh Tuấn");
            lstLopB.Items.Add("Hoàng Thùy Linh");
        }

        private void InitializeComponent()
        {
            this.Size = new Size(600, 450);
            this.Text = "Quản lý sinh viên";
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTieuDe = new Label { Text = "DANH SÁCH SINH VIÊN", Font = new Font("Arial", 16, FontStyle.Bold), AutoSize = true, Location = new Point(180, 20), ForeColor = Color.Blue };
            
            lblHoTen = new Label { Text = "Họ và tên:", Location = new Point(50, 70), AutoSize = true };
            txtHoTen = new TextBox { Location = new Point(120, 65), Width = 300 };
            btnCapNhat = new Button { Text = "Cập Nhật", Location = new Point(440, 63), Width = 80 };
            btnCapNhat.Click += BtnCapNhat_Click;

            grpLopA = new GroupBox { Text = "Lớp A", Location = new Point(30, 110), Size = new Size(200, 250) };
            lstLopA = new ListBox { Location = new Point(10, 20), Size = new Size(180, 220), SelectionMode = SelectionMode.MultiExtended };
            grpLopA.Controls.Add(lstLopA);

            grpLopB = new GroupBox { Text = "Lớp B", Location = new Point(350, 110), Size = new Size(200, 250) };
            lstLopB = new ListBox { Location = new Point(10, 20), Size = new Size(180, 220), SelectionMode = SelectionMode.MultiExtended };
            grpLopB.Controls.Add(lstLopB);

            btnSangPhai = new Button { Text = ">", Location = new Point(245, 160), Size = new Size(40, 30) };
            btnSangTrai = new Button { Text = "<", Location = new Point(295, 160), Size = new Size(40, 30) };
            btnTatCaSangPhai = new Button { Text = ">>", Location = new Point(245, 200), Size = new Size(40, 30) };
            btnTatCaSangTrai = new Button { Text = "<<", Location = new Point(295, 200), Size = new Size(40, 30) };

            btnSangPhai.Click += (s, e) => MoveSelectedItems(lstLopA, lstLopB);
            btnSangTrai.Click += (s, e) => MoveSelectedItems(lstLopB, lstLopA);
            btnTatCaSangPhai.Click += (s, e) => MoveAllItems(lstLopA, lstLopB);
            btnTatCaSangTrai.Click += (s, e) => MoveAllItems(lstLopB, lstLopA);

            btnXoa = new Button { Text = "Xóa", Location = new Point(100, 380), Width = 80 };
            btnXoa.Click += BtnXoa_Click;
            
            btnKetThuc = new Button { Text = "Kết thúc", Location = new Point(400, 380), Width = 80 };
            btnKetThuc.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblTieuDe, lblHoTen, txtHoTen, btnCapNhat, grpLopA, grpLopB, btnSangPhai, btnSangTrai, btnTatCaSangPhai, btnTatCaSangTrai, btnXoa, btnKetThuc });
        }

        private void BtnCapNhat_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                lstLopA.Items.Add(txtHoTen.Text);
                txtHoTen.Clear();
                txtHoTen.Focus();
            }
            else MessageBox.Show("Vui lòng nhập tên!");
        }

        private void MoveSelectedItems(ListBox source, ListBox dest)
        {
            List<object> temp = new List<object>();
            foreach (var item in source.SelectedItems) temp.Add(item);
            foreach (var item in temp)
            {
                dest.Items.Add(item);
                source.Items.Remove(item);
            }
        }

        private void MoveAllItems(ListBox source, ListBox dest)
        {
            dest.Items.AddRange(source.Items);
            source.Items.Clear();
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            for (int i = lstLopA.SelectedIndices.Count - 1; i >= 0; i--) lstLopA.Items.RemoveAt(lstLopA.SelectedIndices[i]);
            for (int i = lstLopB.SelectedIndices.Count - 1; i >= 0; i--) lstLopB.Items.RemoveAt(lstLopB.SelectedIndices[i]);
        }
    }
}