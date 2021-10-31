using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class GetCargoItemDetailsInput : SearchInput<CargoItemDetailSortType>
  {
    public int? Id { get; set; }
    public int? CargoItemId { get; set; }
    public int[] CargoItemIds { get; set; }
    public bool IsDelete { get; set; }
    public int? CargoId { get; set; }
    public int? EmployeeId { get; set; }
    public string CargoItemCode { get; set; }
    public string CargoCode { get; set; }
    public string LadingCode { get; set; }
    public string PurchaseOrderCode { get; set; }
    public int? FinancialDocumentId { get; set; }
    public int? StuffId { get; set; }
    public int? HowToBuyId { get; set; }
    public int? HowToBuyDetailId { get; set; }
    public int? ProviderId { get; set; }
    public int? ForwarderId { get; set; }
    public DateTime? FromDeadLine { get; set; }
    public DateTime? ToDeadLine { get; set; }
    public DateTime? FromEstimateDate { get; set; }
    public DateTime? ToEstimateDate { get; set; }
    public DateTime? FromRegistrationDate { get; set; }
    public DateTime? ToRegistrationDate { get; set; }
    public RiskLevelStatus? RiskLevelStatus { get; set; }
    public PurchaseOrderType[] PurchaseOrderType { get; set; }
    public CargoItemStatus[] Statuses { get; set; }
    public CargoItemStatus[] NotHasStatuses { get; set; }
    public int[] Ids { get; set; }
    public int? FinancialTransactionBatchIdForCargoCost { get; set; }
    public int? FinancialTransactionBatchIdForDeliveryOrder { get; set; }
    public bool? IsArchived { get; set; }
    public bool? HasLading { get; set; }
    public int? PlanCodeId { get; set; }
    public int[] EmployeeIds { get; set; }
    public int[] ForwarderIds { get; set; }
    public string StuffCode { get; set; }
    public CargoItemStatus? CargoItemStatus { get; set; }
    public DateTime? PurchaseOrderDeadline { get; set; }
    public int? PurchaseOrderId { get; set; }
    public DateTime? ToDateTime { get; set; }
    public ProviderType ProviderType { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime? CargoItemDetailDeadLine { get; set; }
    public string HowToBuyDetailTitle { get; set; }




    public GetCargoItemDetailsInput(PagingInput pagingInput, CargoItemDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
