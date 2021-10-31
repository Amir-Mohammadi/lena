using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialPublishRequest
{
  public class AcceptBillOfMaterialUnpublishRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
