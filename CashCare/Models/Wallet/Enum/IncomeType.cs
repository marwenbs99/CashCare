using System.ComponentModel;

namespace CashCare.Models.Wallet.Enum
{
    public enum IncomeType
    {
        [Description("Monthly Salary")]
        MonthlySalary,
        [Description("Rental Income")]
        RentalIncome,
        [Description("Social Benefits")]
        SocialBenefits,
        [Description("Others")]
        Others,
    }
}
