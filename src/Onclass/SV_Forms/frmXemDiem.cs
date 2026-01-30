using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsAss.src.Onclass.SV_Forms
{
    /// <summary>Form con placeholder: Tra cứu điểm (giữ chỗ).</summary>
    public class frmXemDiem : Form
    {
        public frmXemDiem()
        {
            this.Text = "Xem điểm";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            var lbl = new Label
            {
                Text = "Chức năng tra cứu điểm sẽ được bổ sung sau.",
                AutoSize = true,
                Location = new Point(40, 40),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lbl);
        }
    }
}
