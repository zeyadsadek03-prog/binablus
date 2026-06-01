using System;
using System.Data;
using BinaPlus.DAL;

namespace BinaPlus.BL
{
    public class KisiBL
    {
        private KisiDAL dal = new KisiDAL();

        public DataTable HepsiniGetir() => dal.HepsiniGetir();
        public DataTable Ara(string filtre) => dal.Bul(filtre);

        public void Ekle(string tc, string ad, string soyad, string tel, string mail, string rol)
        {
            if (string.IsNullOrWhiteSpace(tc) || tc.Length != 11) throw new Exception("TC No 11 haneli olmalıdır.");
            if (string.IsNullOrWhiteSpace(ad)) throw new Exception("Ad boş olamaz.");
            if (string.IsNullOrWhiteSpace(soyad)) throw new Exception("Soyad boş olamaz.");
            dal.Ekle(Guid.NewGuid().ToString(), tc, ad, soyad, tel, mail, rol);
        }

        public void Guncelle(string id, string tc, string ad, string soyad, string tel, string mail, string rol)
        {
            if (string.IsNullOrWhiteSpace(ad)) throw new Exception("Ad boş olamaz.");
            dal.Guncelle(id, tc, ad, soyad, tel, mail, rol);
        }

        public void Sil(string id) => dal.Sil(id);

        public float BakiyeGetir(string id) => dal.BakiyeGetir(id);
    }
}
