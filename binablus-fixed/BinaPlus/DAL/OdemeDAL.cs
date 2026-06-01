using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace BinaPlus.DAL
{
    public class OdemeDAL
    {
        public DataTable HepsiniGetir()
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_OdemelerHepsi");
        }

        public DataTable Bul(string filtre)
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_OdemeBul", new[]
            {
                new MySqlParameter("filtre", filtre)
            });
        }

        public void Ekle(string id, string kisiId, string aidatId, DateTime tarih, float tutar, string tur, string aciklama)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_OdemeEkle", new[]
            {
                new MySqlParameter("id", id),
                new MySqlParameter("kid", kisiId),
                new MySqlParameter("aid", aidatId),
                new MySqlParameter("trh", tarih),
                new MySqlParameter("ttr", tutar),
                new MySqlParameter("tr", tur),
                new MySqlParameter("ack", aciklama)
            });
        }

        public void Guncelle(string id, string kisiId, string aidatId, DateTime tarih, float tutar, string tur, string aciklama)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_OdemeGuncelle", new[]
            {
                new MySqlParameter("id", id),
                new MySqlParameter("kid", kisiId),
                new MySqlParameter("aid", aidatId),
                new MySqlParameter("trh", tarih),
                new MySqlParameter("ttr", tutar),
                new MySqlParameter("tr", tur),
                new MySqlParameter("ack", aciklama)
            });
        }

        public void Sil(string id)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_OdemeSil", new[]
            {
                new MySqlParameter("id", id)
            });
        }

        public float ToplamAl()
        {
            var dt = VeritabaniYardimcisi.ProcedureListeAl("bp_OdemeToplam");
            if (dt.Rows.Count > 0)
                return Convert.ToSingle(dt.Rows[0][0]);
            return 0;
        }
    }
}
