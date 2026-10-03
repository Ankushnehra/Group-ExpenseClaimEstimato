// Program.cs
// Staff Expense Claim Estimator - 5101 Assignment 2 C# logic proof-of-concept.
// Console application: collects inputs, validates them, calls ClaimCalculator, shows a summary.
// Out of scope (per the case briefing): storage, logins, approvals, GST, receipts.
using System;

namespace ExpenseClaimEstimator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Staff Expense Claim Estimator ===");
            Console.WriteLine("Estimate only - not a formal claim.\n");

            bool another = true;
            while (another)
            {
                RunOneEstimate();
                Console.Write("\nEstimate another expense? (y/n): ");
                string answer = Console.ReadLine();
                another = answer != null && answer.Trim().ToLower() == "y";
                Console.WriteLine();
            }
            Console.WriteLine("Goodbye.");
        }

        static void RunOneEstimate()
        {
            string name = AskText("Staff name: ", "Staff name");

            DateTime date = AskDate();
            ExpenseCategory category = AskCategory();

            decimal value;
            if (category == ExpenseCategory.Mileage)
            {
                value = AskNumber("Kilometres travelled: ", "Kilometres");
            }
            else
            {
                value = AskNumber("Expense amount ($): ", "Expense amount");
            }

            string purpose = AskText("Short description / purpose: ", "Description");

            ClaimResult result = ClaimCalculator.Estimate(category, value);
            ShowSummary(name, date, purpose, result);
        }

        // ---- input helpers: keep asking until the value passes validation ----
        static string AskText(string prompt, string fieldName)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();
                string error;
                if (InputValidator.ValidateText(text, fieldName, out error)) { return text.Trim(); }
                Console.WriteLine("  ! " + error);
            }
        }

        static DateTime AskDate()
        {
            while (true)
            {
                Console.Write("Expense date (dd/mm/yyyy): ");
                string text = Console.ReadLine();
                DateTime date; string error;
                if (InputValidator.ValidateDate(text, out date, out error)) { return date; }
                Console.WriteLine("  ! " + error);
            }
        }

        static ExpenseCategory AskCategory()
        {
            while (true)
            {
                Console.WriteLine("Category: 1) Mileage  2) Meal  3) Parking");
                Console.Write("Choose 1-3: ");
                string text = Console.ReadLine();
                ExpenseCategory category; string error;
                if (InputValidator.ValidateCategory(text, out category, out error)) { return category; }
                Console.WriteLine("  ! " + error);
            }
        }

        static decimal AskNumber(string prompt, string fieldName)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();
                decimal value; string error;
                if (InputValidator.ValidatePositiveNumber(text, fieldName, out value, out error)) { return value; }
                Console.WriteLine("  ! " + error);
            }
        }

        // ---- output ----
        static void ShowSummary(string name, DateTime date, string purpose, ClaimResult r)
        {
            Console.WriteLine("\n----- Claim Estimate Summary -----");
            Console.WriteLine("Staff name : " + name);
            Console.WriteLine("Date       : " + date.ToString("dd/MM/yyyy"));
            Console.WriteLine("Category   : " + r.Category);
            if (r.Category == ExpenseCategory.Mileage)
            {
                Console.WriteLine("Kilometres : " + r.EnteredValue + " km @ $" + ExpenseRules.MileageRatePerKm.ToString("F2") + "/km");
            }
            else
            {
                Console.WriteLine("Entered    : $" + r.EnteredValue.ToString("F2"));
            }
            Console.WriteLine("Purpose    : " + purpose);
            Console.WriteLine("ESTIMATED REIMBURSABLE AMOUNT: $" + r.EligibleAmount.ToString("F2"));
            if (r.WasAdjusted)
            {
                Console.WriteLine("Not eligible (above limit)   : $" + r.AmountAboveLimit.ToString("F2"));
                Console.WriteLine("NOTE: " + r.Notice);
            }
            Console.WriteLine("This is an estimate only; Finance approval is separate.");
            Console.WriteLine("----------------------------------");
        }
    }
}
