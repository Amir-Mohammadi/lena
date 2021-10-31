using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class FullOrderItemResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int OrderId { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double Qty { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string OrderDescription { get; set; }
    public string Description { get; set; }
    public double ProducedQty { get; set; }
    public double PlannedQty { get; set; }
    public double BlockedQty { get; set; }
    public double PermissionQty { get; set; }
    public double SendedQty { get; set; }
    public OrderItemStatus Status { get; set; }
    public int? ProductPackBillOfMaterialStuffId { get; set; }
    public string ProductPackBillOfMaterialStuffCode { get; set; }
    public int? ProductPackBillOfMaterialVersion { get; set; }
    public StuffType? ProductPackBillOfMaterialStuffType { get; set; }
    public bool HasChange { get; set; }
    public byte[] RowVersion { get; set; }
    public byte[] OrderRowVersion { get; set; }

  }
}
