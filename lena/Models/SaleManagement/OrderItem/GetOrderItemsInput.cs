using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItem
{
  public class GetOrderItemsInput : SearchInput<OrderItemSortType>
  {
    public int? CustomerId { get; set; }
    public int? StuffId { get; set; }
    public DateTime? FromRequestDate { get; set; }
    public DateTime? ToRequestDate { get; set; }
    public DateTime? FromDeliveryDate { get; set; }
    public DateTime? ToDeliveryDate { get; set; }
    public int? OrderId { get; set; }
    public int[] OrderItemIds { get; set; }
    public string Code { get; set; }
    public OrderItemStatus? Status { get; set; }
    public OrderItemStatus[] Statuses { get; set; }
    public bool? HasChange { get; set; }
    public string DocumentNumber { get; set; }
    public OrderDocumentType? DocumentType { get; set; }
    public bool? IsArchive { get; set; }
    public OrderItemStatus[] NotHasStatuses { get; set; }
    public int? ProductPackBillOfMaterialStuffId { get; set; }
    public int? ProductPackBillOfMaterialVersion { get; set; }

    public GetOrderItemsInput(PagingInput pagingInput, OrderItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
