using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }

        // FK
        public int OrderID { get; set; }
        public string ISBN { get; set; }

        // Navigation
        public Order Order { get; set; }
        public Book Book { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
