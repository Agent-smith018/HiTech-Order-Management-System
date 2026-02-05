using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.DAL
{
    public static class BookAuthorDB
    {
        public static void AddBookAuthor(string isbn, int authorID)
        {
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                string sql = "INSERT INTO BookAuthors (ISBN, AuthorID) VALUES (@ISBN, @AuthorID)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ISBN", isbn);
                cmd.Parameters.AddWithValue("@AuthorID", authorID);
                cmd.ExecuteNonQuery();
            }
        }
        public static void UpdateBookAuthor(string isbn, int authorID)
        {
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {


                // If there is already a row for this ISBN, update it; otherwise insert.
                string sql = @"
IF EXISTS (SELECT 1 FROM BookAuthors WHERE ISBN = @ISBN)
    UPDATE BookAuthors SET AuthorID = @AuthorID WHERE ISBN = @ISBN;
ELSE
    INSERT INTO BookAuthors (ISBN, AuthorID) VALUES (@ISBN, @AuthorID);";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    cmd.Parameters.AddWithValue("@AuthorID", authorID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

