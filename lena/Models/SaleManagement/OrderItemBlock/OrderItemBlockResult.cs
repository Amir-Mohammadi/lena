using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemBlock
{
  public class OrderItemBlockResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string OrderTypeName { get; set; }
    public string Description { get; set; }
    public int OrderItemId { get; set; }
    public string OrderItemCode { get; set; }
    public double OrderItemQty { get; set; }
    public int OrderItemUnitId { get; set; }
    public string OrderItemUnitName { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public double PermissionQty { get; set; }
    public double PreparingSendingQty { get; set; }
    public double SendedQty { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public OrderItemBlockType OrderItemBlockType { get; set; }
    public ExitReceiptRequestStatus OrderItemBlockStatusType { get; set; }
    public byte[] RowVersion { get; set; }
    public string DocumentNumber { get; set; }
    public OrderDocumentType? DocumentType { get; set; }

  }
}
