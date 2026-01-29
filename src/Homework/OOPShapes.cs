using System;

namespace WindowsAss.src.Homework
{
    // Lớp trừu tượng TuGiac
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

    // Lớp HinhCN kế thừa từ TuGiac
    public class HinhCN : TuGiac
    {
        public void Nhap()
        {
            double dai;
            while (true)
            {
                Console.Write("Nhap chieu dai: ");
                // Su dung TryParse de kiem tra dau vao an toan
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

    // Lớp HinhVuong kế thừa từ TuGiac
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

    // Lớp để chạy ví dụ
    public class ShapeRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n--- Hinh Chu Nhat (Rectangle) ---");
            HinhCN hcn = new HinhCN();
            hcn.Nhap();
            hcn.display();

            Console.WriteLine("\n--- Hinh Vuong (Square) ---");
            HinhVuong hv = new HinhVuong();
            hv.Nhap();
            hv.display();
        }
    }
}
