using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum ScrumTaskTypes : byte
  {
    ConfirmOrderItem = 1,
    CheckOrderItem = 2,
    ProductionPlan = 3,
    SendPermission = 4,
    ConfirmSendPermission = 5,
    ProductionSchedule = 6,
    PreparingSendig = 7,
    SendProduct = 8,
    PurchaseOrder = 9,
    ConfirmPurchaseOrderFinancing = 10,
    ComplatePurchaseOrderFinancing = 11,
    Shipping = 12,
    ShippingTracking = 13,
    ProductionOrder = 14,
    ProductionMaterialRequest = 15,
    CheckStuffRequest = 16,
    WarehouseIssue = 17,
    StoreReceipt = 18,
    SaveOutboundCargo = 19,
    QualityControlConfirmation = 20,
    ResponseConditionalQualityControl = 21,
    SaveReceiptPurchasePrice = 22,
    OrderItemChangeSaleConfirmation = 23,
    OrderItemChangePlanConfirmation = 24,
    ConfirmPurchaseRequest = 25
  }
}
