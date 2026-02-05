using Project_module_4.Models;
using Project_Module_4.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Project_module_4.Services
{
    public class OrderRepository
    {
        public List<Order> GetAllOrders()
        {
            using (var ctx = new HiTechContext())
            {
                return ctx.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderDetails.Select(od => od.Book))
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();
            }
        }

        public Order GetOrderById(int id)
        {
            using (var ctx = new HiTechContext())
            {
                return ctx.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderDetails.Select(od => od.Book))
                    .FirstOrDefault(o => o.OrderID == id);
            }
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            using (var ctx = new HiTechContext())
            {
                return ctx.Orders
                    .Include(o => o.OrderDetails.Select(od => od.Book))
                    .Where(o => o.CustomerID == customerId)
                    .ToList();
            }
        }

        public Order CreateOrder(Order order)
        {
            using (var ctx = new HiTechContext())
            {
                ctx.Orders.Add(order);
                ctx.SaveChanges();
                return order;
            }
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            using (var ctx = new HiTechContext())
            {
                var order = ctx.Orders.Find(orderId);
                if (order == null) return;

                order.Status = status;
                ctx.SaveChanges();
            }
        }

        public void CancelOrder(int orderId)
        {
            using (var ctx = new HiTechContext())
            {
                var order = ctx.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefault(o => o.OrderID == orderId);

                if (order == null) return;

                ctx.OrderDetails.RemoveRange(order.OrderDetails);
                ctx.Orders.Remove(order);
                ctx.SaveChanges();
            }

        }
    }
}
