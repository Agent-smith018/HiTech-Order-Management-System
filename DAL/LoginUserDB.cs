using Project_module_4.BLL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.DAL
{
    public static class LoginUserDB
    {
        public static LoginUser ValidateLogin(string username, string password)
        {
            LoginUser user = null;

            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                //conn.Open(); // 🔥 FIX: Open the connection first

                string sql = @"SELECT u.Username, u.Password, e.JobTitle
                               FROM Users u
                               INNER JOIN Employees e ON u.EmployeeID = e.EmployeeID
                               WHERE u.Username = @Username AND u.Password = @Password";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new LoginUser
                            {
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                JobTitle = reader["JobTitle"].ToString()
                            };
                        }
                    }
                }
            }
            return user;
        }
    }
}

