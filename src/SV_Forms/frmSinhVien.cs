using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsAss.src.SV_Forms
{
    /// <summary>Form con: Nhập thông tin sinh viên (Mã SV, Họ tên, Ngày sinh, Ngành, Khoa, SĐT). Nút Thêm, Sửa, Xóa.</summary>
    public class frmSinhVien : Form
    {
        private Dictionary<string, Control> _inputs = null!;
        private ListView _lv = null!;

        public frmSinhVien()
        {
            InitializeComponent();
            LoadKhoaCombo();
            RefreshList();
            this.Shown += (s, e) => LoadKhoaCombo();
        }

        private void InitializeComponent()
        {
            this.Text = "Sinh viên";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            var fields = new[]
            {
                new FieldDef { Key = "MaSV", LabelText = "Mã SV:", Type = FieldControlType.Text },
                new FieldDef { Key = "HoTen", LabelText = "Họ tên:", Type = FieldControlType.Text },
                new FieldDef { Key = "NgaySinh", LabelText = "Ngày sinh:", Type = FieldControlType.Date },
                new FieldDef { Key = "NganhHoc", LabelText = "Ngành học:", Type = FieldControlType.Text },
                new FieldDef { Key = "Khoa", LabelText = "Khoa:", Type = FieldControlType.ComboBox },
                new FieldDef { Key = "SoDienThoai", LabelText = "Số ĐT:", Type = FieldControlType.Text }
            };
            _inputs = FormFieldHelper.CreateFields(this, fields, uniformInputWidth: FormFieldHelper.DefaultInputWidth);

            var buttons = new[]
            {
                new ButtonDef { Key = "Them", Text = "Thêm", Click = BtnThem_Click },
                new ButtonDef { Key = "Sua", Text = "Sửa", Click = BtnSua_Click },
                new ButtonDef { Key = "Xoa", Text = "Xóa", Click = BtnXoa_Click }
            };
            FormFieldHelper.CreateButtons(this, buttons, startX: FormFieldHelper.GetSuggestedButtonStartX(), startY: FormFieldHelper.DefaultStartY - 2);

            int listY = FormFieldHelper.ListViewY(fields.Length);
            _lv = FormFieldHelper.CreateListView(this, new ListViewDef
            {
                ColumnNames = new[] { "Mã SV", "Họ tên", "Ngày sinh", "Ngành", "Khoa", "SĐT" },
                ColumnWidths = new[] { 100, 100, 100, 100, 100, 100 },
                Width = 640,
                Height = 220
            }, listY);
            _lv.SelectedIndexChanged += Lv_SelectedIndexChanged;
        }

        private void LoadKhoaCombo()
        {
            var cbo = (ComboBox)_inputs["Khoa"];
            cbo.Items.Clear();
            foreach (var k in DataStore.Khoas) cbo.Items.Add(k);
        }

        private void RefreshList()
        {
            _lv.Items.Clear();
            foreach (var sv in DataStore.SinhViens)
            {
                var li = new ListViewItem(sv.MaSV);
                li.SubItems.Add(sv.HoTen);
                li.SubItems.Add(sv.NgaySinh.ToShortDateString());
                li.SubItems.Add(sv.NganhHoc);
                li.SubItems.Add(sv.MaKhoa);
                li.SubItems.Add(sv.SoDienThoai);
                li.Tag = sv;
                _lv.Items.Add(li);
            }
        }

        private void Lv_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_lv.SelectedItems.Count == 0) return;
            var sv = (SinhVien)_lv.SelectedItems[0].Tag!;
            FormFieldHelper.SetText(_inputs["MaSV"], sv.MaSV);
            FormFieldHelper.SetText(_inputs["HoTen"], sv.HoTen);
            ((DateTimePicker)_inputs["NgaySinh"]).Value = sv.NgaySinh;
            FormFieldHelper.SetText(_inputs["NganhHoc"], sv.NganhHoc);
            ((ComboBox)_inputs["Khoa"]).SelectedItem = DataStore.Khoas.FirstOrDefault(k => k.MaKhoa == sv.MaKhoa);
            FormFieldHelper.SetText(_inputs["SoDienThoai"], sv.SoDienThoai);
        }

        private string GetMaKhoa() => FormFieldHelper.GetInputText(_inputs, "Khoa", o => (o as Khoa)?.MaKhoa ?? "");

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FormFieldHelper.GetInputText(_inputs, "MaSV"))) { MessageBox.Show("Nhập mã SV."); return; }
            if (DataStore.SinhViens.Any(s => s.MaSV == FormFieldHelper.GetInputText(_inputs, "MaSV"))) { MessageBox.Show("Mã SV đã tồn tại."); return; }
            var sv = new SinhVien
            {
                MaSV = FormFieldHelper.GetInputText(_inputs, "MaSV"),
                HoTen = FormFieldHelper.GetInputText(_inputs, "HoTen"),
                NgaySinh = ((DateTimePicker)_inputs["NgaySinh"]).Value,
                NganhHoc = FormFieldHelper.GetInputText(_inputs, "NganhHoc"),
                MaKhoa = GetMaKhoa(),
                SoDienThoai = FormFieldHelper.GetInputText(_inputs, "SoDienThoai")
            };
            DataStore.SinhViens.Add(sv);
            RefreshList();
            FormFieldHelper.ClearInputs(_inputs);
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (_lv.SelectedItems.Count == 0) { MessageBox.Show("Chọn sinh viên cần sửa."); return; }
            var sv = (SinhVien)_lv.SelectedItems[0].Tag!;
            sv.HoTen = FormFieldHelper.GetInputText(_inputs, "HoTen");
            sv.NgaySinh = ((DateTimePicker)_inputs["NgaySinh"]).Value;
            sv.NganhHoc = FormFieldHelper.GetInputText(_inputs, "NganhHoc");
            sv.MaKhoa = GetMaKhoa();
            sv.SoDienThoai = FormFieldHelper.GetInputText(_inputs, "SoDienThoai");
            RefreshList();
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (_lv.SelectedItems.Count == 0) { MessageBox.Show("Chọn sinh viên cần xóa."); return; }
            if (MessageBox.Show("Xóa sinh viên đã chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            var sv = (SinhVien)_lv.SelectedItems[0].Tag!;
            DataStore.SinhViens.Remove(sv);
            DataStore.Diems.RemoveAll(d => d.MaSV == sv.MaSV);
            RefreshList();
            FormFieldHelper.ClearInputs(_inputs);
        }
    }
}
