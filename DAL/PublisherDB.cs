using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_module_4.BLL;

namespace Project_module_4.DAL
{
    public static class PublisherDB
    {
        public static List<Publisher> GetAll()
        {
            List<Publisher> list = new List<Publisher>();

            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                string sql = "SELECT * FROM Publishers";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Publisher
                    {
                        PublisherID = (int)reader["PublisherID"],
                        PublisherName = reader["PublisherName"].ToString()
                    });
                }
            }

            return list;
        }
        public static int AddIfNotExists(string name)
        {
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                string sql = @"
IF EXISTS (SELECT 1 FROM Publishers WHERE PublisherName = @Name)
    SELECT PublisherID FROM Publishers WHERE PublisherName = @Name;
ELSE
BEGIN
    INSERT INTO Publishers(PublisherName) VALUES(@Name);
    SELECT SCOPE_IDENTITY();
END";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

    }
}
