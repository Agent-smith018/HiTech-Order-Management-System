using Project_module_4.BLL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_module_4.DAL
{
    public static class EmployeeDB
    {


        public static bool SaveDetails(Employee employee)
        {
            SqlConnection conn = UtilityDB.GetDBConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"INSERT INTO Employees (FirstName, LastName, JobTitle, Email) 
                       VALUES (@first, @last, @title, @email)";
            cmd.Parameters.AddWithValue("@first", employee.FirstName);
            cmd.Parameters.AddWithValue("@last", employee.LastName);
            cmd.Parameters.AddWithValue("@title", employee.JobTitle);
            cmd.Parameters.AddWithValue("@email", employee.Email);

            try
            {
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed: " + ex.Message, "Error");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }



        public static Employee SearchRecord(int id)
        {
            SqlConnection conn = UtilityDB.GetDBConnection();
            SqlCommand cmdSearchById = new SqlCommand("SELECT * FROM Employees WHERE EmployeeId=@EmployeeId", conn);
            cmdSearchById.Parameters.AddWithValue("@EmployeeId", id);

            try
            {
                SqlDataReader reader = cmdSearchById.ExecuteReader();
                if (reader.Read())
                {
                    Employee emp = new Employee();  // Create inside if
                                                    // ✅ CORRECT - Use generic indexing
                    emp.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                    emp.FirstName = reader["FirstName"].ToString();
                    emp.LastName = reader["LastName"].ToString();
                    emp.JobTitle = reader["JobTitle"].ToString();
                    emp.Email = reader["Email"].ToString();
                    reader.Close();
                    return emp;
                }
                reader.Close();
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        public static List<Employee> GetAllRecords()
        {
            List<Employee> listE = new List<Employee>();
            SqlConnection conn = UtilityDB.GetDBConnection();
            SqlCommand cmdSelectAll = new SqlCommand("SELECT * FROM Employees", conn);


            try
            {

                SqlDataReader reader = cmdSelectAll.ExecuteReader();
                while (reader.Read())
                {
                    Employee emp = new Employee();
                    emp.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                    emp.FirstName = reader["FirstName"].ToString();
                    emp.LastName = reader["LastName"].ToString();
                    emp.JobTitle = reader["JobTitle"].ToString();
                    emp.Email = reader["Email"].ToString();
                    listE.Add(emp);
                }
                reader.Close();
            }
            finally
            {
                conn.Close();
            }
            return listE;
        }


        public static bool IsUniqueId(int id)
        {
            Employee emp = SearchRecord(id);
            if (emp != null)
            {
                return false;
            }

            return true;
        }
        public static List<Employee> SearchRecord(string name)
        {
            List<Employee> listS = new List<Employee>();



            return listS;

        }

        //public static List<Employee> SearchRecord(string fname, string lname)
        //{
        //    List<Employee> listS = new List<Employee>();



        //    return listS;

        //}
        public static void DeleteRecord(int employeeId)
        {
            SqlConnection conn = UtilityDB.GetDBConnection();
            SqlCommand cmdDelete = new SqlCommand("DELETE FROM Employees WHERE EmployeeId = @EmployeeId", conn);
            cmdDelete.Parameters.AddWithValue("@EmployeeId", employeeId);

            try
            {
                int rows = cmdDelete.ExecuteNonQuery();
                if (rows == 0)
                    throw new Exception("No employee found with that ID");
            }
            catch (Exception ex)
            {
                throw new Exception("Delete failed: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
