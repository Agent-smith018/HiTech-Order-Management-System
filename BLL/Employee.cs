using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_module_4.DAL;

namespace Project_module_4.BLL
{
    public class Employee
    {
        private int employeeId;
        private string firstName;
        private string lastName;
        private string jobTitle;
        private string email;


        public int EmployeeId { get { return employeeId; } set { employeeId = value; } }

        public string FirstName { get { return firstName; } set { firstName = value; } }

        public string LastName { get { return lastName; } set { lastName = value; } }

        public string JobTitle { get { return jobTitle; } set { jobTitle = value; } }

        public string Email { get { return email; } set { email = value; } }



        public bool SaveEmployee(Employee emp)  // ✅ Changed void → bool
        {
            return EmployeeDB.SaveDetails(emp);
        }

        public Employee SearchEmployee(int id)
        {
            return EmployeeDB.SearchRecord(id);
        }
        //public bool IsUniqueEmployeeId(int id)
        //{
        //    return EmployeeDB.IsUniqueId(id);
        //}
        public List<Employee> GetAllEmployees()
        {
            return EmployeeDB.GetAllRecords();
        }

        public void DeleteEmployee(int employeeId) => EmployeeDB.DeleteRecord(employeeId);


    }
}
