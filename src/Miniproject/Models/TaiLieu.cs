namespace WindowsAss.src.Miniproject.Models
{
    public abstract class TaiLieu
    {
        public string MaTaiLieu { get; private set; }
        public string TenNXB { get; set; }
        public int SoBanPhatHanh { get; set; }

        protected TaiLieu(string maTaiLieu, string tenNXB, int soBanPhatHanh)
        {
            MaTaiLieu = maTaiLieu;
            TenNXB = tenNXB;
            SoBanPhatHanh = soBanPhatHanh;
        }

        public override string ToString()
        {
            return $"  - Ma tai lieu: {MaTaiLieu}\n" +
                   $"  - Ten NXB: {TenNXB}\n" +
                   $"  - So ban phat hanh: {SoBanPhatHanh}";
        }
    }
}
