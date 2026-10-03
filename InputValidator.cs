// InputValidator.cs
// Each method returns true when the text is acceptable. When it returns false,
// 'error' contains a clear message to show the user.
using System;
using System.Globalization;

namespace ExpenseClaimEstimator
{
    public static class InputValidator
    {
        public static bool ValidateText(string text, string fieldName, out string error)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                error = fieldName + " is required.";
                return false;
            }
            error = "";
            return true;
        }

        public static bool ValidateDate(string text, out DateTime date, out string error)
        {
            date = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(text))
            {
                error = "Expense date is required.";
                return false;
            }
            if (!DateTime.TryParseExact(text.Trim(), "d/M/yyyy", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out date))
            {
                error = "Expense date must be a real date in the format dd/mm/yyyy (e.g. 15/09/2026).";
                return false;
            }
            error = "";
            return true;
        }

        public static bool ValidateCategory(string text, out ExpenseCategory category, out string error)
        {
            category = ExpenseCategory.Mileage;
            int choice;
            if (!int.TryParse(text, out choice) || choice < 1 || choice > 3)
            {
                error = "Please choose 1 (Mileage), 2 (Meal) or 3 (Parking).";
                return false;
            }
            category = (ExpenseCategory)choice;
            error = "";
            return true;
        }

        // Used for kilometres and for dollar amounts: must be a number and greater than 0.
        public static bool ValidatePositiveNumber(string text, string fieldName, out decimal value, out string error)
        {
            value = 0m;
            if (string.IsNullOrWhiteSpace(text))
            {
                error = fieldName + " is required.";
                return false;
            }
            if (!decimal.TryParse(text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value))
            {
                error = fieldName + " must be a number (e.g. 12.5).";
                return false;
            }
            if (value <= 0m)
            {
                error = fieldName + " must be greater than 0.";
                return false;
            }
            error = "";
            return true;
        }
    }
}
