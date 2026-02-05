using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.DAL
{
    public static class UtilityDB
    {

        public static SqlConnection GetDBConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString;
            conn.Open();
            return conn;
        }
        public static SqlConnection ConnectDB()
        {
            SqlConnection conn = new SqlConnection(
                "Data Source=LAPTOP-5T84C0JJ\\SQLEXPRESS2022;Initial Catalog=HiTechDB;User=sa;Password=SMpp2004"
            );

            conn.Open();
            return conn;
        }
    }

}
