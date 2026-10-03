// ExpenseRules.cs
// Business rules supplied in the 5101 Assignment 2 Case Briefing (teaching case only,
// not NZ tax, employment or reimbursement rules). Keeping every rule value in one place
// means a policy change is a one-line edit.
namespace ExpenseClaimEstimator
{
    public enum ExpenseCategory
    {
        Mileage = 1,
        Meal = 2,
        Parking = 3
    }

    public static class ExpenseRules
    {
        public const decimal MileageRatePerKm = 0.85m;  // $ per kilometre
        public const decimal MealMaximum = 35.00m;      // $ per meal
        public const decimal ParkingMaximum = 40.00m;   // $ per expense
    }
}
