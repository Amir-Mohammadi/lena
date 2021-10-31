using lena.Domains;
namespace lena.Models.StaticData
{
  public class StaticActionName
  {
    public static string ConfirmStuffPrice => "/api/Supplies/ConfirmStuffPrice";
    public static string AcceptWarehouseIssue => "/api/WarehouseManagement/AcceptWarehouseIssue";
    public static string SaleOrderItemChangeConfirmationAction => "api/SaleManagement/AddOrderItemChangeConfirmation";
    public static string DownloadBillOfMaterialDocumentAction => "DownloadBillOfMaterialDocument";
    public static string GetAllEmployeeWorkReportList => "api/planning/GetAllEmployeeWorkReportList";
    public static string EvaluateAllEmployee => "api/planning/EvaluateAllEmployee";
    public static string EditWrongLimitCount => "api/production/EditWrongLimitCount";
    public static string EditProductionDateTimeIranKhdoroSerial => "api/production/EditProductionDateTimeIranKhdoroSerial";
    public static string GetAllPermissionRequestList => "api/SaleManagement/GetAllPermissionRequestList";
    public static string DeleteSerialPermissionInAddRepairProduction => "api/qualitycontrol/AddRepairProductionWithDeleteSerial";
    public static string DefineProductionTolerance => "api/qualitycontrol/DefineProductionTolerance";
    public static string AddStoreReceiptWihoutCheckingDateTime => "/api/WarehouseManagement/AddStoreReceiptWihoutCheckingDateTime"; // عملیات ثبت رسید انبار بدون اعمال محدودیت زمانی ورود تا رسید	
    public static string EditPurchaseRequestWithoutCheckOrder => "api/supplies/EditPurchaseRequestWithoutCheckOrder";

    public static string EmployeeComplainShow => "/api/UserManagement/EmployeeComplainShow";
  }
}
