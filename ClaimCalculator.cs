// ClaimCalculator.cs
// Applies the supplied business rules. No Console input/output here, so the logic can be
// reused and checked independently of the user interface.
using System;

namespace ExpenseClaimEstimator
{
    public static class ClaimCalculator
    {
        // Feature 1: Mileage = kilometres x rate. Caller must already have checked km > 0.
        public static ClaimResult CalculateMileage(decimal kilometres)
        {
            ClaimResult result = new ClaimResult();
            result.Category = ExpenseCategory.Mileage;
            result.EnteredValue = kilometres;
            result.EligibleAmount = RoundMoney(kilometres * ExpenseRules.MileageRatePerKm);
            result.WasAdjusted = false;
            result.AmountAboveLimit = 0m;
            result.Notice = "";
            return result;
        }

        // Feature 2: Meal / Parking = lesser of the entered amount and the category maximum.
        // Caller must already have checked amount > 0.
        public static ClaimResult CalculateCapped(ExpenseCategory category, decimal amount)
        {
            decimal maximum = GetMaximum(category);
            amount = RoundMoney(amount);   // compare in whole cents so 35.004 is treated as $35.00, not "above" $35.00
            ClaimResult result = new ClaimResult();
            result.Category = category;
            result.EnteredValue = amount;

            if (amount > maximum)
            {
                result.EligibleAmount = maximum;
                result.WasAdjusted = true;
                result.AmountAboveLimit = amount - maximum;
                result.Notice = "Your entered amount of $" + amount.ToString("F2") +
                    " is above the " + category + " limit. The estimate has been adjusted to the maximum allowed amount of $" +
                    maximum.ToString("F2") + ".";
            }
            else
            {
                result.EligibleAmount = amount;   // exactly equal to the limit is NOT adjusted
                result.WasAdjusted = false;
                result.AmountAboveLimit = 0m;
                result.Notice = "";
            }
            return result;
        }

        public static ClaimResult Estimate(ExpenseCategory category, decimal value)
        {
            if (category == ExpenseCategory.Mileage)
            {
                return CalculateMileage(value);
            }
            return CalculateCapped(category, value);
        }

        private static decimal GetMaximum(ExpenseCategory category)
        {
            if (category == ExpenseCategory.Meal) { return ExpenseRules.MealMaximum; }
            if (category == ExpenseCategory.Parking) { return ExpenseRules.ParkingMaximum; }
            throw new ArgumentException("Only Meal and Parking have a maximum.");
        }

        // Money is rounded to whole cents; .005 rounds up (AwayFromZero) as people expect.
        private static decimal RoundMoney(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
