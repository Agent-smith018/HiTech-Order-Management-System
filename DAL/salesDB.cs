using Project_module_4.BLL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_module_4.DAL
{
    public static class salesDB
    {
        //public static List<Sales> GetAllRecords()
        //{
        //    List<Sales> listS = new List<Sales>();
        //    using (SqlConnection conn = UtilityDB.GetDBConnection())
        //    {
        //        SqlCommand cmdSelectAll = new SqlCommand("SELECT * FROM customers", conn);
        //        SqlDataReader dr = cmdSelectAll.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            Sales sale = new Sales
        //            {
        //                Customer_ID = Convert.ToInt32(dr["customer_id"]),
        //                FirstName = dr["first_name"].ToString(),
        //                LastName = dr["last_name"].ToString(),
        //                Email = dr["email"].ToString(),
        //                Phone = dr["phone"].ToString(),
        //                City = dr["city"].ToString()

        //            };
        //            listS.Add(sale);
        //        }

        //        return listS;
        //    }

        public static class CustomerDB
        {
            public static List<Sales> GetAllRecords()
            {
                List<Sales> listC = new List<Sales>();
                using (SqlConnection conn = UtilityDB.GetDBConnection())
                {
                    SqlCommand cmdSelectAll = new SqlCommand("SELECT * FROM customers", conn);
                    SqlDataReader dr = cmdSelectAll.ExecuteReader();
                    while (dr.Read())
                    {
                        Sales sale = new Sales
                        {
                            CustomerID = Convert.ToInt32(dr["CustomerID"]),
                            Name = dr["name"].ToString(),
                            Email = dr["email"].ToString(),
                            Phone = (dr["phone"]).ToString(),
                            City = dr["city"].ToString(),
                            Street = dr["street"].ToString(),
                            PostalCode = dr["PostalCode"].ToString()
                        };
                        listC.Add(sale);
                    }
                }
                return listC;
            }

            public static Sales SearchRecord(int customerId)
            {
                Sales foundCustomer = null;
                using (SqlConnection conn = UtilityDB.GetDBConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM customers WHERE CustomerID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", customerId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        foundCustomer = new Sales
                        {
                            CustomerID = (int)reader["CustomerID"],
                            Name = reader["name"].ToString(),
                            Email = reader["email"].ToString(),
                            Phone = reader["phone"].ToString(),
                            City = reader["city"].ToString(),
                            Street = reader["street"].ToString(),
                            PostalCode = reader["PostalCode"].ToString()
                        };
                    }
                }
                return foundCustomer;
            }

        }
    }
}