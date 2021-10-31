using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum FinancialDocumentTypeResult : byte
  {
    Deposit = 0, // واریز
    Expense = 1, // هزینه
    PurchaseOrderCost = 2, // هزینه سفارش خرید
    CargoTransferCost = 3, // هزینه حمل محموله
    LadingDutyCost = 4, // هزینه گمرک بارنامه
    LadingTransferCost = 5, // هزینه حمل بارنامه
    Discount = 6, // تخفیف
    PurchaseOrderDiscount = 7, // تخفیف سفارش خرید
    SubmitOrder = 8, // ثبت سفارش
    DeliveryOrder = 9, // تحویل سفارش
    FinancialFactor = 10, //فاکتور مالی
    Transfer = 11, // انتقال
    Beginning = 12, // مانده اول دوره
    Correction = 13, // اصلاح مالی
    GivebackExitReceipt = 14, // کاهش مرجوعی
    QualityControlRejected = 15, // مردودی کنترل کیفی
    OtherLadingCost = 16, //  سایر هزینه های بارنامه
    SaleOfWaste = 17, //  فروش ضایعات
    Giveback = 18 // فروش مرجوعی
  }
}
