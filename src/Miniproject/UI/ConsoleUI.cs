using System;
using System.Collections.Generic;
using System.Globalization;
using WindowsAss.src.Miniproject.Models;
using WindowsAss.src.Miniproject.Services;

namespace WindowsAss.src.Miniproject.UI
{
    public class ConsoleUI
    {
        private readonly LibraryService _libraryService = new LibraryService();

        #region Input Helpers
        private string NhapChuoi(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("=> Gia tri khong duoc de trong.");
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
                Console.WriteLine($"=> Gia tri khong hop le. Vui long nhap mot so nguyen tu {min} den {max}.");
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
                Console.WriteLine("=> Dinh dang ngay khong hop le. Vui long nhap theo dinh dang dd/MM/yyyy.");
            }
        }
        #endregion

        #region Main Feature Handlers
        private void ThemMoiTaiLieu()
        {
            Console.WriteLine("\n[1] Them Sach\n[2] Them Tap Chi\n[3] Them Bao");
            string choice = NhapChuoi("Chon loai tai lieu: ");

            string maTaiLieu;
            while (true)
            {
                maTaiLieu = NhapChuoi("Ma tai lieu: ");
                if (!_libraryService.MaTaiLieuExists(maTaiLieu))
                {
                    break;
                }
                Console.WriteLine("=> Ma tai lieu da ton tai. Vui long nhap ma khac.");
            }

            string tenNXB = NhapChuoi("Ten nha xuat ban: ");
            int soBanPhatHanh = NhapSoNguyen("So ban phat hanh: ", 1);

            TaiLieu? newDocument = null;
            switch (choice)
            {
                case "1":
                    string tenTacGia = NhapChuoi("Ten tac gia: ");
                    int soTrang = NhapSoNguyen("So trang: ", 1);
                    newDocument = new Sach(maTaiLieu, tenNXB, soBanPhatHanh, tenTacGia, soTrang);
                    break;
                case "2":
                    int soPhatHanh = NhapSoNguyen("So phat hanh: ", 1);
                    int thangPhatHanh = NhapSoNguyen("Thang phat hanh: ", 1, 12);
                    newDocument = new TapChi(maTaiLieu, tenNXB, soBanPhatHanh, soPhatHanh, thangPhatHanh);
                    break;
                case "3":
                    DateTime ngayPhatHanh = NhapNgay("Ngay phat hanh (dd/MM/yyyy): ");
                    newDocument = new Bao(maTaiLieu, tenNXB, soBanPhatHanh, ngayPhatHanh);
                    break;
                default:
                    Console.WriteLine("=> Lua chon khong hop le.");
                    return;
            }

            if (newDocument != null)
            {
                _libraryService.AddDocument(newDocument);
                Console.WriteLine("=> Them moi tai lieu thanh cong.");
            }
        }

        private void XoaTaiLieu()
        {
            var allDocuments = _libraryService.GetAllDocuments();
            if (allDocuments.Count == 0)
            {
                Console.WriteLine("=> Thu vien chua co tai lieu nao de xoa.");
                return;
            }
            string maTaiLieu = NhapChuoi("Nhap ma tai lieu can xoa: ");
            if (_libraryService.DeleteDocument(maTaiLieu))
            {
                Console.WriteLine("=> Xoa tai lieu thanh cong.");
            }
            else
            {
                Console.WriteLine("=> Khong tim thay tai lieu voi ma da cho.");
            }
        }

        private void HienThiThongTin()
        {
            var allDocuments = _libraryService.GetAllDocuments();
            if (allDocuments.Count == 0)
            {
                Console.WriteLine("=> Thu vien chua co tai lieu nao.");
                return;
            }
            Console.WriteLine("\n--- Danh sach tat ca tai lieu ---");
            foreach (var taiLieu in allDocuments)
            {
                Console.WriteLine(taiLieu.ToString());
                Console.WriteLine();
            }
        }

        private void TimKiemTheoLoai()
        {
            var allDocuments = _libraryService.GetAllDocuments();
            if (allDocuments.Count == 0)
            {
                Console.WriteLine("=> Thu vien chua co tai lieu nao de tim kiem.");
                return;
            }
            Console.WriteLine("\n[1] Tim Sach\n[2] Tim Tap Chi\n[3] Tim Bao");
            string choice = NhapChuoi("Chon loai tai lieu: ");

            switch (choice)
            {
                case "1":
                    DisplaySearchResults(_libraryService.FindDocumentsByType<Sach>(), "Sach");
                    break;
                case "2":
                    DisplaySearchResults(_libraryService.FindDocumentsByType<TapChi>(), "Tap Chi");
                    break;
                case "3":
                    DisplaySearchResults(_libraryService.FindDocumentsByType<Bao>(), "Bao");
                    break;
                default:
                    Console.WriteLine("=> Lua chon khong hop le.");
                    break;
            }
        }

        private void DisplaySearchResults<T>(List<T> results, string typeName) where T : TaiLieu
        {
            if (results.Count == 0)
            {
                Console.WriteLine($"=> Khong co {typeName} nao trong thu vien.");
            }
            else
            {
                Console.WriteLine($"\n--- Ket qua tim kiem: {results.Count} {typeName} ---");
                foreach (var taiLieu in results)
                {
                    Console.WriteLine(taiLieu.ToString());
                    Console.WriteLine();
                }
            }
        }
        #endregion

        public static void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleUI ui = new ConsoleUI();
            while (true)
            {
                Console.WriteLine("\n========== HE THONG QUAN LY THU VIEN (v2) ==========");
                Console.WriteLine(" [1] Them moi tai lieu");
                Console.WriteLine(" [2] Xoa tai lieu theo ma");
                Console.WriteLine(" [3] Hien thi thong tin tai lieu");
                Console.WriteLine(" [4] Tim kiem theo loai");
                Console.WriteLine(" [0] Quay lai menu chinh");
                Console.WriteLine("=====================================================");
                
                string choice = ui.NhapChuoi("Lua chon cua ban: ");

                switch (choice)
                {
                    case "1":
                        ui.ThemMoiTaiLieu();
                        break;
                    case "2":
                        ui.XoaTaiLieu();
                        break;
                    case "3":
                        ui.HienThiThongTin();
                        break;
                    case "4":
                        ui.TimKiemTheoLoai();
                        break;
                    case "0":
                        Console.WriteLine("Da quay lai menu chinh.");
                        return;
                    default:
                        Console.WriteLine("=> Lua chon khong hop le. Vui long chon lai.");
                        break;
                }
            }
        }
    }
}
