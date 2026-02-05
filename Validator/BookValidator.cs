using Project_module_4.DAL;
using Project_module_4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project_module_4.Validator
{
    public static class BookValidator
    {
        // MAIN entry point – no exceptions, returns bool
        public static bool Validate(Book book, bool checkDuplicate, out string errorMessage)
        {
            errorMessage = "";

            if (!IsValidISBN(book.ISBN))
            {
                errorMessage = "ISBN must be exactly 13 digits.";
                return false;
            }

            if (checkDuplicate && BookDB.Search(book.ISBN) != null)
            {
                errorMessage = "A book with this ISBN already exists.";
                return false;
            }

            if (!IsValidTitle(book.Title))
            {
                errorMessage = "Title must be at least 3 characters and contain only letters, digits, spaces, and punctuation.";
                return false;
            }

            if (!IsValidPrice(book.UnitPrice))
            {
                errorMessage = "Price must be greater than 0 and less than 1000.";
                return false;
            }

            if (!IsValidYear(book.YearPublished))
            {
                errorMessage = "Year must be between 1800 and the current year.";
                return false;
            }

            if (!IsValidQOH(book.QOH))
            {
                errorMessage = "QOH must be between 0 and 10,000.";
                return false;
            }

            if (!IsValidSelection(book.CategoryID))
            {
                errorMessage = "Please select a valid Category.";
                return false;
            }

            if (!IsValidSelection(book.PublisherID))
            {
                errorMessage = "Please select a valid Publisher.";
                return false;
            }

            return true; // VALID
        }

        // ===============================
        // INDIVIDUAL VALIDATORS
        // ===============================

        public static bool IsValidISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            return Regex.IsMatch(isbn.Trim(), @"^\d{13}$");
        }

        public static bool IsValidTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return false;

            if (title.Length < 3)
                return false;

            return Regex.IsMatch(title, @"^[A-Za-z0-9 ,.'\-]+$");
        }

        public static bool IsValidPrice(decimal price)
        {
            return price > 0 && price <= 1000;
        }

        public static bool IsValidYear(int year)
        {
            int current = DateTime.Now.Year;
            return year >= 1800 && year <= current;
        }

        public static bool IsValidQOH(int qoh)
        {
            return qoh >= 0 && qoh <= 10000;
        }

        public static bool IsValidSelection(int id)
        {
            return id > 0;
        }
    }
}
