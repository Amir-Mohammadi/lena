using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDocument
{
  public class EditBillOfMaterialDocumentInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
    public int BillOfMaterialDocumentTypeId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
  }
}
