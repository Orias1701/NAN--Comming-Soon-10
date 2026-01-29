using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAss.src.Onclass
{
    public class PhanSoLogic
    {
        private int tuSo;
        private int mauSo;

        public PhanSoLogic(int tu = 0, int mau = 1)
        {
            this.tuSo = tu;
            this.mauSo = (mau == 0) ? 1 : mau;
            RutGon();
        }

        private int GCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (b != 0)
            {
                int t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        private void RutGon()
        {
            if (tuSo == 0) { mauSo = 1; return; }
            int ucln = GCD(tuSo, mauSo);
            tuSo /= ucln;
            mauSo /= ucln;
            if (mauSo < 0) { tuSo = -tuSo; mauSo = -mauSo; }
        }

        public PhanSoLogic Cong(PhanSoLogic ps) => new PhanSoLogic(tuSo * ps.mauSo + ps.tuSo * mauSo, mauSo * ps.mauSo);
        public PhanSoLogic Tru(PhanSoLogic ps) => new PhanSoLogic(tuSo * ps.mauSo - ps.tuSo * mauSo, mauSo * ps.mauSo);
        public PhanSoLogic Tich(PhanSoLogic ps) => new PhanSoLogic(tuSo * ps.tuSo, mauSo * ps.mauSo);
        public PhanSoLogic Thuong(PhanSoLogic ps) => new PhanSoLogic(tuSo * ps.mauSo, mauSo * ps.tuSo);

        public void InPhanSo()
        {
            if (tuSo == 0) Console.Write("0");
            else if (mauSo == 1) Console.Write(tuSo);
            else Console.Write($"{tuSo}/{mauSo}");
        }

        public static void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("--- Nhập phân số thứ nhất ---");
            PhanSoLogic ps1 = Nhap();
            Console.WriteLine("--- Nhập phân số thứ hai ---");
            PhanSoLogic ps2 = Nhap();

            Console.Write("\nTổng: "); ps1.Cong(ps2).InPhanSo();
            Console.Write("\nHiệu: "); ps1.Tru(ps2).InPhanSo();
            Console.Write("\nTích: "); ps1.Tich(ps2).InPhanSo();
            Console.Write("\nThương: "); ps1.Thuong(ps2).InPhanSo();
            Console.WriteLine();
        }

        private static PhanSoLogic Nhap()
        {
            int tu;
            while (true)
            {
                Console.Write("Nhập tử số: ");
                if (int.TryParse(Console.ReadLine(), out tu))
                {
                    break;
                }
                Console.WriteLine("Giá trị không hợp lệ. Vui lòng nhập một số nguyên.");
            }

            int mau;
            while (true)
            {
                Console.Write("Nhập mẫu số: ");
                if (int.TryParse(Console.ReadLine(), out mau) && mau != 0)
                {
                    break;
                }
                Console.WriteLine("Giá trị không hợp lệ. Mẫu số phải là một số nguyên khác 0.");
            }
            return new PhanSoLogic(tu, mau);
        }


    }
}