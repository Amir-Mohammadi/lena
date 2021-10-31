using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GeneralStuffRequest
{
  public class ConfirmGeneralStuffRequestInput
  {
    public int Id { get; set; }
    public double ConfirmedQty { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public string Description { get; set; }
    public bool AddAlternativePurchaseRequest { get; set; }
    public DateTime? PurchaseRequestDeadline { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
