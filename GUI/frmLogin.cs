using Project_module_4.BLL;
using Project_module_4.DAL;
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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // 🔒 Input Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Username and Password!",
                                "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoginUser user = LoginUserDB.ValidateLogin(username, password);

            if (user == null)
            {
                MessageBox.Show("Invalid username or password",
                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                return;
            }

          
            Form nextForm = null;

            switch (user.JobTitle.Trim().ToLower())
            {
                case "mis manager":
                    nextForm = new MISManagerDashboard();
                    break;
                case "order clerk":
                    nextForm = new FormOrderClerk();
                    break;
                case "sales manager":
                    nextForm = new SalesForm();
                    break;
                case "inventory controller":
                    nextForm = new frmBook();
                    break;
                default:
                    MessageBox.Show("Unknown Role! Contact Administrator.");
                    return;
            }

            MessageBox.Show($"Welcome {user.Username}! Role: {user.JobTitle}");
           
            nextForm.Show();   // open role form
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
