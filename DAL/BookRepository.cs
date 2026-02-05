using Project_module_4.Models;
using Project_Module_4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.Services
{
    public class BookRepository
    {
        public List<Book> GetAllBooks()
        {
            using (var ctx = new HiTechContext())
            {
                return ctx.Books.ToList();
            }
        }

        public Book GetBookByISBN(string isbn)
        {
            using (var ctx = new HiTechContext())
            {
                return ctx.Books.Find(isbn);
            }
        }
    }
}