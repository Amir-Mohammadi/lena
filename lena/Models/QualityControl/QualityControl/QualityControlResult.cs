using lena.Domains.Enums;
using lena.Models.QualityControl.QualityControlItem;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControl
{
  public class QualityControlResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string StoreReceiptCode { get; set; }
    public string LadingCode { get; set; }
    public string CargoCode { get; set; }
    public string CargoItemCode { get; set; }
    public string PurchaseOrderCode { get; set; }
    public string ResponsibleFullNames { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double? UnitPrice { get; set; }
    public double? TotalPrice { get; set; }
    public int? CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public QualityControlType QualityControlType { get; set; }
    public StoreReceiptType? StoreReceiptType { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int? CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public QualityControlStatus Status { get; set; }
    public int? PayRequestId { get; set; }
    public PayRequestStatus? PayRequestStatus { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double? AcceptedQty { get; set; }
    public double? FailedQty { get; set; }
    public double? ConditionalRequestQty { get; set; }
    public double? ConditionalQty { get; set; }
    public double? ConditionalRejectedQty { get; set; }
    public double? ReturnedQty { get; set; }
    public double? ConsumedQty { get; set; }
    public string Description { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public int? StuffPurchaseCategoryId { get; set; }
    public string StuffPurchaseCategoryTitle { get; set; }
    public int? StuffPurchaseCategoryQualityControlDepartmentId { get; set; }
    public string StuffPurchaseCategoryQualityControlDepartmentName { get; set; }
    public int? StuffPurchaseCategoryQualityControlUserGroupId { get; set; }
    public string StuffPurchaseCategoryQualityControlUserGroupName { get; set; }
    public Guid? DocumentId { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime DateTime { get; set; }
    public int? ConfirmationUserId { get; set; }
    public int? ConfirmationEmployeeId { get; set; }
    public string ConfirmationEmployeeFullName { get; set; }
    public bool NeedToQualityControlDocumentUpload { get; set; }
    public DateTime? ConfirmationDateTime { get; set; }
    public DateTime? StoreReceiptDateTime { get; set; }
    public DateTime? InboundCargoDateTime { get; set; }
    public bool? IsEmergency { get; set; }
    public int? CargoId { get; set; }
    public int? CargoItemId { get; set; }
    public int? PurchaseOrderId { get; set; }
    public bool HasUploadedDocument { get; set; }
    public string ReceiptCode { get; set; }
    public int? ReceiptId { get; set; }
    public ReceiptStatus? ReceiptStatus { get; set; }
    public string QualityControlConfirmationDescription { get; set; }
    public int? QualityControlConfirmationId { get; set; }
    public double? Price { get; set; }
    public QualityControlPaymentSuggestStatus? QualityControlPaymentSuggestStatus { get; set; }
    public IQueryable<QualityControlItemResult> QualityControlItems { get; set; }

  }
}
