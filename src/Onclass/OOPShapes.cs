using System;

namespace WindowsAss.src.Homework
{
    public abstract class TuGiac
    {
        protected double ChieuDai { get; set; }
        protected double ChieuRong { get; set; }

        public abstract double getArea(); // Diện tích

        public abstract double getPeri(); // Chu vi

        public virtual void display() // Hiển thị
        {
            Console.WriteLine($"Chu vi: {getPeri()}");
            Console.WriteLine($"Dien tich: {getArea()}");
        }
    }

    public class HinhCN : TuGiac
    {
        public void Nhap()
        {
            double dai;
            while (true)
            {
                Console.Write("Nhap chieu dai: ");
                if (double.TryParse(Console.ReadLine(), out dai) && dai > 0)
                {
                    this.ChieuDai = dai;
                    break;
                }
                Console.WriteLine("Chieu dai khong hop le. Vui long nhap mot so duong.");
            }

            double rong;
            while (true)
            {
                Console.Write("Nhap chieu rong: ");
                if (double.TryParse(Console.ReadLine(), out rong) && rong > 0)
                {
                    this.ChieuRong = rong;
                    break;
                }
                Console.WriteLine("Chieu rong khong hop le. Vui long nhap mot so duong.");
            }
        }

        public override double getArea()
        {
            return ChieuDai * ChieuRong;
        }

        public override double getPeri()
        {
            return (ChieuDai + ChieuRong) * 2;
        }
    }

    public class HinhVuong : TuGiac
    {
        public void Nhap()
        {
            double canh;
            while (true)
            {
                Console.Write("Nhap do dai canh: ");
                if (double.TryParse(Console.ReadLine(), out canh) && canh > 0)
                {
                    this.ChieuDai = canh;
                    this.ChieuRong = canh;
                    break;
                }
                Console.WriteLine("Do dai canh khong hop le. Vui long nhap mot so duong.");
            }
        }

        public override double getArea()
        {
            return ChieuDai * ChieuDai;
        }

        public override double getPeri()
        {
            return ChieuDai * 4;
        }
    }

    public class ShapeRunner
    {
        public static void Run()
        {
            while (true)
            {
                Console.WriteLine("\n=== May tinh hinh hoc ===");
                Console.WriteLine("Chon loai hinh ban muon tinh:");
                Console.WriteLine("  0: Hinh Chu Nhat");
                Console.WriteLine("  1: Hinh Vuong");
                Console.WriteLine("  Else: Thoat");
                Console.Write("Lua chon cua ban: ");

                if (!int.TryParse(Console.ReadLine(), out int type))
                {
                    Console.WriteLine("==> Thoat chuong trinh.");
                    break;
                }
                if (type == 0)
                {
                    Console.WriteLine("\n--- Hinh Chu Nhat ---");
                    HinhCN hcn = new HinhCN();
                    hcn.Nhap();
                    hcn.display();
                }
                else if (type == 1)
                {
                    Console.WriteLine("\n--- Hinh Vuong ---");
                    HinhVuong hv = new HinhVuong();
                    hv.Nhap();
                    hv.display();
                }
                else
                {
                    Console.WriteLine("==> Thoat chuong trinh.");
                    break;
                }
            }
        }
    }
}
