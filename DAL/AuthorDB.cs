using Project_module_4.BLL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.DAL
{
    public static class AuthorDB
    {
        public static List<Author> GetAll()
        {
            List<Author> list = new List<Author>();
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                string sql = "SELECT AuthorID, FirstName, LastName, Email FROM Authors";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Author
                    {
                        AuthorID = Convert.ToInt32(reader["AuthorID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }
            return list;
        }
        public static int AddIfNotExists(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Author name cannot be empty.");

            // Split into First + Last safely
            var parts = fullName.Trim()
                                .Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                throw new ArgumentException("Author name cannot be empty.");

            string first = parts[0];
            string last = parts.Length > 1 ? parts[1] : "-";

            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                // 1) Try to find existing author
                string findSql = @"SELECT AuthorID 
                           FROM Authors 
                           WHERE FirstName = @First AND LastName = @Last";

                using (SqlCommand findCmd = new SqlCommand(findSql, conn))
                {
                    findCmd.Parameters.AddWithValue("@First", first);
                    findCmd.Parameters.AddWithValue("@Last", last);

                    object existing = findCmd.ExecuteScalar();
                    if (existing != null)
                        return Convert.ToInt32(existing);
                }

                // 2) Insert new author with UNIQUE fake email
                string email = $"{first}.{last}.{Guid.NewGuid():N}@noemail.local";

                string insertSql = @"INSERT INTO Authors (FirstName, LastName, Email)
                             VALUES (@First, @Last, @Email);
                             SELECT SCOPE_IDENTITY();";

                using (SqlCommand insertCmd = new SqlCommand(insertSql, conn))
                {
                    insertCmd.Parameters.AddWithValue("@First", first);
                    insertCmd.Parameters.AddWithValue("@Last", last);
                    insertCmd.Parameters.AddWithValue("@Email", email);

                    return Convert.ToInt32(insertCmd.ExecuteScalar());
                }
            }
        }



    }
}
