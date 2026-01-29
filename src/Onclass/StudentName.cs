using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAss.src.Onclass
{
    internal class StudentName
    {
        static string GetName(string fullName)
        {
            var parts = fullName.Split(' ');
            return parts.Last();
        }

        static string GetRest(string fullName)
        {
            var parts = fullName.Split(' ');
            if (parts.Length <= 1) return "";
            return string.Join(" ", parts.Take(parts.Length - 1));
        }

        public static void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            int n;
            while (true)
            {
                Console.Write("Nhập số sinh viên: ");
                if (int.TryParse(Console.ReadLine(), out n) && n > 0)
                {
                    break;
                }
                Console.WriteLine("So luong khong hop le. Vui long nhap mot so nguyen duong.");
            }

            List<string> danhSach = new List<string>();
            for (int i = 0; i < n; i++)
            {
                string? name;
                while (true)
                {
                    Console.Write($"Sinh viên {i + 1}: ");
                    name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        danhSach.Add(name.Trim());
                        break;
                    }
                    Console.WriteLine("Ten khong duoc de trong. Vui long nhap lai.");
                }
            }

            var danhSachSapXep = danhSach.OrderBy(fullName => GetName(fullName))
                                         .ThenBy(fullName => GetRest(fullName))
                                         .ToList();

            Console.WriteLine("\nSắp xếp: ");
            for (int i = 0; i < danhSachSapXep.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {danhSachSapXep[i]}");
            }

            while (true)
            {
                Console.Write("\nTìm kiếm (Nhập \"Exit\" để thoát): ");
                string? target = Console.ReadLine();

                if (string.Equals(target, "Exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Đã thoát");
                    break;
                }

                if (string.IsNullOrWhiteSpace(target))
                {
                    Console.WriteLine("Ten tim kiem khong duoc de trong.");
                    continue;
                }
                
                string? foundStudent = danhSachSapXep.FirstOrDefault(s => s.Equals(target.Trim(), StringComparison.OrdinalIgnoreCase));

                if (foundStudent != null)
                {
                    int index = danhSachSapXep.IndexOf(foundStudent);
                    Console.WriteLine($"Tim thay '{foundStudent}' tai vi tri: {index + 1}");
                }
                else
                {
                    Console.WriteLine("Not Found");
                }
            }
        }
    }
}
