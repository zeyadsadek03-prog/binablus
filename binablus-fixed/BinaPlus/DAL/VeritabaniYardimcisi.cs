using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace BinaPlus.DAL
{
    public static class VeritabaniYardimcisi
    {
        private static string connectionString =
            "Server=localhost;Database=binaplus;Uid=root;Pwd=clannishline890;CharSet=utf8;";

        public static string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public static MySqlConnection BaglantiAl()
        {
            return new MySqlConnection(connectionString);
        }

        public static DataTable ProcedureListeAl(string procedureName, MySqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection con = BaglantiAl())
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(procedureName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            return dt;
        }

        public static void ProcedureKomutCalistir(string procedureName, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection con = BaglantiAl())
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(procedureName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ProcedureSkalarAl(string procedureName, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection con = BaglantiAl())
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(procedureName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
