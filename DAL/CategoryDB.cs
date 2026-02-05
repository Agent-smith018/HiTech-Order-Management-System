using Project_module_4.BLL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.DAL
{
    public static class CategoryDB
    {
        public static List<Category> GetAll()
        {
            List<Category> list = new List<Category>();

            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                string sql = "SELECT * FROM Categories";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Category
                    {
                        CategoryID = (int)reader["CategoryID"],
                        CategoryName = reader["CategoryName"].ToString()
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
IF EXISTS (SELECT 1 FROM Categories WHERE CategoryName = @Name)
    SELECT CategoryID FROM Categories WHERE CategoryName = @Name;
ELSE
BEGIN
    INSERT INTO Categories(CategoryName) VALUES(@Name);
    SELECT SCOPE_IDENTITY();
END";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

    }
}
