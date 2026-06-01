using System;
using System.Data;
using BinaPlus.DAL;

namespace BinaPlus.BL
{
    public class AidatBL
    {
        private AidatDAL dal = new AidatDAL();

        public DataTable HepsiniGetir() => dal.HepsiniGetir();
        public DataTable Ara(string filtre) => dal.Bul(filtre);

        public void Ekle(string daireId, int ay, int yil, float tutar, DateTime son, string durum)
        {
            if (string.IsNullOrWhiteSpace(daireId)) throw new Exception("Daire seçilmeli.");
            if (tutar <= 0) throw new Exception("Tutar 0'dan büyük olmalı.");
            dal.Ekle(Guid.NewGuid().ToString(), daireId, ay, yil, tutar, son, durum);
        }

        public void Guncelle(string id, string daireId, int ay, int yil, float tutar, DateTime son, string durum)
        {
            dal.Guncelle(id, daireId, ay, yil, tutar, son, durum);
        }

        public void Sil(string id) => dal.Sil(id);

        public float ToplamAl() => dal.ToplamAl();
    }
}
