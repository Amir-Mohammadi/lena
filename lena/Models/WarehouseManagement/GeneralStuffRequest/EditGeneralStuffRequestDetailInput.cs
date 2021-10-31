using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GeneralStuffRequest
{
  public class EditGeneralStuffRequestDetailInput : AddGeneralStuffRequestDetailInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
