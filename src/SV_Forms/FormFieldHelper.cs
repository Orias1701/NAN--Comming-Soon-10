using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsAss.src.SV_Forms
{
    /// <summary>Loại control nhập liệu: Text, Number, Date, ComboBox.</summary>
    public enum FieldControlType
    {
        Text,
        Number,
        Date,
        ComboBox
    }

    /// <summary>Định nghĩa một trường: nhãn + control (TextBox, DateTimePicker, ComboBox).</summary>
    public class FieldDef
    {
        public string Key { get; set; } = "";
        public string LabelText { get; set; } = "";
        public FieldControlType Type { get; set; }
        public int InputWidth { get; set; } = 200;
    }

    /// <summary>Định nghĩa một nút: Text + Click handler.</summary>
    public class ButtonDef
    {
        public string Key { get; set; } = "";
        public string Text { get; set; } = "";
        public EventHandler? Click { get; set; }
    }

    /// <summary>Định nghĩa ListView: tên cột + độ rộng (optional).</summary>
    public class ListViewDef
    {
        public string[] ColumnNames { get; set; } = Array.Empty<string>();
        public int[]? ColumnWidths { get; set; }
        public int Width { get; set; } = 400;
        public int Height { get; set; } = 250;
    }

    /// <summary>Hàm chung: layout mặc định + tạo Label/Input, nút, ListView từ định nghĩa.</summary>
    public static class FormFieldHelper
    {
        public const int DefaultStartY = 20;
        public const int DefaultRowHeight = 35;
        public const int DefaultLabelX = 20;
        public const int DefaultInputX = 100;
        /// <summary>Chiều rộng thống nhất cho tất cả ô input trong form (tránh lệch layout).</summary>
        public const int DefaultInputWidth = 240;
        public const int DefaultButtonWidth = 80;
        public const int DefaultButtonHeight = 28;
        public const int DefaultButtonGapX = 10;
        /// <summary>Khoảng cách tối thiểu giữa cột input và cột nút.</summary>
        public const int DefaultInputToButtonGap = 16;
        public const int DefaultListViewMarginTop = 10;
        public const int DefaultListViewX = 20;

        /// <summary>Tính vị trí X bắt đầu cho cột nút sao cho không đè lên ô input (inputX + inputWidth + gap).</summary>
        public static int GetSuggestedButtonStartX(int inputX = DefaultInputX, int inputWidth = DefaultInputWidth) => inputX + inputWidth + DefaultInputToButtonGap;

        /// <summary>Tạo các cặp Label + Control theo danh sách FieldDef, thêm vào parent, trả về Dictionary[key] = Control. Nếu uniformInputWidth có giá trị thì mọi ô input dùng chung chiều rộng đó.</summary>
        public static Dictionary<string, Control> CreateFields(
            Control parent,
            IEnumerable<FieldDef> definitions,
            int startY = DefaultStartY,
            int labelX = DefaultLabelX,
            int inputX = DefaultInputX,
            int rowHeight = DefaultRowHeight,
            int? uniformInputWidth = null)
        {
            var result = new Dictionary<string, Control>();
            int y = startY;

            foreach (var def in definitions)
            {
                int inputW = uniformInputWidth ?? def.InputWidth;

                var lbl = new Label
                {
                    Text = def.LabelText,
                    Location = new Point(labelX, y),
                    AutoSize = true
                };
                Control input = def.Type switch
                {
                    FieldControlType.Text => new TextBox { Location = new Point(inputX, y - 2), Width = inputW },
                    FieldControlType.Number => new TextBox { Location = new Point(inputX, y - 2), Width = inputW },
                    FieldControlType.Date => new DateTimePicker
                    {
                        Location = new Point(inputX, y - 2),
                        Width = inputW,
                        Format = DateTimePickerFormat.Short
                    },
                    FieldControlType.ComboBox => new ComboBox
                    {
                        Location = new Point(inputX, y - 2),
                        Width = inputW,
                        DropDownStyle = ComboBoxStyle.DropDownList
                    },
                    _ => new TextBox { Location = new Point(inputX, y - 2), Width = inputW }
                };

                input.Name = def.Key;
                parent.Controls.Add(lbl);
                parent.Controls.Add(input);
                result[def.Key] = input;
                y += rowHeight;
            }

            return result;
        }

        /// <summary>Tạo các nút từ danh sách ButtonDef, xếp dọc bên phải; thêm vào parent. Mặc định startX = GetSuggestedButtonStartX() để không đè lên ô input.</summary>
        public static Dictionary<string, Button> CreateButtons(
            Control parent,
            IEnumerable<ButtonDef> definitions,
            int startX = -1,
            int startY = DefaultStartY - 2,
            int rowHeight = DefaultRowHeight,
            int width = DefaultButtonWidth,
            int height = DefaultButtonHeight)
        {
            var result = new Dictionary<string, Button>();
            int y = startY;
            foreach (var def in definitions)
            {
                var btn = new Button
                {
                    Text = def.Text,
                    Location = new Point(startX, y),
                    Size = new Size(width, height)
                };
                if (def.Click != null) btn.Click += def.Click;
                btn.Name = def.Key;
                parent.Controls.Add(btn);
                result[def.Key] = btn;
                y += rowHeight;
            }
            return result;
        }

        /// <summary>Tạo ListView từ ListViewDef, đặt dưới vùng field (tính Y từ fieldCount); thêm vào parent.</summary>
        public static ListView CreateListView(
            Control parent,
            ListViewDef def,
            int listY)
        {
            var lv = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                Location = new Point(DefaultListViewX, listY),
                Size = new Size(def.Width, def.Height)
            };
            foreach (var name in def.ColumnNames)
                lv.Columns.Add(name);
            if (def.ColumnWidths != null)
            {
                for (int i = 0; i < Math.Min(def.ColumnWidths.Length, lv.Columns.Count); i++)
                    lv.Columns[i].Width = def.ColumnWidths[i];
            }
            parent.Controls.Add(lv);
            return lv;
        }

        /// <summary>Tính Y bắt đầu cho ListView sau khi có fieldCount dòng field.</summary>
        public static int ListViewY(int fieldCount, int startY = DefaultStartY, int rowHeight = DefaultRowHeight, int marginTop = DefaultListViewMarginTop)
            => startY + fieldCount * rowHeight + marginTop;

        /// <summary>Lấy giá trị text từ Control (TextBox, DateTimePicker). ComboBox cần xử lý riêng trong form (SelectedItem).</summary>
        public static string GetText(Control c)
        {
            if (c is TextBox tb) return tb.Text.Trim();
            if (c is DateTimePicker dtp) return dtp.Value.ToShortDateString();
            if (c is ComboBox cb) return cb.SelectedItem?.ToString() ?? "";
            return "";
        }

        /// <summary>Lấy giá trị nhập từ control theo key. Với ComboBox: dùng getComboValue(SelectedItem) nếu có, không thì ToString().</summary>
        public static string GetInputText(Dictionary<string, Control> inputs, string key, Func<object?, string>? getComboValue = null)
        {
            if (!inputs.TryGetValue(key, out var c)) return "";
            if (c is TextBox tb) return tb.Text.Trim();
            if (c is DateTimePicker dtp) return dtp.Value.ToShortDateString();
            if (c is ComboBox cb) return getComboValue != null ? getComboValue(cb.SelectedItem) : (cb.SelectedItem?.ToString() ?? "");
            return "";
        }

        /// <summary>Gán giá trị text cho Control (TextBox, DateTimePicker). ComboBox form tự gán SelectedItem.</summary>
        public static void SetText(Control c, string value)
        {
            if (c is TextBox tb) tb.Text = value;
            else if (c is DateTimePicker dtp && DateTime.TryParse(value, out var d)) dtp.Value = d;
        }

        /// <summary>Xóa toàn bộ input: TextBox Clear(), DateTimePicker = Today, ComboBox SelectedIndex = -1.</summary>
        public static void ClearInputs(Dictionary<string, Control> inputs)
        {
            foreach (var c in inputs.Values)
            {
                if (c is TextBox tb) tb.Clear();
                else if (c is DateTimePicker dtp) dtp.Value = DateTime.Today;
                else if (c is ComboBox cb) cb.SelectedIndex = -1;
            }
        }
    }
}
