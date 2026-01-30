using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsAss.src.Onclass.SV_Forms
{
    /// <summary>Form con: Nhập Mã khoa, Tên khoa.</summary>
    public class frmKhoa : Form
    {
        private Dictionary<string, Control> _inputs = null!;
        private ListView _lv = null!;

        public frmKhoa()
        {
            InitializeComponent();
            RefreshList();
        }

        private void InitializeComponent()
        {
            this.Text = "Khoa";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            var fields = new[]
            {
                new FieldDef { Key = "MaKhoa", LabelText = "Mã khoa:", Type = FieldControlType.Text },
                new FieldDef { Key = "TenKhoa", LabelText = "Tên khoa:", Type = FieldControlType.Text }
            };
            _inputs = FormFieldHelper.CreateFields(this, fields, uniformInputWidth: FormFieldHelper.DefaultInputWidth);

            var buttons = new[]
            {
                new ButtonDef { Key = "Them", Text = "Thêm", Click = BtnThem_Click },
                new ButtonDef { Key = "Sua", Text = "Sửa", Click = BtnSua_Click },
                new ButtonDef { Key = "Xoa", Text = "Xóa", Click = BtnXoa_Click }
            };
            FormFieldHelper.CreateButtons(this, buttons, startX: FormFieldHelper.GetSuggestedButtonStartX(), startY: FormFieldHelper.DefaultStartY - 2);

            // Dùng 3 dòng (bằng số nút) để ListView nằm dưới hết nút, tránh nút thứ 3 đè lên bảng.
            int listY = FormFieldHelper.ListViewY(3);
            _lv = FormFieldHelper.CreateListView(this, new ListViewDef
            {
                ColumnNames = new[] { "Mã khoa", "Tên khoa" },
                ColumnWidths = new[] { 120, 280 },
                Width = 430,
                Height = 250
            }, listY);
            _lv.SelectedIndexChanged += (s, e) =>
            {
                if (_lv.SelectedItems.Count == 0) return;
                var k = (Khoa)_lv.SelectedItems[0].Tag!;
                FormFieldHelper.SetText(_inputs["MaKhoa"], k.MaKhoa);
                FormFieldHelper.SetText(_inputs["TenKhoa"], k.TenKhoa);
            };
        }

        private void RefreshList()
        {
            _lv.Items.Clear();
            foreach (var k in DataStore.Khoas)
            {
                var li = new ListViewItem(k.MaKhoa) { Tag = k };
                li.SubItems.Add(k.TenKhoa);
                _lv.Items.Add(li);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FormFieldHelper.GetInputText(_inputs, "MaKhoa"))) { MessageBox.Show("Nhập mã khoa."); return; }
            if (DataStore.Khoas.Exists(k => k.MaKhoa == FormFieldHelper.GetInputText(_inputs, "MaKhoa"))) { MessageBox.Show("Mã khoa đã tồn tại."); return; }
            DataStore.Khoas.Add(new Khoa { MaKhoa = FormFieldHelper.GetInputText(_inputs, "MaKhoa"), TenKhoa = FormFieldHelper.GetInputText(_inputs, "TenKhoa") });
            RefreshList();
            FormFieldHelper.ClearInputs(_inputs);
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (_lv.SelectedItems.Count == 0) { MessageBox.Show("Chọn khoa cần sửa."); return; }
            var k = (Khoa)_lv.SelectedItems[0].Tag!;
            k.TenKhoa = FormFieldHelper.GetInputText(_inputs, "TenKhoa");
            RefreshList();
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (_lv.SelectedItems.Count == 0) { MessageBox.Show("Chọn khoa cần xóa."); return; }
            if (MessageBox.Show("Xóa khoa đã chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            var k = (Khoa)_lv.SelectedItems[0].Tag!;
            DataStore.Khoas.Remove(k);
            RefreshList();
            FormFieldHelper.ClearInputs(_inputs);
        }
    }
}
