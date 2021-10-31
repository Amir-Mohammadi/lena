using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialPublishRequest
{
  public class AddBillOfMaterialPublishRequestInput
  {
    public int StuffId { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public string Description { get; set; }
  }
}
