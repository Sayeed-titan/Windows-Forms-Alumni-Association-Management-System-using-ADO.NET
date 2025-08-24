using System;
using System.Data;
using System.Data.SqlClient;

namespace AlumniAssociationApp
{
    public static class DatabaseHelper
    {
        private static string connectionString = @"Server=.;Database=AlumniDB;Trusted_Connection=True;";

        public static DataTable GetData(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static void ExecuteCommand(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
