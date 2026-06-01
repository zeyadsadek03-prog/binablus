using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace BinaPlus.DAL
{
    public class DaireSakinDAL
    {
        public DataTable HepsiniGetir()
        {
            return VeritabaniYardimcisi.ProcedureListeAl("bp_SakinlerHepsi");
        }

        public void Ekle(string kisiId, string daireId, string rol, DateTime baslangic)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_SakinEkle", new[]
            {
                new MySqlParameter("kid", kisiId),
                new MySqlParameter("did", daireId),
                new MySqlParameter("rl", rol),
                new MySqlParameter("bas", baslangic.Date)
            });
        }

        public void Guncelle(string kisiId, string daireId, string rol, DateTime baslangic)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_SakinGuncelle", new[]
            {
                new MySqlParameter("kid", kisiId),
                new MySqlParameter("did", daireId),
                new MySqlParameter("rl", rol),
                new MySqlParameter("bas", baslangic.Date)
            });
        }

        public void Sil(string kisiId, string daireId)
        {
            VeritabaniYardimcisi.ProcedureKomutCalistir("bp_SakinSil", new[]
            {
                new MySqlParameter("kid", kisiId),
                new MySqlParameter("did", daireId)
            });
        }
    }
}
