using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsAss.src.Onclass.SV_Forms
{
    /// <summary>Form con: Chọn Sinh viên, chọn Môn học, nhập Điểm số.</summary>
    public class frmNhapDiem : Form
    {
        private Dictionary<string, Control> _inputs = null!;
        private ListView _lv = null!;

        public frmNhapDiem()
        {
            InitializeComponent();
            LoadCombos();
            RefreshList();
            this.Shown += (s, e) => LoadCombos();
        }

        private void InitializeComponent()
        {
            this.Text = "Nhập điểm";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            var fields = new[]
            {
                new FieldDef { Key = "SinhVien", LabelText = "Sinh viên:", Type = FieldControlType.ComboBox },
                new FieldDef { Key = "MonHoc", LabelText = "Môn học:", Type = FieldControlType.ComboBox },
                new FieldDef { Key = "DiemSo", LabelText = "Điểm số:", Type = FieldControlType.Number }
            };
            _inputs = FormFieldHelper.CreateFields(this, fields, uniformInputWidth: FormFieldHelper.DefaultInputWidth);

            var buttons = new[]
            {
                new ButtonDef { Key = "Them", Text = "Thêm", Click = BtnThem_Click },
                new ButtonDef { Key = "Xoa", Text = "Xóa", Click = BtnXoa_Click }
            };
            FormFieldHelper.CreateButtons(this, buttons, startX: FormFieldHelper.GetSuggestedButtonStartX(), startY: FormFieldHelper.DefaultStartY - 2);

            int listY = FormFieldHelper.ListViewY(fields.Length);
            _lv = FormFieldHelper.CreateListView(this, new ListViewDef
            {
                ColumnNames = new[] { "Mã SV", "Họ tên", "Môn", "Điểm" },
                ColumnWidths = new[] { 80, 180, 180, 80 },
                Width = 540,
                Height = 270
            }, listY);
        }

        private void LoadCombos()
        {
            var cboSV = (ComboBox)_inputs["SinhVien"];
            var cboMon = (ComboBox)_inputs["MonHoc"];
            cboSV.Items.Clear();
            foreach (var sv in DataStore.SinhViens) cboSV.Items.Add(sv);
            cboMon.Items.Clear();
            foreach (var m in DataStore.MonHocs) cboMon.Items.Add(m);
        }

        private void RefreshList()
        {
            _lv.Items.Clear();
            foreach (var d in DataStore.Diems)
            {
                var sv = DataStore.SinhViens.FirstOrDefault(s => s.MaSV == d.MaSV);
                var m = DataStore.MonHocs.FirstOrDefault(x => x.MaMon == d.MaMon);
                var li = new ListViewItem(d.MaSV) { Tag = d };
                li.SubItems.Add(sv?.HoTen ?? "");
                li.SubItems.Add(m?.TenMon ?? d.MaMon);
                li.SubItems.Add(d.DiemSo.ToString("F1"));
                _lv.Items.Add(li);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if ((_inputs["SinhVien"] as ComboBox)?.SelectedItem is not SinhVien sv) { MessageBox.Show("Chọn sinh viên."); return; }
            if ((_inputs["MonHoc"] as ComboBox)?.SelectedItem is not MonHoc m) { MessageBox.Show("Chọn môn học."); return; }
            if (!double.TryParse(FormFieldHelper.GetInputText(_inputs, "DiemSo"), out double diem) || diem < 0 || diem > 10) { MessageBox.Show("Điểm từ 0 đến 10."); return; }
            var existing = DataStore.Diems.FirstOrDefault(d => d.MaSV == sv.MaSV && d.MaMon == m.MaMon);
            if (existing != null) { existing.DiemSo = diem; MessageBox.Show("Đã cập nhật điểm."); }
            else DataStore.Diems.Add(new Diem { MaSV = sv.MaSV, MaMon = m.MaMon, DiemSo = diem });
            RefreshList();
            ((TextBox)_inputs["DiemSo"]).Clear();
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (_lv.SelectedItems.Count == 0) { MessageBox.Show("Chọn dòng điểm cần xóa."); return; }
            var d = (Diem)_lv.SelectedItems[0].Tag!;
            DataStore.Diems.Remove(d);
            RefreshList();
        }
    }
}
