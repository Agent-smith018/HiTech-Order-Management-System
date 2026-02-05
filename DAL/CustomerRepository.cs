using Project_module_4.Models;
using Project_Module_4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.Services
{
    public class CustomerRepository
    {
        public List<Customer> GetAllCustomers()
        {
            using (var ctx = new HiTechContext())
            {
                return ctx.Customers.ToList();
            }
        }
    }
}
