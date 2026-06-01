using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace BinaPlus.DAL
{
    public class AidatDAL
    {
        public DataTable HepsiniGetir()
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_AidatlarHepsi");
        }

        public DataTable Bul(string filtre)
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_AidatBul", new[]
            {
                new MySqlParameter("filtre", filtre)
            });
        }

        public void Ekle(string id, string daireId, int ay, int yil, float tutar, DateTime son, string durum)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_AidatEkle", new[]
            {
                new MySqlParameter("id", id),
                new MySqlParameter("did", daireId),
                new MySqlParameter("ayy", ay),
                new MySqlParameter("yl", yil),
                new MySqlParameter("ttr", tutar),
                new MySqlParameter("son", son.Date),
                new MySqlParameter("drm", durum)
            });
        }

        public void Guncelle(string id, string daireId, int ay, int yil, float tutar, DateTime son, string durum)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_AidatGuncelle", new[]
            {
                new MySqlParameter("id", id),
                new MySqlParameter("did", daireId),
                new MySqlParameter("ayy", ay),
                new MySqlParameter("yl", yil),
                new MySqlParameter("ttr", tutar),
                new MySqlParameter("son", son.Date),
                new MySqlParameter("drm", durum)
            });
        }

        public void Sil(string id)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_AidatSil", new[]
            {
                new MySqlParameter("id", id)
            });
        }

        public float ToplamAl()
        {
            var dt = VeritabaniYardimcisi.ProcedureListeAl("bp_AidatToplam");
            if (dt.Rows.Count > 0)
                return Convert.ToSingle(dt.Rows[0][0]);
            return 0;
        }
    }
}
