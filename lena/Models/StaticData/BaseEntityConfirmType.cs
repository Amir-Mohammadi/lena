using lena.Domains.Enums;
using lena.Domains;
namespace lena.Models.StaticData
{
  public static class StaticBaseEntityConfirmTypes
  {
    static StaticBaseEntityConfirmTypes()
    {
      PurchaseRequestConfirmation = new BaseEntityConfirmType()
      {
        Id = 1,
        ConfirmType = EntityType.PurchaseRequest,
        DepartmentId = (int)Departments.Warehouse,
        UserId = null,
        ConfirmPageUrl = "/pages/planning/purchase-request-list",
      };
      QtyCorrectionRequestConfirmation = new BaseEntityConfirmType()
      {
        Id = 2,
        ConfirmType = EntityType.QtyCorrectionRequest,
        DepartmentId = (int)Departments.Warehouse,
        UserId = null,
        ConfirmPageUrl = "/pages/warehouse/qtycorrectionrequestlist",
      };
      BasePriceConfirmation = new BaseEntityConfirmType()
      {
        Id = 3,
        ConfirmType = EntityType.StuffPrice,
        DepartmentId = (int)Departments.Accounting,
        UserId = null,
        ConfirmPageUrl = "/pages/Accounting/qtycorrectionrequestlist",
      };
      PurchaseOrderPriceConfirmation = new BaseEntityConfirmType()
      {
        Id = 4,
        ConfirmType = EntityType.PurchaseOrder,
        DepartmentId = (int)Departments.Supplies,
        UserId = null,
        ConfirmPageUrl = "/pages/Supplies/purchaseorderlist",
      };
    }
    public static lena.Domains.BaseEntityConfirmType PurchaseRequestConfirmation { get; }
    public static lena.Domains.BaseEntityConfirmType QtyCorrectionRequestConfirmation { get; }
    public static lena.Domains.BaseEntityConfirmType BasePriceConfirmation { get; }
    public static lena.Domains.BaseEntityConfirmType PurchaseOrderPriceConfirmation { get; }
  }
}