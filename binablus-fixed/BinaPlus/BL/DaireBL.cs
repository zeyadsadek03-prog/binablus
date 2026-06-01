using System;
using System.Data;
using BinaPlus.DAL;

namespace BinaPlus.BL
{
    public class DaireBL
    {
        private DaireDAL dal = new DaireDAL();

        public DataTable HepsiniGetir() => dal.HepsiniGetir();
        public DataTable Ara(string filtre) => dal.Bul(filtre);

        public void Ekle(string blok, string no, string tip, float m2, string durum)
        {
            if (string.IsNullOrWhiteSpace(blok)) throw new Exception("Blok boş olamaz.");
            if (string.IsNullOrWhiteSpace(no)) throw new Exception("Daire No boş olamaz.");
            dal.Ekle(Guid.NewGuid().ToString(), blok, no, tip, m2, durum);
        }

        public void Guncelle(string id, string blok, string no, string tip, float m2, string durum)
        {
            if (string.IsNullOrWhiteSpace(blok)) throw new Exception("Blok boş olamaz.");
            if (string.IsNullOrWhiteSpace(no)) throw new Exception("Daire No boş olamaz.");
            dal.Guncelle(id, blok, no, tip, m2, durum);
        }

        public void Sil(string id) => dal.Sil(id);
    }
}
