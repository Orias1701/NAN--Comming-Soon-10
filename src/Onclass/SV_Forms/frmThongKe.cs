using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsAss.src.Onclass.SV_Forms
{
    /// <summary>Form con placeholder: Thống kê SV theo khoa (giữ chỗ).</summary>
    public class frmThongKe : Form
    {
        public frmThongKe()
        {
            this.Text = "Thống kê theo khoa";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            var lbl = new Label
            {
                Text = "Chức năng thống kê sinh viên theo khoa sẽ được bổ sung sau.",
                AutoSize = true,
                Location = new Point(40, 40),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lbl);
        }
    }
}
