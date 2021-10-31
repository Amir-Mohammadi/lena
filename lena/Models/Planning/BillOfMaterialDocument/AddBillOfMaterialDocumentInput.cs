using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDocument
{
  public class AddBillOfMaterialDocumentInput
  {
    public string Title { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public int BillOfMaterialDocumentTypeId { get; set; }
    public string Description { get; set; }
    public string FileKey { get; set; }
  }
}
