using Project_module_4.DAL;
using Project_module_4.Models;
using Project_module_4.Validator;
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
    public partial class frmBook : Form
    {
        public frmBook()
        {
            InitializeComponent();
            LoadCategories();
            LoadPublishers();
            LoadAuthors();
            LoadBookList();
        }
        private void LoadAuthors()
        {
            cmbAuthor.DataSource = AuthorDB.GetAll();   // change from lstAuthors
            cmbAuthor.DisplayMember = "FullName";
            cmbAuthor.ValueMember = "AuthorID";
            cmbAuthor.SelectedIndex = -1;
        }
        private void LoadCategories()
        {
            cmbCategory.DataSource = CategoryDB.GetAll();
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryID";
            cmbCategory.SelectedIndex = -1;
        }
        private void LoadPublishers()
        {
            cmbPublisher.DataSource = PublisherDB.GetAll();
            cmbPublisher.DisplayMember = "PublisherName";
            cmbPublisher.ValueMember = "PublisherID";
            cmbPublisher.SelectedIndex = -1;
        }
        private void LoadBookList()
        {
            dataGridViewBooks.DataSource = BookDB.GetAllWithNamesAndAuthors();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                MessageBox.Show("Please enter or select a Category.", "Validation Error");
                return;
            }
            int catID;
            if (cmbCategory.SelectedValue == null)
            {
                catID = CategoryDB.AddIfNotExists(cmbCategory.Text.Trim());
                LoadCategories();
                cmbCategory.SelectedValue = catID;
            }
            else
            {
                catID = (int)cmbCategory.SelectedValue;
            }

            // PUBLISHER: select OR type new
            if (string.IsNullOrWhiteSpace(cmbPublisher.Text))
            {
                MessageBox.Show("Please enter or select a Publisher.", "Validation Error");
                return;
            }
            int pubID;
            if (cmbPublisher.SelectedValue == null)
            {
                pubID = PublisherDB.AddIfNotExists(cmbPublisher.Text.Trim());
                LoadPublishers();
                cmbPublisher.SelectedValue = pubID;
            }
            else
            {
                pubID = (int)cmbPublisher.SelectedValue;
            }

            // AUTHOR: select OR type new (single author)
            if (string.IsNullOrWhiteSpace(cmbAuthor.Text))
            {
                MessageBox.Show("Please enter or select an Author.", "Validation Error");
                return;
            }

            int authorID;
            if (cmbAuthor.SelectedValue == null)           // user typed a new author
            {
                try
                {
                    authorID = AuthorDB.AddIfNotExists(cmbAuthor.Text.Trim());
                    LoadAuthors();
                    cmbAuthor.SelectedValue = authorID;    // select the newly created one
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid author name: " + ex.Message, "Error");
                    return;
                }
            }
            else                                          // user picked from dropdown
            {
                authorID = (int)cmbAuthor.SelectedValue;
            }


            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) || unitPrice <= 0)
            {
                MessageBox.Show("Unit Price must be > 0.");
                return;
            }
            if (!int.TryParse(txtYearPublished.Text, out int year))
            {
                MessageBox.Show("Year must be a valid integer.");
                return;
            }
            if (!int.TryParse(txtQOH.Text, out int qoh) || qoh < 0)
            {
                MessageBox.Show("QOH must be >= 0.");
                return;
            }

            // 3. Create Book with SAFE values
            Book b = new Book
            {
                ISBN = txtISBN.Text.Trim(),
                Title = txtTitle.Text.Trim(),
                UnitPrice = unitPrice,
                YearPublished = year,
                QOH = qoh,
                CategoryID = catID,        // SAFE int, not null
                PublisherID = pubID        // SAFE int, not null

            };

            // 4. Your validator ✅
            if (!BookValidator.Validate(b, checkDuplicate: true, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5. Add to database ✅
            BookDB.Add(b);
            BookAuthorDB.AddBookAuthor(b.ISBN, authorID);
            MessageBox.Show("Book added successfully!");
            LoadBookList();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbPublisher_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtQOH_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtYearPublished_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtISBN_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string isbn = txtISBN.Text.Trim();

            if (!BookValidator.IsValidISBN(isbn))
            {
                MessageBox.Show("ISBN must be exactly 13 digits.",
                                "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Delete this book and its author links?",
                                "Confirm Delete",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    BookDB.Delete(isbn);   // Only this call
                    MessageBox.Show("Book deleted successfully.");
                    LoadBookList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete failed: " + ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpadte_Click(object sender, EventArgs e)
        {
            // 1) Safe numeric parsing
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) || unitPrice <= 0)
            {
                MessageBox.Show("Unit Price must be > 0.", "Validation Error");
                return;
            }
            if (!int.TryParse(txtYearPublished.Text, out int year))
            {
                MessageBox.Show("Year must be a valid integer.", "Validation Error");
                return;
            }
            if (!int.TryParse(txtQOH.Text, out int qoh) || qoh < 0)
            {
                MessageBox.Show("QOH must be >= 0.", "Validation Error");
                return;
            }

            // 2) Category: select OR type new
            if (string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                MessageBox.Show("Please enter or select a Category.", "Validation Error");
                return;
            }
            int catID;
            if (cmbCategory.SelectedValue == null)
            {
                catID = CategoryDB.AddIfNotExists(cmbCategory.Text.Trim());
                LoadCategories();
                cmbCategory.SelectedValue = catID;
            }
            else
            {
                catID = (int)cmbCategory.SelectedValue;
            }

            // 3) Publisher: select OR type new
            if (string.IsNullOrWhiteSpace(cmbPublisher.Text))
            {
                MessageBox.Show("Please enter or select a Publisher.", "Validation Error");
                return;
            }
            int pubID;
            if (cmbPublisher.SelectedValue == null)
            {
                pubID = PublisherDB.AddIfNotExists(cmbPublisher.Text.Trim());
                LoadPublishers();
                cmbPublisher.SelectedValue = pubID;
            }
            else
            {
                pubID = (int)cmbPublisher.SelectedValue;
            }

            // AUTHOR: select OR type new (single author)
            if (string.IsNullOrWhiteSpace(cmbAuthor.Text))
            {
                MessageBox.Show("Please enter or select an Author.", "Validation Error");
                return;
            }

            int authorID;
            if (cmbAuthor.SelectedValue == null)           // user typed a new author
            {
                try
                {
                    authorID = AuthorDB.AddIfNotExists(cmbAuthor.Text.Trim());
                    LoadAuthors();
                    cmbAuthor.SelectedValue = authorID;    // select the newly created one
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid author name: " + ex.Message, "Error");
                    return;
                }
            }
            else                                          // user picked from dropdown
            {
                authorID = (int)cmbAuthor.SelectedValue;
            }



            // 5) Build Book object
            Book b = new Book
            {
                ISBN = txtISBN.Text.Trim(),
                Title = txtTitle.Text.Trim(),
                UnitPrice = unitPrice,
                YearPublished = year,
                QOH = qoh,
                CategoryID = catID,
                PublisherID = pubID
                // Author is stored via BookAuthors; update that separately if needed
            };

            // 6) Validate (no duplicate check for update)
            if (!BookValidator.Validate(b, checkDuplicate: false, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation Error");
                return;
            }

            // 7) Update in DB
            BookDB.Update(b);

            BookAuthorDB.UpdateBookAuthor(b.ISBN, authorID);

            MessageBox.Show("Book updated successfully!");
            LoadBookList();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Book b = BookDB.Search(txtISBN.Text.Trim());

            if (b != null)
            {
                txtTitle.Text = b.Title;
                txtUnitPrice.Text = b.UnitPrice.ToString();
                txtYearPublished.Text = b.YearPublished.ToString();
                txtQOH.Text = b.QOH.ToString();

                cmbCategory.SelectedValue = b.CategoryID;
                cmbPublisher.SelectedValue = b.PublisherID;

                if (b.AuthorID.HasValue)
                    cmbAuthor.SelectedValue = b.AuthorID.Value;
                else
                    cmbAuthor.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Book not found.");
            }
        }

        private void cmbAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmBook_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }
    }
}
