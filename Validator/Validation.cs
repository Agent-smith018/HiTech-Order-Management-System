using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_module_4.Validator
{
    public static class Validation
    {
        public static bool ValidateOrderItem(ComboBox cmbBook, TextBox txtQuantity, out string errorMessage)
        {
            errorMessage = "";

            if (cmbBook.SelectedValue == null || cmbBook.SelectedIndex < 0)
            {
                errorMessage = "Please select a book from the list.";
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0 || quantity > 1000)
            {
                errorMessage = "Quantity must be between 1 and 1000.";
                return false;
            }

            return true;
        }


        public static bool ValidateOrderUpdate(TextBox txtOrderID, TextBox txtCustomerName,
                                     DataGridView dgvItems, out string errorMessage)
        {
            errorMessage = "";

            // Order ID required
            if (string.IsNullOrWhiteSpace(txtOrderID.Text) || !int.TryParse(txtOrderID.Text, out int orderId) || orderId <= 0)
            {
                errorMessage = "Valid Order ID required.";
                return false;
            }

            // Customer name reasonable length
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text) || txtCustomerName.Text.Length > 100)
            {
                errorMessage = "Customer name required (max 100 characters).";
                return false;
            }

            // Must have detail items
            if (dgvItems.Rows.Count == 0 || dgvItems.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                errorMessage = "At least one order item required.";
                return false;
            }

            // Validate each line item quantity
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.IsNewRow) continue;

                if (!int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int qty) || qty <= 0 || qty > 1000)
                {
                    errorMessage = $"Invalid quantity in row {row.Index + 1}: must be 1-1000.";
                    return false;
                }
            }

            return true;
        }


    }
}

