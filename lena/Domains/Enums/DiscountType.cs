using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum DiscountType : byte
  {
    PurchaseOrderGroup = 0, // تخفیف برای تجمیع سفارش خرید
    PurchaseOrderItem = 1 // تخفیف برای آیتمهای سفارش خرید
  }
}
