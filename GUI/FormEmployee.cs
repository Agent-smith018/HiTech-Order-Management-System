using Project_module_4.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Project_module_4.GUI
{
    public partial class FormEmployee : Form
    {
        public FormEmployee()
        {
            InitializeComponent();
        }

        private void FormEmployee_Load(object sender, EventArgs e)
        {

        }

        private void buttonSaveEmployee_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFirstName.Text) || string.IsNullOrWhiteSpace(textBoxLastName.Text))
            {
                MessageBox.Show("First Name and Last Name required.", "Validation Error");
                return;
            }

            Employee emp = new Employee();
            emp.FirstName = textBoxFirstName.Text.Trim();
            emp.LastName = textBoxLastName.Text.Trim();
            emp.JobTitle = textBoxRole.Text.Trim();
            emp.Email = txtEmail.Text.Trim();

            bool success = emp.SaveEmployee(emp);  // ✅ Now returns bool
            if (success)
            {
                MessageBox.Show("Employee saved!", "Success");
                ClearAllFields();
                buttonListAll_Click(null, null);
            }
            else
            {
                MessageBox.Show("Save failed.", "Error");
            }
        }

        private void buttonListAll_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            var listS = employee.GetAllEmployees();
            listViewEmployee.Items.Clear();
            foreach (Employee emp in listS)
            {
                ListViewItem item = new ListViewItem(emp.EmployeeId.ToString());
                item.SubItems.Add(emp.FirstName);
                item.SubItems.Add(emp.LastName);
                item.SubItems.Add(emp.JobTitle);
                item.SubItems.Add(emp.Email);
                listViewEmployee.Items.Add(item);
            }
        }

        private void buttonSearchStudent_Click(object sender, EventArgs e)
        {
            string idInput = Interaction.InputBox("Enter Employee ID:", "Search", "");
            if (!int.TryParse(idInput, out int id))
            {
                MessageBox.Show("Enter valid ID.", "Invalid Input");
                return;
            }

            Employee employee = new Employee();
            var result = employee.SearchEmployee(id);
            if (result != null)
            {
                textBoxFirstName.Text = result.FirstName;
                textBoxLastName.Text = result.LastName;
                textBoxRole.Text = result.JobTitle;
                txtEmail.Text = result.Email;
            }
            else
            {
                MessageBox.Show("Employee not found.");
            }
        }

        private void buttonDeleteEmployee_Click(object sender, EventArgs e)
        {
            string idInput = Interaction.InputBox("Enter Employee ID to delete:", "Delete", "");
            if (!int.TryParse(idInput, out int empNumber))
            {
                MessageBox.Show("Enter valid ID.", "Invalid Input");
                return;
            }

            var confirm = MessageBox.Show($"Delete employee #{empNumber}?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    Employee employee = new Employee();
                    employee.DeleteEmployee(empNumber);
                    MessageBox.Show("Deleted successfully.");
                    ClearAllFields();
                    buttonListAll_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }
        private void ClearAllFields()
        {
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            textBoxRole.Clear();
            txtEmail.Clear();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBoxRole_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
