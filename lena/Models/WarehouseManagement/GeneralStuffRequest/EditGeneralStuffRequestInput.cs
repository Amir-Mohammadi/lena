using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GeneralStuffRequest
{
  public class EditGeneralStuffRequestInput : AddGeneralStuffRequestInput
  {
    public int Id { get; set; }
    public double StuffRequestQty { get; set; }
    public double PurchaseRequestQty { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
