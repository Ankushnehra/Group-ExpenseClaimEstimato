// ClaimResult.cs
// Plain data holder returned by ClaimCalculator so that calculation and display stay separate.
namespace ExpenseClaimEstimator
{
    public class ClaimResult
    {
        public ExpenseCategory Category { get; set; }
        public decimal EnteredValue { get; set; }      // km for Mileage, dollars for Meal/Parking
        public decimal EligibleAmount { get; set; }    // estimated reimbursable amount in dollars
        public bool WasAdjusted { get; set; }          // true when a limit reduced the amount
        public decimal AmountAboveLimit { get; set; }  // dollars not eligible because of the limit
        public string Notice { get; set; }             // message shown to the user (may be empty)
    }
}
