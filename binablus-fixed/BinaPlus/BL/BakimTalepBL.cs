using System;
using System.Data;
using BinaPlus.DAL;

namespace BinaPlus.BL
{
    public class BakimTalepBL
    {
        private BakimTalepDAL dal = new BakimTalepDAL();

        public DataTable HepsiniGetir() => dal.HepsiniGetir();
        public DataTable Ara(string filtre) => dal.Bul(filtre);

        public void Ekle(string daireId, string kategori, string aciklama, string durum, DateTime tarih, DateTime? cozum)
        {
            if (string.IsNullOrWhiteSpace(daireId)) throw new Exception("Daire seçilmeli.");
            if (string.IsNullOrWhiteSpace(kategori)) throw new Exception("Kategori boş olamaz.");
            dal.Ekle(Guid.NewGuid().ToString(), daireId, kategori, aciklama, durum, tarih, cozum);
        }

        public void Guncelle(string id, string daireId, string kategori, string aciklama, string durum, DateTime tarih, DateTime? cozum)
        {
            dal.Guncelle(id, daireId, kategori, aciklama, durum, tarih, cozum);
        }

        public void Sil(string id) => dal.Sil(id);
    }
}
