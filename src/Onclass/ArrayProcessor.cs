using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAss.src.Onclass
{
    public class ArrayProcessor
    {
                public static void Run()
                {
                    int n;
                    while (true)
                    {
                        Console.Write("\nNhap so luong phan tu cua mang: ");
                        if (int.TryParse(Console.ReadLine(), out n) && n > 0)
                        {
                            break;
                        }
                        Console.WriteLine("So luong khong hop le. Vui long nhap mot so nguyen duong.");
                    }
        
                    int[] arr = new int[n];
                    for (int i = 0; i < n; i++)
                    {
                        int tempValue;
                        while (true)
                        {
                            Console.Write($"arr[{i}] = ");
                            if (int.TryParse(Console.ReadLine(), out tempValue))
                            {
                                arr[i] = tempValue;
                                break;
                            }
                            Console.WriteLine("Gia tri khong hop le. Vui long nhap mot so nguyen.");
                        }
                    }
        
                    Console.WriteLine("\nMang da nhap:");
                    Console.WriteLine(string.Join(" ", arr));
        
                    List<int> evens = arr.Where(x => x % 2 == 0)
                                         .OrderByDescending(x => x)
                                         .ToList();
        
                    List<int> odds = arr.Where(x => x % 2 != 0)
                                        .OrderBy(x => x)
                                        .ToList();
                    
                    Console.WriteLine("\nKet qua sap xep (chan giam dan, le tang dan):");
                    foreach (var item in evens)
                    {
                        Console.Write(item + " ");
                    }
        
                    foreach (var item in odds)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                }
    }
}