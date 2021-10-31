using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class GetFullCargoItemsInput : SearchInput<FullCargoItemSortType>
  {
    public int? CargoItemId { get; set; }
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
    public StuffType? StuffType { get; set; }
    public int[] SelectedPlanCodeIds { get; set; }
    public GetFullCargoItemsInput(PagingInput pagingInput, FullCargoItemSortType sortType, SortOrder sortOrder)
        : base(pagingInput: pagingInput, sortType: sortType, sortOrder: sortOrder)
    {
    }
  }
}
