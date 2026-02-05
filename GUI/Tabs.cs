using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_module_4.GUI
{
    public partial class Tabs : Form
    {
        public Tabs()
        {
            InitializeComponent();
        }

        private void lstForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstForms.SelectedItem == null) return;

            string role = lstForms.SelectedItem.ToString();
            Form nextForm = null;

            switch (role)
            {
                case "Employee & User":
                    nextForm = new MISManagerDashboard();
                    break;
                case "Customer":
                    nextForm = new SalesForm();
                    break;
                case "Book":
                    nextForm = new frmBook();
                    break;
                case "Order":
                    nextForm = new FormOrderClerk();
                    break;
                default:
                    MessageBox.Show("Form not implemented.");
                    return;
            }

            nextForm.Show();
        }

        private void Tabs_Load(object sender, EventArgs e)
        {
            lstForms.Items.Add("Employee");
            lstForms.Items.Add("Customer");
            lstForms.Items.Add("Book");
            lstForms.Items.Add("Order");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }
    }
}
