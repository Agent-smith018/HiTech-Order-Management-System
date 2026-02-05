using Project_module_4.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.DAL
{
    public static class BookDB
    {
        // ADD
        public static void Add(Book book)
        {
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                string sql = @"INSERT INTO Books
                      (ISBN, Title, UnitPrice, YearPublished, QOH, CategoryID, PublisherID, AuthorID) 
                      VALUES(@ISBN, @Title, @UnitPrice, @YearPublished, @QOH, @CategoryID, @PublisherID, @AuthorID)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@UnitPrice", book.UnitPrice);
                cmd.Parameters.AddWithValue("@YearPublished", book.YearPublished);
                cmd.Parameters.AddWithValue("@QOH", book.QOH);
                cmd.Parameters.AddWithValue("@CategoryID", book.CategoryID);
                cmd.Parameters.AddWithValue("@PublisherID", book.PublisherID);
                cmd.Parameters.AddWithValue("@AuthorID", book.AuthorID ?? 1); // Default or from form

                cmd.ExecuteNonQuery();
            }
        }

        // UPDATE
        public static void Update(Book book)
        {
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                string sql = @"UPDATE Books
                               SET Title=@Title, UnitPrice=@UnitPrice, YearPublished=@YearPublished,
                                   QOH=@QOH, CategoryID=@CategoryID, PublisherID=@PublisherID
                               WHERE ISBN=@ISBN";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@UnitPrice", book.UnitPrice);
                cmd.Parameters.AddWithValue("@YearPublished", book.YearPublished);
                cmd.Parameters.AddWithValue("@QOH", book.QOH);
                cmd.Parameters.AddWithValue("@CategoryID", book.CategoryID);
                cmd.Parameters.AddWithValue("@PublisherID", book.PublisherID);

                cmd.ExecuteNonQuery();
            }
        }

        // DELETE
        public static void Delete(string isbn)
        {
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {


                // 1) Delete from junction table BookAuthors
                using (SqlCommand cmd1 = new SqlCommand(
                    "DELETE FROM BookAuthors WHERE ISBN = @ISBN", conn))
                {
                    cmd1.Parameters.AddWithValue("@ISBN", isbn);
                    cmd1.ExecuteNonQuery();
                }

                // 2) Delete from Books
                using (SqlCommand cmd2 = new SqlCommand(
                    "DELETE FROM Books WHERE ISBN = @ISBN", conn))
                {
                    cmd2.Parameters.AddWithValue("@ISBN", isbn);
                    cmd2.ExecuteNonQuery();
                }

            }

        }


        // SEARCH
        public static Book Search(string isbn)
        {
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                string sql = @"
            SELECT TOP 1 
                   b.ISBN,
                   b.Title,
                   b.UnitPrice,
                   b.YearPublished,
                   b.QOH,
                   b.CategoryID,
                   b.PublisherID,
                   ba.AuthorID,
                   a.FirstName,
                   a.LastName
            FROM Books b
            LEFT JOIN BookAuthors ba ON b.ISBN = ba.ISBN
            LEFT JOIN Authors a ON ba.AuthorID = a.AuthorID
            WHERE b.ISBN = @ISBN";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ISBN", isbn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Book
                    {
                        ISBN = reader["ISBN"].ToString(),
                        Title = reader["Title"].ToString(),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        YearPublished = Convert.ToInt32(reader["YearPublished"]),
                        QOH = Convert.ToInt32(reader["QOH"]),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        PublisherID = Convert.ToInt32(reader["PublisherID"]),
                        AuthorID = reader["AuthorID"] == DBNull.Value
                                   ? (int?)null
                                   : Convert.ToInt32(reader["AuthorID"])
                    };
                }
            }
            return null;
        }


        // GET ALL
        public static List<Book> GetAll()
        {
            List<Book> list = new List<Book>();

            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                string sql = "SELECT * FROM Books";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Book
                    {
                        ISBN = reader["ISBN"].ToString(),
                        Title = reader["Title"].ToString(),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        YearPublished = Convert.ToInt32(reader["YearPublished"]),
                        QOH = Convert.ToInt32(reader["QOH"]),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        PublisherID = Convert.ToInt32(reader["PublisherID"])
                    });
                }
            }

            return list;
        }
        public static DataTable GetAllWithNamesAndAuthors()
        {
            using (SqlConnection conn = UtilityDB.ConnectDB())
            {
                string sql = @"
            SELECT  b.ISBN,
                    b.Title,
                    b.UnitPrice,
                    b.YearPublished,
                    b.QOH,
                    c.CategoryName,
                    p.PublisherName,
                    STRING_AGG(a.FirstName + ' ' + a.LastName, ', ') AS Authors
            FROM    Books b
            INNER JOIN Categories c ON b.CategoryID = c.CategoryID
            INNER JOIN Publishers p ON b.PublisherID = p.PublisherID
            LEFT JOIN BookAuthors ba ON b.ISBN = ba.ISBN
            LEFT JOIN Authors a ON ba.AuthorID = a.AuthorID
            GROUP BY b.ISBN, b.Title, b.UnitPrice, b.YearPublished, b.QOH,
                     c.CategoryName, p.PublisherName;
        ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

    }
}
