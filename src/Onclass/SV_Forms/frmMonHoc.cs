using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsAss.src.Onclass.SV_Forms
{
    /// <summary>Form con: Nhập Mã môn, Tên môn, Số tín chỉ.</summary>
    public class frmMonHoc : Form
    {
        private Dictionary<string, Control> _inputs = null!;
        private ListView _lv = null!;

        public frmMonHoc()
        {
            InitializeComponent();
            RefreshList();
        }

        private void InitializeComponent()
        {
            this.Text = "Môn học";
            this.Size = new Size(550, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            var fields = new[]
            {
                new FieldDef { Key = "MaMon", LabelText = "Mã môn:", Type = FieldControlType.Text },
                new FieldDef { Key = "TenMon", LabelText = "Tên môn:", Type = FieldControlType.Text },
                new FieldDef { Key = "SoTinChi", LabelText = "Số tín chỉ:", Type = FieldControlType.Number }
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
                ColumnNames = new[] { "Mã môn", "Tên môn", "Số TC" },
                ColumnWidths = new[] { 100, 280, 80 },
                Width = 500,
                Height = 250
            }, listY);
            _lv.SelectedIndexChanged += (s, e) =>
            {
                if (_lv.SelectedItems.Count == 0) return;
                var m = (MonHoc)_lv.SelectedItems[0].Tag!;
                FormFieldHelper.SetText(_inputs["MaMon"], m.MaMon);
                FormFieldHelper.SetText(_inputs["TenMon"], m.TenMon);
                ((TextBox)_inputs["SoTinChi"]).Text = m.SoTinChi.ToString();
            };
        }

        private void RefreshList()
        {
            _lv.Items.Clear();
            foreach (var m in DataStore.MonHocs)
            {
                var li = new ListViewItem(m.MaMon) { Tag = m };
                li.SubItems.Add(m.TenMon);
                li.SubItems.Add(m.SoTinChi.ToString());
                _lv.Items.Add(li);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FormFieldHelper.GetInputText(_inputs, "MaMon"))) { MessageBox.Show("Nhập mã môn."); return; }
            if (DataStore.MonHocs.Exists(m => m.MaMon == FormFieldHelper.GetInputText(_inputs, "MaMon"))) { MessageBox.Show("Mã môn đã tồn tại."); return; }
            if (!int.TryParse(FormFieldHelper.GetInputText(_inputs, "SoTinChi"), out int tc) || tc < 0) { MessageBox.Show("Số tín chỉ không hợp lệ."); return; }
            DataStore.MonHocs.Add(new MonHoc { MaMon = FormFieldHelper.GetInputText(_inputs, "MaMon"), TenMon = FormFieldHelper.GetInputText(_inputs, "TenMon"), SoTinChi = tc });
            RefreshList();
            FormFieldHelper.ClearInputs(_inputs);
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (_lv.SelectedItems.Count == 0) { MessageBox.Show("Chọn môn cần sửa."); return; }
            var m = (MonHoc)_lv.SelectedItems[0].Tag!;
            m.TenMon = FormFieldHelper.GetInputText(_inputs, "TenMon");
            if (int.TryParse(FormFieldHelper.GetInputText(_inputs, "SoTinChi"), out int tc) && tc >= 0) m.SoTinChi = tc;
            RefreshList();
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (_lv.SelectedItems.Count == 0) { MessageBox.Show("Chọn môn cần xóa."); return; }
            if (MessageBox.Show("Xóa môn đã chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            var m = (MonHoc)_lv.SelectedItems[0].Tag!;
            DataStore.MonHocs.Remove(m);
            DataStore.Diems.RemoveAll(d => d.MaMon == m.MaMon);
            RefreshList();
            FormFieldHelper.ClearInputs(_inputs);
        }
    }
}
