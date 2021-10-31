using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDocumentType
{
  public class AddBillOfMaterialDocumentTypeInput
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public int[] UserIds { get; set; }
    public int[] UserGroupIds { get; set; }
  }
}
