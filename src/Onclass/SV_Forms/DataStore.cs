using System;
using System.Collections.Generic;

namespace WindowsAss.src.Onclass.SV_Forms
{
    /// <summary>Lớp mô tả Khoa.</summary>
    public class Khoa
    {
        public string MaKhoa { get; set; } = "";
        public string TenKhoa { get; set; } = "";
        public override string ToString() => $"{MaKhoa} - {TenKhoa}";
    }

    /// <summary>Lớp mô tả Sinh viên.</summary>
    public class SinhVien
    {
        public string MaSV { get; set; } = "";
        public string HoTen { get; set; } = "";
        public DateTime NgaySinh { get; set; }
        public string NganhHoc { get; set; } = "";
        public string MaKhoa { get; set; } = "";
        public string SoDienThoai { get; set; } = "";
        public override string ToString() => $"{MaSV} - {HoTen}";
    }

    /// <summary>Lớp mô tả Môn học.</summary>
    public class MonHoc
    {
        public string MaMon { get; set; } = "";
        public string TenMon { get; set; } = "";
        public int SoTinChi { get; set; }
        public override string ToString() => $"{MaMon} - {TenMon}";
    }

    /// <summary>Lớp mô tả Điểm (SV + Môn + Điểm số).</summary>
    public class Diem
    {
        public string MaSV { get; set; } = "";
        public string MaMon { get; set; } = "";
        public double DiemSo { get; set; }
    }

    /// <summary>Kho dữ liệu dùng chung trong bộ form MDI (in-memory).</summary>
    public static class DataStore
    {
        public static List<Khoa> Khoas { get; } = new List<Khoa>();
        public static List<SinhVien> SinhViens { get; } = new List<SinhVien>();
        public static List<MonHoc> MonHocs { get; } = new List<MonHoc>();
        public static List<Diem> Diems { get; } = new List<Diem>();
    }
}
