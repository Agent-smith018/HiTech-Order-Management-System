using Project_module_4.BLL;
using Project_module_4.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Project_module_4.DAL.salesDB;

namespace Project_module_4.GUI
{
    public partial class SalesForm : Form
    {
        SqlDataAdapter da;
        DataSet dsSalesDB;
        DataTable dtCustomers;
        SqlCommandBuilder cmdBuilder;
        Sales sale = new Sales();
        public SalesForm()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            DataRow dr = dtCustomers.NewRow();
            dr["Name"] = textBoxFirstName.Text.Trim();
            dr["Email"] = textBoxEmail.Text.Trim();
            dr["Phone"] = textBoxPhone.Text.Trim();
            dr["City"] = textBoxCity.Text.Trim();
            dr["Street"] = textBoxStreet.Text.Trim();
            dr["PostalCode"] = textBoxPostalCode.Text.Trim();

            dtCustomers.Rows.Add(dr);

            try
            {
                da.Update(dtCustomers);   // use the DataTable, not dsSalesDB string name
                MessageBox.Show("Customer added to database successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}");
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxStudentId.Text.Trim(), out int id))
            {
                Sales foundCustomer = CustomerDB.SearchRecord(id);
                if (foundCustomer != null)
                {
                    textBoxFirstName.Text = foundCustomer.Name;
                    //textBoxLastName.Text = foundCustomer.LastName;
                    textBoxEmail.Text = foundCustomer.Email;
                    textBoxPhone.Text = foundCustomer.Phone;
                    textBoxCity.Text = foundCustomer.City;
                    textBoxStreet.Text = foundCustomer.Street;
                    textBoxPostalCode.Text = foundCustomer.PostalCode;
                }
                else
                {
                    MessageBox.Show("Customer not found in database!", "Search Result");
                }
            }
            else
            {
                MessageBox.Show("Enter a valid Customer ID.");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxStudentId.Text.Trim(), out int id))
            {
                DataRow dr = dtCustomers.Rows.Find(id);
                if (dr != null)
                {
                    dr.Delete();
                    da.Update(dtCustomers);  // ✅ use dtCustomers
                    MessageBox.Show("Customer deleted from database successfully!");
                }
                else
                {
                    MessageBox.Show("Customer not found.");
                }
            }
            else
            {
                MessageBox.Show("Enter a valid Customer ID.");
            }
        }

        private void buttonUpdateDB_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxStudentId.Text.Trim(), out int id))
            {
                DataRow dr = dtCustomers.Rows.Find(id);
                if (dr != null)
                {
                    dr["Name"] = textBoxFirstName.Text.Trim();
                    dr["Email"] = textBoxEmail.Text.Trim();
                    dr["Phone"] = textBoxPhone.Text.Trim();
                    dr["City"] = textBoxCity.Text.Trim();
                    dr["Street"] = textBoxStreet.Text.Trim();
                    dr["PostalCode"] = textBoxPostalCode.Text.Trim();

                    da.Update(dtCustomers);  // ✅ use dtCustomers
                    MessageBox.Show("Customer updated in database successfully!");
                }
                else
                {
                    MessageBox.Show("Customer not found in dataset.");
                }
            }
            else
            {
                MessageBox.Show("Enter a valid Customer ID.");
            }
        }

        private void buttonListStudentFromDB_Click(object sender, EventArgs e)
        {
            List<Sales> customerList = CustomerDB.GetAllRecords();
            if (customerList != null && customerList.Count > 0)
            {
                dataGridViewCustomerFromDB.DataSource = customerList;
            }
            else
            {
                MessageBox.Show("No customer data in the database.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SalesForm_Load(object sender, EventArgs e)
        {
            dsSalesDB = new DataSet("Hitech_ds");
            dtCustomers = new DataTable("Customers");
            dsSalesDB.Tables.Add(dtCustomers);
            da = new SqlDataAdapter(
        "SELECT CustomerID, Name, Email, Phone, City, Street, PostalCode FROM Customers",
        UtilityDB.GetDBConnection());
            cmdBuilder = new SqlCommandBuilder(da);
            da.Fill(dtCustomers);
            dtCustomers.PrimaryKey = new DataColumn[] { dtCustomers.Columns["CustomerID"] };
            dataGridViewCustomerFromDB.DataSource = dtCustomers;

            dtCustomers.Columns["CustomerID"].AutoIncrement = true;
            dtCustomers.Columns["CustomerID"].AutoIncrementSeed = -1;  // temporary negative IDs
            dtCustomers.Columns["CustomerID"].AutoIncrementStep = -1;
            dtCustomers.Columns["CustomerID"].ReadOnly = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }
    }
}
