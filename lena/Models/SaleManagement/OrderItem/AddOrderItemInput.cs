using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddOrderItemInput
  {
    public int? ProductPackStuffId { get; set; }
    public short? ProductPackVersion { get; set; }
    public int OrderId { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public byte UnitId { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public double Qty { get; set; }
    public string Description { get; set; }
  }
}