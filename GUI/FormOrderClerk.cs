using Project_module_4.Models;
using Project_module_4.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project_module_4.Validator;

namespace Project_module_4.GUI
{
    public partial class FormOrderClerk : Form
    {
        private readonly OrderRepository orderRepo = new OrderRepository();
        private readonly BookRepository bookRepo = new BookRepository();
        private readonly CustomerRepository customerRepo = new CustomerRepository();
        private List<OrderDetail> tempDetails = new List<OrderDetail>();

        public FormOrderClerk()
        {
            InitializeComponent();
            LoadCustomers();
            LoadBooks();
            SetupGrid();
        }
        private void LoadCustomers()
        {
            cmbCustomer.DataSource = customerRepo.GetAllCustomers();
            cmbCustomer.DisplayMember = "Name";
            cmbCustomer.ValueMember = "CustomerID";
        }

        private void LoadBooks()
        {
            cmbBook.DataSource = bookRepo.GetAllBooks();
            cmbBook.DisplayMember = "Title";
            cmbBook.ValueMember = "ISBN";
        }

        private void SetupGrid()
        {
            dgvItems.AutoGenerateColumns = false;
            dgvItems.Columns.Clear();

            dgvItems.Columns.Add("OrderID", "Order ID");
            dgvItems.Columns.Add("ISBN", "ISBN");
            dgvItems.Columns.Add("Title", "Book Title");
            dgvItems.Columns.Add("Quantity", "Quantity");
            dgvItems.Columns.Add("UnitPrice", "Unit Price");
            dgvItems.Columns.Add("Total", "Total Price");
            dgvItems.Columns.Add("OrderDate", "Order Date");
            dgvItems.Columns.Add("Status", "Status");
        }


        private void FormOrderClerk_Load(object sender, EventArgs e)
        {
            cboOrderStatus.Items.Clear();
            cboOrderStatus.Items.Add("Pending");
            cboOrderStatus.Items.Add("In Progress");
            cboOrderStatus.Items.Add("Complete");

            cboOrderStatus.SelectedIndex = 0; // default to "Pending"
        }


        private void btnAddItem_Click(object sender, EventArgs e)
        {
            // Validate using OrderClerkValidator
            // Validate using OrderClerkValidator
            if (!Validation.ValidateOrderItem(cmbBook, txtQuantity, out string errorMsg))
            {
                MessageBox.Show(errorMsg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string isbn = cmbBook.SelectedValue.ToString();
            var book = bookRepo.GetBookByISBN(isbn);

            // Use correct property name (likely Qoh or Stock, not QOH)
            int quantity = int.Parse(txtQuantity.Text);  // Safe after validation
            if (book.QOH < quantity)  // or book.Stock or book.QuantityOnHand
            {
                MessageBox.Show($"Insufficient stock. Available: {book.QOH}", "Stock Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var detail = new OrderDetail
            {
                ISBN = isbn,
                Quantity = quantity,
                UnitPrice = book.UnitPrice
            };

            tempDetails.Add(detail);

            decimal total = quantity * book.UnitPrice;

            dgvItems.Rows.Add(
                "", isbn, book.Title, quantity, book.UnitPrice, total, "", ""
            );

            txtQuantity.Clear();
            cmbBook.SelectedIndex = -1;
        }

        private void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Select customer");
                return;
            }

            if (tempDetails.Count == 0)
            {
                MessageBox.Show("Add at least one item.");
                return;
            }

            var newOrder = new Order
            {
                CustomerID = (int)cmbCustomer.SelectedValue,
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderDetails = tempDetails
            };

            orderRepo.CreateOrder(newOrder);

            MessageBox.Show($"Order Created Successfully! Order ID: {newOrder.OrderID}");

            tempDetails.Clear();
            dgvItems.Rows.Clear();
        }

        private void btnSearchOrder_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtOrderIDSearch.Text, out int id))
            {
                MessageBox.Show("Enter valid order ID");
                return;
            }

            var order = orderRepo.GetOrderById(id);
            if (order == null)
            {
                MessageBox.Show("Order not found.");
                return;
            }

            dgvItems.Rows.Clear();

            foreach (var d in order.OrderDetails)
            {
                decimal total = d.Quantity * d.UnitPrice;

                dgvItems.Rows.Add(
                    order.OrderID,
                    d.ISBN,
                    d.Book.Title,
                    d.Quantity,
                    d.UnitPrice,
                    total,
                    order.OrderDate.ToString("yyyy-MM-dd"),
                    order.Status
                );
            }

            cboOrderStatus.SelectedItem = order.Status;
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtOrderIDSearch.Text, out id)) return;

            orderRepo.UpdateOrderStatus(id, cboOrderStatus.SelectedItem.ToString()
);
            MessageBox.Show("Status updated!");
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtOrderIDSearch.Text, out id)) return;

            orderRepo.CancelOrder(id);
            MessageBox.Show("Order cancelled!");

            dgvItems.Rows.Clear();
            cboOrderStatus.SelectedItem = -1;

        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var orders = orderRepo.GetAllOrders()
                      .OrderByDescending(o => o.OrderDate)
                      .ToList();

            dgvItems.Rows.Clear();

            foreach (var order in orders)
            {
                foreach (var d in order.OrderDetails)
                {
                    decimal total = d.Quantity * d.UnitPrice;

                    dgvItems.Rows.Add(
                        order.OrderID,
                        d.ISBN,
                        d.Book.Title,
                        d.Quantity,
                        d.UnitPrice,
                        total,
                        order.OrderDate.ToString("yyyy-MM-dd"),
                        order.Status
                    );
                }
            }
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbBook_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtOrderIDSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }
    }
}
