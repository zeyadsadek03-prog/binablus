using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace BinaPlus.DAL
{
    public class BakimTalepDAL
    {
        public DataTable HepsiniGetir()
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_BakimHepsi");
        }

        public DataTable Bul(string filtre)
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_BakimBul", new[]
            {
                new MySqlParameter("filtre", filtre)
            });
        }

        public void Ekle(string id, string daireId, string kategori, string aciklama, string durum, DateTime tarih, DateTime? cozum)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_BakimEkle", new[]
            {
                new MySqlParameter("id", id),
                new MySqlParameter("did", daireId),
                new MySqlParameter("kat", kategori),
                new MySqlParameter("ack", aciklama),
                new MySqlParameter("drm", durum),
                new MySqlParameter("trh", tarih),
                new MySqlParameter("coz", cozum.HasValue ? (object)cozum.Value : DBNull.Value)
            });
        }

        public void Guncelle(string id, string daireId, string kategori, string aciklama, string durum, DateTime tarih, DateTime? cozum)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_BakimGuncelle", new[]
            {
                new MySqlParameter("id", id),
                new MySqlParameter("did", daireId),
                new MySqlParameter("kat", kategori),
                new MySqlParameter("ack", aciklama),
                new MySqlParameter("drm", durum),
                new MySqlParameter("trh", tarih),
                new MySqlParameter("coz", cozum.HasValue ? (object)cozum.Value : DBNull.Value)
            });
        }

        public void Sil(string id)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_BakimSil", new[]
            {
                new MySqlParameter("id", id)
            });
        }
    }
}
