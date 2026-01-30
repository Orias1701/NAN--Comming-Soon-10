namespace WindowsAss.src.Miniproject.Models
{
    public class Sach : TaiLieu
    {
        public string TenTacGia { get; set; }
        public int SoTrang { get; set; }

        public Sach(string maTaiLieu, string tenNXB, int soBanPhatHanh, string tenTacGia, int soTrang)
            : base(maTaiLieu, tenNXB, soBanPhatHanh)
        {
            TenTacGia = tenTacGia;
            SoTrang = soTrang;
        }

        public override string ToString()
        {
            return $"---\n" +
                   base.ToString() + "\n" +
                   $"  - Ten tac gia: {TenTacGia}\n" +
                   $"  - So trang: {SoTrang}";
        }
    }
}
