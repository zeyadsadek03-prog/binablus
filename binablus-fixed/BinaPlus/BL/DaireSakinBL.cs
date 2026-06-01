using System;
using System.Data;
using BinaPlus.DAL;

namespace BinaPlus.BL
{
    public class DaireSakinBL
    {
        private DaireSakinDAL dal = new DaireSakinDAL();

        public DataTable HepsiniGetir() => dal.HepsiniGetir();

        public void Ekle(string kisiId, string daireId, string rol, DateTime baslangic)
        {
            if (string.IsNullOrWhiteSpace(kisiId)) throw new Exception("Kişi seçilmeli.");
            if (string.IsNullOrWhiteSpace(daireId)) throw new Exception("Daire seçilmeli.");
            dal.Ekle(kisiId, daireId, rol, baslangic);
        }

        public void Guncelle(string kisiId, string daireId, string rol, DateTime baslangic)
        {
            dal.Guncelle(kisiId, daireId, rol, baslangic);
        }

        public void Sil(string kisiId, string daireId) => dal.Sil(kisiId, daireId);
    }
}
