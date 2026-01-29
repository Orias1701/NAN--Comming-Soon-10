using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WindowsAss.src.Onclass;

namespace WindowsAss
{
    public abstract class NhanVien
    {
        public string HoTen { get; set; }
        public int Tuoi { get; set; }
        public string DiaChi { get; set; }

        public NhanVien(string ten, int tuoi, string diaChi)
        {
            HoTen = ten;
            Tuoi = tuoi;
            DiaChi = diaChi;
        }

        public abstract double TinhLuong();

        public virtual void HienThiThongTin()
        {
            Console.Write($"{GetType().Name.Replace("NhanVien", ""),-5} | {HoTen,-20} | {Tuoi,-5} | {DiaChi,-15} | ");
        }
    }

    public class NhanVienX : NhanVien
    {
        private const double HE_SO_X = 20000;

        public int SoSanPham { get; set; }

        public NhanVienX(string ten, int tuoi, string diaChi, int soSanPham)
            : base(ten, tuoi, diaChi)
        {
            SoSanPham = soSanPham;
        }

        public override double TinhLuong()
        {
            return SoSanPham * HE_SO_X;
        }
    }

    public class NhanVienY : NhanVien
    {
        private const double HE_SO_Y = 50000;

        public int SoNgayCong { get; set; }

        public NhanVienY(string ten, int tuoi, string diaChi, int soNgayCong)
            : base(ten, tuoi, diaChi)
        {
            SoNgayCong = soNgayCong;
        }

        public override double TinhLuong()
        {
            return SoNgayCong * HE_SO_Y;
        }
    }

    public class NhanVienZ : NhanVien
    {
        public double LuongCoBan { get; set; }
        public double HeSoLuong { get; set; }

        public NhanVienZ(string ten, int tuoi, string diaChi, double luongCB, double heSo)
            : base(ten, tuoi, diaChi)
        {
            LuongCoBan = luongCB;
            HeSoLuong = heSo;
        }

        public override double TinhLuong()
        {
            return LuongCoBan * HeSoLuong;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            List<NhanVien> danhSachNV = new List<NhanVien>();

            try
            {
                Console.Write("Nhập số lượng nhân viên (N): ");
                int n = int.Parse(Console.ReadLine());

                Console.WriteLine("Nhập thông tin theo định dạng (cách nhau bởi khoảng trắng):");
                Console.WriteLine("- Loại X: X Ten Tuoi DiaChi SoSanPham");
                Console.WriteLine("- Loại Y: Y Ten Tuoi DiaChi SoNgayCong");
                Console.WriteLine("- Loại Z: Z Ten Tuoi DiaChi LuongCoBan HeSoLuong");
                Console.WriteLine("------------------------------------------------");

                for (int i = 0; i < n; i++)
                {
                    string line = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    string loai = parts[0].ToUpper();
                    string ten = parts[1];
                    int tuoi = int.Parse(parts[2]);
                    string diaChi = parts[3];

                    switch (loai)
                    {
                        case "X":
                            int sp = int.Parse(parts[4]);
                            danhSachNV.Add(new NhanVienX(ten, tuoi, diaChi, sp));
                            break;

                        case "Y":
                            int ngay = int.Parse(parts[4]);
                            danhSachNV.Add(new NhanVienY(ten, tuoi, diaChi, ngay));
                            break;

                        case "Z":
                            double luongCB = double.Parse(parts[4]);
                            double heSo = double.Parse(parts[5]);
                            danhSachNV.Add(new NhanVienZ(ten, tuoi, diaChi, luongCB, heSo));
                            break;

                        default:
                            Console.WriteLine($"Loại nhân viên '{loai}' không hợp lệ.");
                            break;
                    }
                }

                Console.WriteLine("\n--- DANH SÁCH LƯƠNG NHÂN VIÊN ---");
                Console.WriteLine($"{"Loại",-5} | {"Họ Tên",-20} | {"Tuổi",-5} | {"Địa Chỉ",-15} | {"Lương Thực Lĩnh",-15}");
                Console.WriteLine(new string('-', 70));

                foreach (var nv in danhSachNV)
                {
                    nv.HienThiThongTin();
                    Console.WriteLine($"{nv.TinhLuong().ToString("#,##0")} VNĐ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi nhập liệu: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}

// X = Sản xuất
// Y = Công nhật
// Z = Quản lý

// 3
// X NguyenVanA 25 Hanoi 100
// Y LeThiB 30 Saigon 22
// Z TranVanC 40 Danang 5000000 2.5