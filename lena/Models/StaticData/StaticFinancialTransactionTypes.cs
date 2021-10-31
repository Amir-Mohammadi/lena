using lena.Domains.Enums;
using lena.Domains;
namespace lena.Models.StaticData
{
  public static class StaticFinancialTransactionTypes
  {
    static StaticFinancialTransactionTypes()
    {
      Deposit = new FinancialTransactionType
      {
        Id = 2,
        Title = "واریز",
        Factor = TransactionTypeFactor.Plus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = ""
      };
      Expense = new FinancialTransactionType
      {
        Id = 3,
        Title = "هزینه",
        Factor = TransactionTypeFactor.Minus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = ""
      };
      OrderDepositCorrection = new FinancialTransactionType
      {
        Id = 4,
        Title = "اصلاح واریز سفارش",
        Factor = TransactionTypeFactor.Plus,
        FinancialTransactionLevel = FinancialTransactionLevel.Order,
        Description = ""
      };
      OrderExpenseCorrection = new FinancialTransactionType
      {
        Id = 5,
        Title = "اصلاح هزینه سفارش",
        Factor = TransactionTypeFactor.Minus,
        FinancialTransactionLevel = FinancialTransactionLevel.Order,
        Description = ""
      };
      ImportToPurchaseOrder = new FinancialTransactionType
      {
        Id = 6,
        Title = "سفارش خرید",
        Factor = TransactionTypeFactor.Minus,
        FinancialTransactionLevel = FinancialTransactionLevel.Order,
        Description = ""
      };
      ExportFromPurchase = new FinancialTransactionType
      {
        Id = 7,
        Title = "تحویل سفارش",
        Factor = TransactionTypeFactor.Plus,
        FinancialTransactionLevel = FinancialTransactionLevel.Order,
        Description = ""
      };
      ImportToCargo = new FinancialTransactionType
      {
        Id = 8,
        Title = "تحویل سفارش",
        Factor = TransactionTypeFactor.Minus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = ""
      };
      ExportFromCargo = new FinancialTransactionType
      {
        Id = 9,
        Title = "حذف از محموله",
        Factor = TransactionTypeFactor.Plus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = ""
      };
      AccountDepositCorrection = new FinancialTransactionType
      {
        Id = 10,
        Title = "اصلاح واریز مالی",
        Factor = TransactionTypeFactor.Plus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = ""
      };
      AccountExpenseCorrection = new FinancialTransactionType
      {
        Id = 11,
        Title = "اصلاح هزینه مالی",
        Factor = TransactionTypeFactor.Minus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = ""
      };
      TransferExpense = new FinancialTransactionType
      {
        Id = 13,
        Title = "انتقال-برداشت",
        Factor = TransactionTypeFactor.Minus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = ""
      };
      TransferDeposit = new FinancialTransactionType
      {
        Id = 14,
        Title = "انتقال-واریز",
        Factor = TransactionTypeFactor.Plus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = ""
      };
      GivebackExitReceipt = new FinancialTransactionType
      {
        Id = 15,
        Title = "کاهش-مرجوعی",
        Factor = TransactionTypeFactor.Minus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = "کم کردن از حساب مالی مشتری زمان مرجوعی"
      };
      QualityControlRejected = new FinancialTransactionType
      {
        Id = 16,
        Title = "مردودی کنترل کیفی",
        Factor = TransactionTypeFactor.Plus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = ""
      };
      SaleOfWaste = new FinancialTransactionType
      {
        Id = 17,
        Title = "فروش ضایعات",
        Factor = TransactionTypeFactor.Plus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = "فروش ضایعات"
      };
      Giveback = new FinancialTransactionType
      {
        Id = 18,
        Title = "فروش مرجوعی",
        Factor = TransactionTypeFactor.Plus,
        FinancialTransactionLevel = FinancialTransactionLevel.Account,
        Description = "فروش مرجوعی"
      };
    }
    public static FinancialTransactionType Deposit { get; }
    public static FinancialTransactionType Expense { get; }
    public static FinancialTransactionType OrderExpenseCorrection { get; }
    public static FinancialTransactionType OrderDepositCorrection { get; }
    public static FinancialTransactionType AccountExpenseCorrection { get; }
    public static FinancialTransactionType AccountDepositCorrection { get; }
    public static FinancialTransactionType TransferExpense { get; }
    public static FinancialTransactionType TransferDeposit { get; }
    public static FinancialTransactionType ImportToPurchaseOrder { get; }
    public static FinancialTransactionType ExportFromPurchase { get; }
    public static FinancialTransactionType ImportToCargo { get; }
    public static FinancialTransactionType ExportFromCargo { get; }
    public static FinancialTransactionType GivebackExitReceipt { get; }
    public static FinancialTransactionType QualityControlRejected { get; }
    public static FinancialTransactionType SaleOfWaste { get; }
    public static FinancialTransactionType Giveback { get; }
  }
}