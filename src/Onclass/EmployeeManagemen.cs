using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsAss.src.Onclass
{
    // === PHẦN 1: RUNNER (Cầu nối với Main) ===
    public class EmployeeRunner
    {
        public static void Run()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EmployeeForm());
        }
    }

    // === PHẦN 2: FORM XỬ LÝ ===
    public class EmployeeForm : Form
    {
        // Khai báo các control
        private Label lblTitle = null!, lblHoTen = null!, lblNgaySinh = null!, lblDiaChi = null!, lblDienThoai = null!;
        private TextBox txtHoTen = null!, txtDiaChi = null!, txtDienThoai = null!;
        private DateTimePicker dtpNgaySinh = null!;
        private Button btnThem = null!, btnXoa = null!, btnSua = null!, btnThoat = null!;
        private ListView lsvNhanVien = null!;
        private GroupBox grpChiTiet = null!, grpDanhSach = null!;

        public EmployeeForm()
        {
            InitializeComponent();
            LoadSampleData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản Lý Nhân Viên";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 1. Tiêu đề
            lblTitle = new Label
            {
                Text = "DANH MỤC NHÂN VIÊN",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Blue,
                AutoSize = true,
                Location = new Point(220, 10)
            };

            // 2. Group Thông tin chi tiết
            grpChiTiet = new GroupBox { Text = "Thông tin chi tiết", Location = new Point(20, 50), Size = new Size(650, 150) };

            // --- CỘT TRÁI ---
            lblHoTen = new Label { Text = "Họ tên:", Location = new Point(20, 30), AutoSize = true };
            txtHoTen = new TextBox { Location = new Point(120, 27), Width = 180 };

            lblNgaySinh = new Label { Text = "Ngày sinh:", Location = new Point(20, 70), AutoSize = true };
            dtpNgaySinh = new DateTimePicker { Location = new Point(120, 67), Width = 180, Format = DateTimePickerFormat.Short };

            // --- CỘT PHẢI ---
            lblDienThoai = new Label { Text = "Điện thoại:", Location = new Point(330, 30), AutoSize = true };
            txtDienThoai = new TextBox { Location = new Point(430, 27), Width = 180 };

            lblDiaChi = new Label { Text = "Địa chỉ:", Location = new Point(330, 70), AutoSize = true };
            txtDiaChi = new TextBox { Location = new Point(430, 67), Width = 180 };

            // --- NÚT BẤM ---
            btnThem = new Button { Text = "Thêm", Location = new Point(310, 110), Width = 70 };
            btnXoa = new Button { Text = "Xóa", Location = new Point(390, 110), Width = 70 };
            btnSua = new Button { Text = "Sửa", Location = new Point(470, 110), Width = 70 };
            btnThoat = new Button { Text = "Thoát", Location = new Point(550, 110), Width = 70 };

            grpChiTiet.Controls.AddRange(new Control[] { 
                lblHoTen, txtHoTen, 
                lblNgaySinh, dtpNgaySinh, 
                lblDienThoai, txtDienThoai, 
                lblDiaChi, txtDiaChi, 
                btnThem, btnXoa, btnSua, btnThoat 
            });

            // 3. Group Thông tin chung (ListView)
            grpDanhSach = new GroupBox { Text = "Thông tin chung", Location = new Point(20, 210), Size = new Size(640, 230) };
            
            lsvNhanVien = new ListView
            {
                Location = new Point(10, 20),
                Size = new Size(620, 200),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            lsvNhanVien.Columns.Add("Họ Tên", 150);
            lsvNhanVien.Columns.Add("Ngày Sinh", 100);
            lsvNhanVien.Columns.Add("Địa Chỉ", 200);
            lsvNhanVien.Columns.Add("Điện Thoại", 100);

            grpDanhSach.Controls.Add(lsvNhanVien);

            // 4. Thêm sự kiện (Event)
            btnThem.Click += BtnThem_Click;
            btnXoa.Click += BtnXoa_Click;
            btnSua.Click += BtnSua_Click;
            btnThoat.Click += (s, e) => this.Close();
            
            lsvNhanVien.SelectedIndexChanged += LsvNhanVien_SelectedIndexChanged;

            this.Controls.AddRange(new Control[] { lblTitle, grpChiTiet, grpDanhSach });
        }

        // == LOGIC ===

        private void LoadSampleData()
        {
            CreateEmployee("Nguyễn Văn A", "01/01/1990", "Hà Nội", "0901234567");
            CreateEmployee("Trần Thị B", "15/05/1995", "Hồ Chí Minh", "0912345678");
            CreateEmployee("Lê Văn C", "20/10/2000", "Đà Nẵng", "0987654321");
        }

        private void CreateEmployee(string ten, string ngaysinh, string diachi, string sdt)
        {
            ListViewItem item = new ListViewItem(ten);
            item.SubItems.Add(ngaysinh);
            item.SubItems.Add(diachi);
            item.SubItems.Add(sdt);
            lsvNhanVien.Items.Add(item);
        }

        // 1. Nút THÊM
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống!", "Lỗi nhập liệu");
                txtHoTen.Focus();
                return;
            }

            CreateEmployee(
                txtHoTen.Text,
                dtpNgaySinh.Value.ToString("dd/MM/yyyy"),
                txtDiaChi.Text,
                txtDienThoai.Text
            );
            
            ResetInputs();
        }

        // 2. Nút XÓA
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (lsvNhanVien.SelectedItems.Count > 0)
            {
                if(MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lsvNhanVien.Items.Remove(lsvNhanVien.SelectedItems[0]);
                    ResetInputs();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa.");
            }
        }

        // 3. Nút SỬA
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (lsvNhanVien.SelectedItems.Count > 0)
            {
                ListViewItem item = lsvNhanVien.SelectedItems[0];
                item.Text = txtHoTen.Text; // Cập nhật cột 0
                item.SubItems[1].Text = dtpNgaySinh.Value.ToString("dd/MM/yyyy");
                item.SubItems[2].Text = txtDiaChi.Text;
                item.SubItems[3].Text = txtDienThoai.Text;
                
                MessageBox.Show("Cập nhật thành công!");
                ResetInputs();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên để sửa.");
            }
        }

        // 4. Sự kiện CHỌN DÒNG -> Hiển thị lên ô nhập
        private void LsvNhanVien_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lsvNhanVien.SelectedItems.Count > 0)
            {
                ListViewItem item = lsvNhanVien.SelectedItems[0];
                txtHoTen.Text = item.Text;
                // Xử lý ngày sinh (chống lỗi format)
                try { dtpNgaySinh.Value = DateTime.ParseExact(item.SubItems[1].Text, "dd/MM/yyyy", null); } catch { }
                txtDiaChi.Text = item.SubItems[2].Text;
                txtDienThoai.Text = item.SubItems[3].Text;
            }
        }

        private void ResetInputs()
        {
            txtHoTen.Clear();
            txtDiaChi.Clear();
            txtDienThoai.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            txtHoTen.Focus();
        }
    }
}