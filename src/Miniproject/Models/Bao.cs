using System;

namespace WindowsAss.src.Miniproject.Models
{
    public class Bao : TaiLieu
    {
        public DateTime NgayPhatHanh { get; set; }

        public Bao(string maTaiLieu, string tenNXB, int soBanPhatHanh, DateTime ngayPhatHanh)
            : base(maTaiLieu, tenNXB, soBanPhatHanh)
        {
            NgayPhatHanh = ngayPhatHanh;
        }

        public override string ToString()
        {
            return $"---\n" +
                   base.ToString() + "\n" +
                   $"  - Ngay phat hanh: {NgayPhatHanh:dd/MM/yyyy}";
        }
    }
}
