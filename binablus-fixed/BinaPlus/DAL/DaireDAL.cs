using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace BinaPlus.DAL
{
    public class DaireDAL
    {
        public DataTable HepsiniGetir()
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_DairelerHepsi");
        }

        public DataTable Bul(string filtre)
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_DaireBul", new[]
            {
                new MySqlParameter("filtre", filtre)
            });
        }

        public void Ekle(string id, string blok, string no, string tip, float m2, string durum)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_DaireEkle", new[]
            {
                new MySqlParameter("id", id),
                new MySqlParameter("bl", blok),
                new MySqlParameter("no", no),
                new MySqlParameter("tp", tip),
                new MySqlParameter("metre", m2),
                new MySqlParameter("drm", durum)
            });
        }

        public void Guncelle(string id, string blok, string no, string tip, float m2, string durum)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_DaireGuncelle", new[]
            {
                new MySqlParameter("id", id),
                new MySqlParameter("bl", blok),
                new MySqlParameter("no", no),
                new MySqlParameter("tp", tip),
                new MySqlParameter("metre", m2),
                new MySqlParameter("drm", durum)
            });
        }

        public void Sil(string id)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_DaireSil", new[]
            {
                new MySqlParameter("id", id)
            });
        }
    }
}
