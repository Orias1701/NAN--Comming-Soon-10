using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAss.src.Onclass
{
    public class GettingStarted
    {
        public int GCD(int a, int b)
        {
            while (b != 0)
            {
                int r = a % b;
                a = b;
                b = r;
            }
            return a;
        }
        public void PrintPS(int a, int b, int c, int d)
        {
            int tu = a * d + c * b;
            int mau = b * d;

            int ucln = GCD(tu, mau);

            tu /= ucln;
            mau /= ucln;

            Console.WriteLine("result = {0}/{1}\n", tu, mau);
        }
        public static void Run()
        {
            int a, b, c, d;

            a = ReadInteger("a");
            b = ReadInteger("b", nonZero: true);
            c = ReadInteger("c");
d = ReadInteger("d", nonZero: true);

            GettingStarted pr = new GettingStarted();
            pr.PrintPS(a, b, c, d);
        }

        private static int ReadInteger(string variableName, bool nonZero = false)
        {
            int value;
            while (true)
            {
                Console.Write($"\nNhap {variableName} = ");
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    if (nonZero && value == 0)
                    {
                        Console.WriteLine("Gia tri nay phai khac 0. Vui long nhap lai.");
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                {
                    Console.WriteLine("Gia tri khong hop le. Vui long nhap mot so nguyen.");
                }
            }
        }
    }
}
