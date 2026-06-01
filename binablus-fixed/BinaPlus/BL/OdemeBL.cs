using System;
using System.Data;
using BinaPlus.DAL;

namespace BinaPlus.BL
{
    public class OdemeBL
    {
        private OdemeDAL dal = new OdemeDAL();

        public DataTable HepsiniGetir() => dal.HepsiniGetir();
        public DataTable Ara(string filtre) => dal.Bul(filtre);

        public void Ekle(string kisiId, string aidatId, DateTime tarih, float tutar, string tur, string aciklama)
        {
            if (string.IsNullOrWhiteSpace(kisiId)) throw new Exception("Kişi seçilmeli.");
            if (string.IsNullOrWhiteSpace(aidatId)) throw new Exception("Aidat seçilmeli.");
            if (tutar <= 0) throw new Exception("Tutar 0'dan büyük olmalı.");
            dal.Ekle(Guid.NewGuid().ToString(), kisiId, aidatId, tarih, tutar, tur, aciklama);
        }

        public void Guncelle(string id, string kisiId, string aidatId, DateTime tarih, float tutar, string tur, string aciklama)
        {
            dal.Guncelle(id, kisiId, aidatId, tarih, tutar, tur, aciklama);
        }

        public void Sil(string id) => dal.Sil(id);

        public float ToplamAl() => dal.ToplamAl();
    }
}
