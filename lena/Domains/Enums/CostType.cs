using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum CostType : byte
  {
    TransferCargo = 0, // هزینه حمل یک محموله
    TransferCargoItems = 1, // هزینه حمل آیتمهای محموله های مختلف

    TransferLading = 2, // هزینه حمل یک بارنامه
    TransferLadingItems = 3, // هزینه حمل آیتمهای بارنامه های مختلف

    DutyLading = 4, // هزینه گمرک یک بارنامه
    DutyLadingItems = 5, // هزینه گمرک بارنامه های مختلف

    PurchaseOrderGroup = 6, // هزینه سفارش خرید تجمیع شده
    PurchaseOrderItem = 7, // هزینه آیتمهای سفارش خرید 

    LadingOtherCosts = 8, // سایر هزینه ها برای بارنامه

    BankOrderOtherCosts = 9 //سایر هزینه ها برای سفارش بانکی
  }
}
