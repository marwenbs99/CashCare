using System.ComponentModel;

namespace CashCare.Models.Wallet.Enum
{
    public enum DebtType
    {
        [Description("Personal Debt")]
        PersonalDebt,
        [Description("Student Debt")]
        StudentDebt,
        [Description("Consumer Debt")]
        ConsumerDebt,
        [Description("Medical Debt")]
        MedicalDebt,
        [Description("Others")]
        Others,
    }
}
