using System;
using System.Collections.Generic;
using System.Linq;
using WindowsAss.src.Miniproject.Models;

namespace WindowsAss.src.Miniproject.Services
{
    public class LibraryService
    {
        private readonly List<TaiLieu> _danhSachTaiLieu = new List<TaiLieu>();

        public bool MaTaiLieuExists(string maTaiLieu)
        {
            return _danhSachTaiLieu.Any(tl => tl.MaTaiLieu.Equals(maTaiLieu, StringComparison.OrdinalIgnoreCase));
        }

        public void AddDocument(TaiLieu taiLieu)
        {
            if (taiLieu == null)
            {
                throw new ArgumentNullException(nameof(taiLieu));
            }
            if (MaTaiLieuExists(taiLieu.MaTaiLieu))
            {
                throw new InvalidOperationException("Ma tai lieu da ton tai.");
            }
            _danhSachTaiLieu.Add(taiLieu);
        }

        public bool DeleteDocument(string maTaiLieu)
        {
            var taiLieu = _danhSachTaiLieu.FirstOrDefault(tl => tl.MaTaiLieu.Equals(maTaiLieu, StringComparison.OrdinalIgnoreCase));
            if (taiLieu != null)
            {
                _danhSachTaiLieu.Remove(taiLieu);
                return true;
            }
            return false;
        }

        public List<TaiLieu> GetAllDocuments()
        {
            return new List<TaiLieu>(_danhSachTaiLieu);
        }

        public List<T> FindDocumentsByType<T>() where T : TaiLieu
        {
            return _danhSachTaiLieu.OfType<T>().ToList();
        }
    }
}
