using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WindowsAss.src.Onclass
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

        public virtual void HienThiThongTin()
        {
            Console.WriteLine($"  - Ma tai lieu: {MaTaiLieu}");
            Console.WriteLine($"  - Ten NXB: {TenNXB}");
            Console.WriteLine($"  - So ban phat hanh: {SoBanPhatHanh}");
        }
    }

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

        public override void HienThiThongTin()
        {
            Console.WriteLine("--- Sach ---");
            base.HienThiThongTin();
            Console.WriteLine($"  - Ten tac gia: {TenTacGia}");
            Console.WriteLine($"  - So trang: {SoTrang}");
        }
    }

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

        public override void HienThiThongTin()
        {
            Console.WriteLine("--- Tap Chi ---");
            base.HienThiThongTin();
            Console.WriteLine($"  - So phat hanh: {SoPhatHanh}");
            Console.WriteLine($"  - Thang phat hanh: {ThangPhatHanh}");
        }
    }

    public class Bao : TaiLieu
    {
        public DateTime NgayPhatHanh { get; set; }

        public Bao(string maTaiLieu, string tenNXB, int soBanPhatHanh, DateTime ngayPhatHanh)
            : base(maTaiLieu, tenNXB, soBanPhatHanh)
        {
            NgayPhatHanh = ngayPhatHanh;
        }

        public override void HienThiThongTin()
        {
            Console.WriteLine("--- Bao ---");
            base.HienThiThongTin();
            Console.WriteLine($"  - Ngay phat hanh: {NgayPhatHanh:dd/MM/yyyy}");
        }
    }

    public class QuanLySach
    {
        private readonly List<TaiLieu> _danhSachTaiLieu = new List<TaiLieu>();

        private string NhapChuoi(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Gia tri khong duoc de trong.");
                }
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        private int NhapSoNguyen(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            int number;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine() ?? "", out number) && number >= min && number <= max)
                {
                    return number;
                }
                Console.WriteLine($"Gia tri khong hop le. Vui long nhap mot so nguyen tu {min} den {max}.");
            }
        }

        private DateTime NhapNgay(string prompt)
        {
            DateTime date;
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParseExact(Console.ReadLine() ?? "", "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }
                Console.WriteLine("Dinh dang ngay khong hop le. Vui long nhap theo dinh dang dd/MM/yyyy.");
            }
        }

        public void ThemMoiTaiLieu()
        {
            Console.WriteLine("\n[1] Them Sach\n[2] Them Tap Chi\n[3] Them Bao");
            string choice = NhapChuoi("Chon loai tai lieu: ");

            string maTaiLieu;
            while (true)
            {
                maTaiLieu = NhapChuoi("Ma tai lieu: ");
                if (!_danhSachTaiLieu.Any(tl => tl.MaTaiLieu.Equals(maTaiLieu, StringComparison.OrdinalIgnoreCase)))
                {
                    break;
                }
                Console.WriteLine("Ma tai lieu da ton tai. Vui long nhap ma khac.");
            }

            string tenNXB = NhapChuoi("Ten nha xuat ban: ");
            int soBanPhatHanh = NhapSoNguyen("So ban phat hanh: ", 1);

            switch (choice)
            {
                case "1":
                    string tenTacGia = NhapChuoi("Ten tac gia: ");
                    int soTrang = NhapSoNguyen("So trang: ", 1);
                    _danhSachTaiLieu.Add(new Sach(maTaiLieu, tenNXB, soBanPhatHanh, tenTacGia, soTrang));
                    break;
                case "2":
                    int soPhatHanh = NhapSoNguyen("So phat hanh: ", 1);
                    int thangPhatHanh = NhapSoNguyen("Thang phat hanh: ", 1, 12);
                    _danhSachTaiLieu.Add(new TapChi(maTaiLieu, tenNXB, soBanPhatHanh, soPhatHanh, thangPhatHanh));
                    break;
                case "3":
                    DateTime ngayPhatHanh = NhapNgay("Ngay phat hanh (dd/MM/yyyy): ");
                    _danhSachTaiLieu.Add(new Bao(maTaiLieu, tenNXB, soBanPhatHanh, ngayPhatHanh));
                    break;
                default:
                    Console.WriteLine("Lua chon khong hop le.");
                    return;
            }
            Console.WriteLine("=> Them moi tai lieu thanh cong.");
        }

        public void XoaTaiLieu()
        {
            if (_danhSachTaiLieu.Count == 0)
            {
                Console.WriteLine("Thu vien chua co tai lieu nao de xoa.");
                return;
            }
            string maTaiLieu = NhapChuoi("Nhap ma tai lieu can xoa: ");
            var taiLieu = _danhSachTaiLieu.FirstOrDefault(tl => tl.MaTaiLieu.Equals(maTaiLieu, StringComparison.OrdinalIgnoreCase));
            if (taiLieu != null)
            {
                _danhSachTaiLieu.Remove(taiLieu);
                Console.WriteLine("=> Xoa tai lieu thanh cong.");
            }
            else
            {
                Console.WriteLine("=> Khong tim thay tai lieu voi ma da cho.");
            }
        }

        public void HienThiThongTin()
        {
            if (_danhSachTaiLieu.Count == 0)
            {
                Console.WriteLine("Thu vien chua co tai lieu nao.");
                return;
            }
            Console.WriteLine("\n--- Danh sach tat ca tai lieu ---");
            foreach (var taiLieu in _danhSachTaiLieu)
            {
                taiLieu.HienThiThongTin();
                Console.WriteLine();
            }
        }

        public void TimKiemTheoLoai()
        {
             if (_danhSachTaiLieu.Count == 0)
            {
                Console.WriteLine("Thu vien chua co tai lieu nao de tim kiem.");
                return;
            }
            Console.WriteLine("\n[1] Tim Sach\n[2] Tim Tap Chi\n[3] Tim Bao");
            string choice = NhapChuoi("Chon loai tai lieu: ");

            var ketQua = new List<TaiLieu>();
            string typeName = "";

            switch (choice)
            {
                case "1":
                    ketQua = _danhSachTaiLieu.OfType<Sach>().Cast<TaiLieu>().ToList();
                    typeName = "Sach";
                    break;
                case "2":
                    ketQua = _danhSachTaiLieu.OfType<TapChi>().Cast<TaiLieu>().ToList();
                    typeName = "Tap Chi";
                    break;
                case "3":
                    ketQua = _danhSachTaiLieu.OfType<Bao>().Cast<TaiLieu>().ToList();
                    typeName = "Bao";
                    break;
                default:
                    Console.WriteLine("Lua chon khong hop le.");
                    return;
            }

            if (ketQua.Count == 0)
            {
                Console.WriteLine($"=> Khong co {typeName} nao trong thu vien.");
            }
            else
            {
                Console.WriteLine($"\n--- Ket qua tim kiem: {ketQua.Count} {typeName} ---");
                foreach (var taiLieu in ketQua)
                {
                    taiLieu.HienThiThongTin();
                    Console.WriteLine();
                }
            }
        }

        public static void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            QuanLySach qls = new QuanLySach();
            while (true)
            {
                Console.WriteLine("\n========== HE THONG QUAN LY THU VIEN ==========");
                Console.WriteLine(" [1] Them moi tai lieu");
                Console.WriteLine(" [2] Xoa tai lieu theo ma");
                Console.WriteLine(" [3] Hien thi thong tin tai lieu");
                Console.WriteLine(" [4] Tim kiem theo loai");
                Console.WriteLine(" [0] Thoat");
                Console.WriteLine("===============================================");
                
                string choice = qls.NhapChuoi("Lua chon cua ban: ");

                switch (choice)
                {
                    case "1":
                        qls.ThemMoiTaiLieu();
                        break;
                    case "2":
                        qls.XoaTaiLieu();
                        break;
                    case "3":
                        qls.HienThiThongTin();
                        break;
                    case "4":
                        qls.TimKiemTheoLoai();
                        break;
                    case "0":
                        Console.WriteLine("Da thoat chuong trinh.");
                        return;
                    default:
                        Console.WriteLine("Lua chon khong hop le. Vui long chon lai.");
                        break;
                }
            }
        }
    }
}