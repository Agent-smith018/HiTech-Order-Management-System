using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_module_4.GUI
{
    public partial class frmUser : Form
    {
        private SqlConnection conn;
        private SqlDataAdapter da;
        private DataTable dtUsers;

        public frmUser()
        {
            InitializeComponent();
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString);
            LoadUsers();
            LoadRoles();
        }
        private void LoadRoles()
        {
            comboBoxRole.Items.AddRange(new object[] { "MIS_Manager", "Sales_Manager", "Inventory_Controller", "Order_Clerk" });
        }

        private void LoadUsers()
        {
            try
            {
                string sql = @"
            SELECT 
                U.UserID,
                U.Username,
                U.Password,
                U.EmployeeID,
                E.JobTitle
            FROM Users U
            LEFT JOIN Employees E ON U.EmployeeID = E.EmployeeID";

                da = new SqlDataAdapter(sql, conn);
                dtUsers = new DataTable();
                da.Fill(dtUsers);
                dataGridView1.DataSource = dtUsers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load error: {ex.Message}");
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Users (Username, Password, EmployeeID) VALUES (@Username, @Password, @EmployeeID)", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", textBoxUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", textBoxPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@EmployeeID", int.Parse(textBoxEmployeeID.Text));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                LoadUsers();
                ClearFields();
                MessageBox.Show("User added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add error: {ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxUserID.Text, out int userID))
            {
                MessageBox.Show("Enter a valid UserID.");
                return;
            }

            if (!int.TryParse(textBoxEmployeeID.Text, out int employeeID))
            {
                MessageBox.Show("Enter a valid EmployeeID.");
                return;
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString))
                {
                    cn.Open();

                    // 1) Update Users (username, password, employee link)
                    using (SqlCommand cmdUser = new SqlCommand(
                        "UPDATE Users SET Username=@Username, Password=@Password, EmployeeID=@EmployeeID WHERE UserID=@UserID", cn))
                    {
                        cmdUser.Parameters.AddWithValue("@Username", textBoxUsername.Text.Trim());
                        cmdUser.Parameters.AddWithValue("@Password", textBoxPassword.Text.Trim());
                        cmdUser.Parameters.AddWithValue("@EmployeeID", employeeID);
                        cmdUser.Parameters.AddWithValue("@UserID", userID);

                        int rowsUser = cmdUser.ExecuteNonQuery();
                        if (rowsUser == 0)
                        {
                            MessageBox.Show("User not found to update.");
                            return;
                        }
                    }

                    // 2) Update Employees.JobTitle (role) for this employee
                    using (SqlCommand cmdEmp = new SqlCommand(
                        "UPDATE Employees SET JobTitle=@JobTitle WHERE EmployeeID=@EmployeeID", cn))
                    {
                        cmdEmp.Parameters.AddWithValue("@JobTitle", comboBoxRole.Text.Trim());
                        cmdEmp.Parameters.AddWithValue("@EmployeeID", employeeID);

                        cmdEmp.ExecuteNonQuery();
                    }
                }

                LoadUsers();      // reload grid with updated JobTitle
                ClearFields();
                MessageBox.Show("User and role updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update error: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxUserID.Text, out int userID))
            {
                MessageBox.Show("Enter a valid UserID.");
                return;
            }

            if (MessageBox.Show("Delete this user?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection cn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Users WHERE UserID=@UserID", cn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    cn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        MessageBox.Show("User not found to delete.");
                        return;
                    }
                }

                LoadUsers();
                ClearFields();
                MessageBox.Show("User deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete error: {ex.Message}");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxUserID.Text, out int userID))
            {
                MessageBox.Show("Enter a valid UserID.");
                return;
            }

            try
            {
                string sql = @"
            SELECT U.UserID, U.Username, U.Password, U.EmployeeID, E.JobTitle
            FROM Users U
            LEFT JOIN Employees E ON U.EmployeeID = E.EmployeeID
            WHERE U.UserID = @UserID";

                using (SqlConnection cn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxUsername.Text = reader["Username"].ToString();
                            textBoxPassword.Text = reader["Password"].ToString();
                            textBoxEmployeeID.Text = reader["EmployeeID"].ToString();
                            comboBoxRole.Text = reader["JobTitle"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("User not found!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search error: {ex.Message}");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            textBoxUserID.Clear();
            textBoxUsername.Clear();
            textBoxPassword.Clear();
            textBoxEmployeeID.Clear();
            comboBoxRole.SelectedIndex = -1;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                textBoxUserID.Text = dataGridView1.CurrentRow.Cells["UserID"].Value.ToString();
                textBoxUsername.Text = dataGridView1.CurrentRow.Cells["Username"].Value.ToString();
                textBoxPassword.Text = dataGridView1.CurrentRow.Cells["Password"].Value.ToString();
                textBoxEmployeeID.Text = dataGridView1.CurrentRow.Cells["EmployeeID"].Value.ToString();
                comboBoxRole.Text = dataGridView1.CurrentRow.Cells["JobTitle"].Value.ToString();
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    

        private void frmUser_Load(object sender, EventArgs e)
        {

        }

        private void frmUser_Load_1(object sender, EventArgs e)
        {

        }
    }
}
