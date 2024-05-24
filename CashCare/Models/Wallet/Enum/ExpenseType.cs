using System.ComponentModel;

namespace CashCare.Models.Wallet.Enum
{
    public enum ExpenseType
    {
        [Description("Housing Rent")]
        HousingRent,
        [Description("Tuition")]
        Tuition,
        [Description("Abonnements")]
        Abonnements,
        [Description("Transportation")]
        Transportation,
        [Description("Others")]
        Others,
    }
}
