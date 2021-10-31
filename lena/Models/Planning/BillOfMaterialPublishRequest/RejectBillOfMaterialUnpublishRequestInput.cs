using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialPublishRequest
{
  public class RejectBillOfMaterialUnpublishRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
