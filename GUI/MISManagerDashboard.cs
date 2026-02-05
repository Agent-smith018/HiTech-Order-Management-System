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
    public partial class MISManagerDashboard : Form
    {
        
        public MISManagerDashboard()
        {
            InitializeComponent();
        }

        private void MISManagerDashboard_Load(object sender, EventArgs e)
        {

        }
        private void btnUsers_Click(object sender, EventArgs e)
        {
            try { new Project_module_4.GUI.frmUser().ShowDialog(); }
            catch { MessageBox.Show("UsersForm not ready"); }
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            try { new Project_module_4.GUI.FormEmployee().ShowDialog(); }
            catch { MessageBox.Show("EmployeesForm not ready"); }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
