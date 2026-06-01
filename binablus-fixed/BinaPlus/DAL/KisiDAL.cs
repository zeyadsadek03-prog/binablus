using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace BinaPlus.DAL
{
    public class KisiDAL
    {
        public DataTable HepsiniGetir()
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_KisilerHepsi");
        }

        public DataTable Bul(string filtre)
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_KisiBul", new[]
            {
                new MySqlParameter("filtre", filtre)
            });
        }

        public void Ekle(string id, string tc, string ad, string soyad, string tel, string mail, string rol)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_KisiEkle", new[]
            {
                new MySqlParameter("id", id),
                new MySqlParameter("tc", tc),
                new MySqlParameter("adi", ad),
                new MySqlParameter("soy", soyad),
                new MySqlParameter("tel", tel),
                new MySqlParameter("ml", mail),
                new MySqlParameter("rl", rol)
            });
        }

        public void Guncelle(string id, string tc, string ad, string soyad, string tel, string mail, string rol)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_KisiGuncelle", new[]
            {
                new MySqlParameter("id", id),
                new MySqlParameter("tc", tc),
                new MySqlParameter("adi", ad),
                new MySqlParameter("soy", soyad),
                new MySqlParameter("tel", tel),
                new MySqlParameter("ml", mail),
                new MySqlParameter("rl", rol)
            });
        }

        public void Sil(string id)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_KisiSil", new[]
            {
                new MySqlParameter("id", id)
            });
        }

        public float BakiyeGetir(string id)
        {
            var dt = VeritabaniYardimcisi.ProcedureListeAl("bp_KisiBakiye", new[]
            {
                new MySqlParameter("id", id)
            });
            if (dt.Rows.Count > 0)
                return Convert.ToSingle(dt.Rows[0][0]);
            return 0;
        }
    }
}
