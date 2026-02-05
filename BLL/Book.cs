using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.Models
{
    public class Book
    {
        [Key]
        public string ISBN { get; set; }
        public string Title { get; set; }
        public decimal UnitPrice { get; set; }
        public int YearPublished { get; set; }
        public int QOH { get; set; }
        public int CategoryID { get; set; }
        public int PublisherID { get; set; }
        public int? AuthorID { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
