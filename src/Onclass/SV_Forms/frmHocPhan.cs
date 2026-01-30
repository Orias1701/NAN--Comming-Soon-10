using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace WindowsAss.src.Onclass.SV_Forms
{
    /// <summary>Form con: SQLite cs_assignment.db. Xóa mềm dùng cột Status (0 = Active, 1 = Deleted).</summary>
    public class frmHocPhan : Form
    {
        private const int StatusActive = 0;
        private const int StatusDeleted = 1;

        private static string ConnectionString => "Data Source=" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cs_assignment.db");
        private Dictionary<string, Control> _inputs = null!;
        private ListView _lv = null!;

        public frmHocPhan()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "THÔNG TIN VỀ HỌC PHẦN";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += FrmHocPhan_Load;

            const int marginBottom = 24;
            const int marginRight = 24;
            const int listViewMarginTop = 16;
            const int listViewHeight = 220;
            const int listViewWidth = 500;

            var fields = new[]
            {
                new FieldDef { Key = "MaHP", LabelText = "Mã học phần:", Type = FieldControlType.Text },
                new FieldDef { Key = "TenHP", LabelText = "Tên học phần:", Type = FieldControlType.Text },
                new FieldDef { Key = "SoDVHT", LabelText = "Số ĐVHT:", Type = FieldControlType.Number }
            };
            _inputs = FormFieldHelper.CreateFields(this, fields, uniformInputWidth: FormFieldHelper.DefaultInputWidth);

            var buttons = new[]
            {
                new ButtonDef { Key = "Them", Text = "Thêm", Click = BtnThem_Click },
                new ButtonDef { Key = "Sua", Text = "Sửa", Click = BtnSua_Click },
                new ButtonDef { Key = "Xoa", Text = "Xóa", Click = BtnXoa_Click }
            };
            FormFieldHelper.CreateButtons(this, buttons, startX: FormFieldHelper.GetSuggestedButtonStartX(), startY: FormFieldHelper.DefaultStartY - 2);

            int listY = FormFieldHelper.ListViewY(fields.Length, marginTop: listViewMarginTop);
            _lv = FormFieldHelper.CreateListView(this, new ListViewDef
            {
                ColumnNames = new[] { "Mã học phần", "Tên học phần", "Số ĐVHT" },
                ColumnWidths = new[] { 100, 280, 80 },
                Width = listViewWidth,
                Height = listViewHeight
            }, listY);

            this.ClientSize = new Size(FormFieldHelper.DefaultListViewX + listViewWidth + marginRight, listY + listViewHeight + marginBottom);
            _lv.SelectedIndexChanged += (s, e) =>
            {
                if (_lv.SelectedItems.Count == 0)
                {
                    SetMaHPReadOnly(false);
                    return;
                }
                var li = _lv.SelectedItems[0];
                FormFieldHelper.SetText(_inputs["MaHP"], li.Text);
                FormFieldHelper.SetText(_inputs["TenHP"], li.SubItems.Count > 1 ? li.SubItems[1].Text : "");
                FormFieldHelper.SetText(_inputs["SoDVHT"], li.SubItems.Count > 2 ? li.SubItems[2].Text : "");
                SetMaHPReadOnly(true);
            };
        }

        private void SetMaHPReadOnly(bool readOnly)
        {
            if (_inputs["MaHP"] is TextBox tb) tb.ReadOnly = readOnly;
        }

        private void FrmHocPhan_Load(object? sender, EventArgs e)
        {
            EnsureDatabase();
            LoadHocPhanToListView();
        }

        private static void EnsureDatabase()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqliteCommand(
                "CREATE TABLE IF NOT EXISTS HocPhan (MaHP TEXT PRIMARY KEY, TenHP TEXT, SoDVHT INTEGER, Status INTEGER DEFAULT 0)", conn);
            cmd.ExecuteNonQuery();
        }

        private void LoadHocPhanToListView()
        {
            _lv.Items.Clear();
            try
            {
                using var conn = new SqliteConnection(ConnectionString);
                conn.Open();
                using var cmd = new SqliteCommand(
                    "SELECT MaHP, TenHP, SoDVHT FROM HocPhan WHERE (Status = @Active OR Status IS NULL) ORDER BY MaHP",
                    conn);
                cmd.Parameters.AddWithValue("@Active", StatusActive);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var li = new ListViewItem(reader.GetString(0).Trim());
                    li.SubItems.Add(reader.IsDBNull(1) ? "" : reader.GetString(1).Trim());
                    li.SubItems.Add(reader.IsDBNull(2) ? "" : reader.GetInt64(2).ToString());
                    _lv.Items.Add(li);
                }
            }
            catch (SqliteException ex)
            {
                MessageBox.Show("Không thể kết nối hoặc đọc dữ liệu.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            var ma = FormFieldHelper.GetInputText(_inputs, "MaHP");
            var ten = FormFieldHelper.GetInputText(_inputs, "TenHP");
            if (string.IsNullOrWhiteSpace(ma)) { MessageBox.Show("Nhập mã học phần."); return; }
            if (!int.TryParse(FormFieldHelper.GetInputText(_inputs, "SoDVHT"), out int dvht) || dvht < 0) { MessageBox.Show("Số ĐVHT không hợp lệ."); return; }

            if (MaHPExists(ma))
            {
                MessageBox.Show("Mã học phần đã tồn tại.");
                return;
            }

            try
            {
                using var conn = new SqliteConnection(ConnectionString);
                conn.Open();
                using var cmd = new SqliteCommand(
                    "INSERT INTO HocPhan (MaHP, TenHP, SoDVHT, Status) VALUES (@MaHP, @TenHP, @SoDVHT, @Status)",
                    conn);
                cmd.Parameters.AddWithValue("@MaHP", ma);
                cmd.Parameters.AddWithValue("@TenHP", string.IsNullOrEmpty(ten) ? DBNull.Value : ten);
                cmd.Parameters.AddWithValue("@SoDVHT", dvht);
                cmd.Parameters.AddWithValue("@Status", StatusActive);
                cmd.ExecuteNonQuery();
                LoadHocPhanToListView();
                FormFieldHelper.ClearInputs(_inputs);
                SetMaHPReadOnly(false);
                MessageBox.Show("Đã thêm học phần.");
            }
            catch (SqliteException ex)
            {
                if ((int)ex.SqliteErrorCode == 19) MessageBox.Show("Mã học phần đã tồn tại.");
                else MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private static bool MaHPExists(string maHP)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqliteCommand("SELECT 1 FROM HocPhan WHERE MaHP = @MaHP", conn);
            cmd.Parameters.AddWithValue("@MaHP", maHP);
            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (_lv.SelectedItems.Count == 0) { MessageBox.Show("Chọn học phần cần sửa."); return; }
            var ma = FormFieldHelper.GetInputText(_inputs, "MaHP");
            var ten = FormFieldHelper.GetInputText(_inputs, "TenHP");
            if (!int.TryParse(FormFieldHelper.GetInputText(_inputs, "SoDVHT"), out int dvht) || dvht < 0) { MessageBox.Show("Số ĐVHT không hợp lệ."); return; }

            try
            {
                using var conn = new SqliteConnection(ConnectionString);
                conn.Open();
                using var cmd = new SqliteCommand(
                    "UPDATE HocPhan SET TenHP = @TenHP, SoDVHT = @SoDVHT WHERE MaHP = @MaHP",
                    conn);
                cmd.Parameters.AddWithValue("@MaHP", ma);
                cmd.Parameters.AddWithValue("@TenHP", string.IsNullOrEmpty(ten) ? DBNull.Value : ten);
                cmd.Parameters.AddWithValue("@SoDVHT", dvht);
                var n = cmd.ExecuteNonQuery();
                if (n > 0) { LoadHocPhanToListView(); MessageBox.Show("Đã cập nhật."); }
            }
            catch (SqliteException ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (_lv.SelectedItems.Count == 0) { MessageBox.Show("Chọn học phần cần xóa."); return; }
            if (MessageBox.Show("Xóa học phần đã chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            var ma = FormFieldHelper.GetInputText(_inputs, "MaHP");

            try
            {
                using var conn = new SqliteConnection(ConnectionString);
                conn.Open();
                using var cmd = new SqliteCommand(
                    "UPDATE HocPhan SET Status = @Status WHERE MaHP = @MaHP",
                    conn);
                cmd.Parameters.AddWithValue("@MaHP", ma);
                cmd.Parameters.AddWithValue("@Status", StatusDeleted);
                cmd.ExecuteNonQuery();
                LoadHocPhanToListView();
                FormFieldHelper.ClearInputs(_inputs);
                SetMaHPReadOnly(false);
                MessageBox.Show("Đã xóa học phần.");
            }
            catch (SqliteException ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }
    }
}
