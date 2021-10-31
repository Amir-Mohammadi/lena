using lena.Models.UserManagement.SecurityAction;

using lena.Domains;
namespace lena.Models.StaticData
{
  public static class WarehouseSecurityAction
  {
    static WarehouseSecurityAction()
    {
      ConfirmTheRequestForStuff = new SecurityActionResult()
      {
        Id = 0,
        Name = "تایید درخواست کالا از انبار  ",
        ActionName = "/api/WarehouseManagement/AcceptStuffRequestItem",
        SecurityActionGroupId = 30,
      };
      RejectTheRequestForStuff = new SecurityActionResult()
      {
        Id = 1,
        Name = "رد درخواست کالا از انبار ",
        ActionName = "/api/WarehouseManagement/RejectStuffRequestItem",
        SecurityActionGroupId = 30,
      };
      RemoveTheConfirmationOfStuffRequest = new SecurityActionResult()
      {
        Id = 2,
        Name = "حذف تایید درخواست کالا از انبار ",
        ActionName = "/api/WarehouseManagement/RemoveStuffRequestItemResponse",
        SecurityActionGroupId = 30,
      };
      WarehouseDraft = new SecurityActionResult()
      {
        Id = 3,
        Name = "حواله انبار ",
        ActionName = "/api/WarehouseManagement/AddWarehouseIssue",
        SecurityActionGroupId = 30,
      };
      ConfirmWarehouseDraft = new SecurityActionResult()
      {
        Id = 4,
        Name = "تایید حواله انبار ",
        ActionName = "/api/WarehouseManagement/AcceptWarehouseIssue",
        SecurityActionGroupId = 30,
      };
      RejectWarehouseDraft = new SecurityActionResult()
      {
        Id = 5,
        Name = "رد حواله انبار",
        ActionName = "/api/WarehouseManagement/RejectWarehouseIssue",
        SecurityActionGroupId = 30,
      };

    }

    public static SecurityActionResult[] WarehouseSecurityActions()
    {
      SecurityActionResult[] WarehouseSecurityActions = {
                ConfirmTheRequestForStuff,
                RejectTheRequestForStuff,
                RemoveTheConfirmationOfStuffRequest,
                WarehouseDraft,
                ConfirmWarehouseDraft,
                RejectWarehouseDraft

            };
      return WarehouseSecurityActions;
    }

    public static SecurityActionResult ConfirmTheRequestForStuff { get; }
    public static SecurityActionResult RejectTheRequestForStuff { get; }
    public static SecurityActionResult RemoveTheConfirmationOfStuffRequest { get; }
    public static SecurityActionResult WarehouseDraft { get; }
    public static SecurityActionResult ConfirmWarehouseDraft { get; }
    public static SecurityActionResult RejectWarehouseDraft { get; }

  }
}
