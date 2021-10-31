using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class PurchaseOrderDetailInput
  {
    public int Id { get; set; }
    public double Qty { get; set; }
    public double OrderedQty { get; set; }
    public byte[] RowVersion { get; set; }
  }
}