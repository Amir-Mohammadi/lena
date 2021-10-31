using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum FinancialDocumentType : byte
  {
    Deposit = 1,
    Expense = 2,
    Discount = 3,
    Transfer = 8,
    Beginning = 14,
    Correction = 15
  }
}
