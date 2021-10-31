using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItem
{
  public class EditOrderItemInput
  {
    public int Id { get; set; }
    public byte UnitId { get; set; }
    public double Qty { get; set; }
    public double CanceledQty { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string Description { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public byte[] RowVersion { get; set; }
  }
}