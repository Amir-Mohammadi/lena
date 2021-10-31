using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GeneralStuffRequest
{
  public class AddPurchaseRequestGeneralStuffRequestInput
  {
    public int Id { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public DateTime Deadline { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
