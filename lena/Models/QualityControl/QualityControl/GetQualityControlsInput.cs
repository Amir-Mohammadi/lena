using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControl
{
  public class GetQualityControlsInput : SearchInput<QualityControlSortType>
  {
    public GetQualityControlsInput(PagingInput pagingInput, QualityControlSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int[] Ids { get; set; }
    public int? StuffId { get; set; }
    public string Serial { get; set; }
    public QualityControlType? QualityControlType { get; set; }
    public StoreReceiptType? StoreReceiptType { get; set; }
    public QualityControlStatus? Status { get; set; }
    public QualityControlStatus? HasNotStatus { get; set; }
    public int? WarehouseId { get; set; }
    public int? DepartmentId { get; set; }
    public int? StuffPurchaseCategoryQualityControlDepartmentId { get; set; }
    public int? StuffPurchaseCategoryId { get; set; }
    public int? EmployeeId { get; set; }
    public int? CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public int? StoreReceiptId { get; set; }
    public int? ReceiptId { get; set; }
    public string QualityControlCode { get; set; }
    public string ReceiptCode { get; set; }
    public string StoreReceiptCode { get; set; }
    public bool GetAllQCType { get; set; }
    public bool? IsEmergency { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToConfirmationDate { get; set; }
    public DateTime? FromConfirmationDate { get; set; }
    public DateTime? FromStoreReceiptDate { get; set; }
    public DateTime? ToStoreReceiptDate { get; set; }
    public DateTime? FromInboundCargoDate { get; set; }
    public DateTime? ToInboundCargoDate { get; set; }
  }
}
