namespace WindowsAss.src.Miniproject.Models
{
    public class TapChi : TaiLieu
    {
        public int SoPhatHanh { get; set; }
        public int ThangPhatHanh { get; set; }

        public TapChi(string maTaiLieu, string tenNXB, int soBanPhatHanh, int soPhatHanh, int thangPhatHanh)
            : base(maTaiLieu, tenNXB, soBanPhatHanh)
        {
            SoPhatHanh = soPhatHanh;
            ThangPhatHanh = thangPhatHanh;
        }

        public override string ToString()
        {
            return "--- Tap Chi ---\"n" +
                   base.ToString() + "\n" +
                   "  - So phat hanh: " + SoPhatHanh + "\n" +
                   "  - Thang phat hanh: " + ThangPhatHanh;
        }
    }
}
