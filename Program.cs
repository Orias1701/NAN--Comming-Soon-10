using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WindowsAss.src.Onclass;
using WindowsAss.src.Homework;
using WindowsAss.src.Miniproject.UI;

namespace WindowsAss
{    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while (true)
            {
                Console.WriteLine("\n========= CHON CHUONG TRINH DE CHAY =========");
                Console.WriteLine("1. Getting Started (Hello World)");
                Console.WriteLine("2. Array Processor (Sap xep chan le)");
                Console.WriteLine("3. Phan So Logic (Phep toan phan so)");
                Console.WriteLine("4. Student Name Sorter (Sap xep ten)");
                Console.WriteLine("5. OOP Shapes (Tinh chu vi, dien tich)");
                Console.WriteLine("6. Library Manager (Quan ly thu vien)");
                Console.WriteLine("7. Quadratic Equation (Phuong trinh bac 2)");
                Console.WriteLine("0. Thoat");
                Console.WriteLine("=============================================");
                Console.Write("Lua chon cua ban: ");
                string choice = Console.ReadLine() ?? "";

                Console.Clear();

                switch (choice)
                {
                    case "1":
                        GettingStarted.Run();
                        break;
                    case "2":
                        ArrayProcessor.Run();
                        break;
                    case "3":
                        PhanSoLogic.Run();
                        break;
                    case "4":
                        StudentName.Run();
                        break;
                    case "5":
                        ShapeRunner.Run();
                        break;
                    case "6":
                        ConsoleUI.Run();
                        break;
                    case "7":
                        QuadraticEquation.Run();
                        break;
                    case "0":
                        Console.WriteLine("Da thoat chuong trinh chinh.");
                        return;
                    default:
                        Console.WriteLine("Lua chon khong hop le. Vui long chon lai.");
                        break;
                }
                
                Console.WriteLine("\nNhan phim bat ky de quay lai menu chinh...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}