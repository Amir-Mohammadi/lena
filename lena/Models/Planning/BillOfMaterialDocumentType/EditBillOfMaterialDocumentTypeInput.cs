using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDocumentType
{
  public class EditBillOfMaterialDocumentTypeInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int[] AddUserIds { get; set; }
    public int[] DeleteUserIds { get; set; }
    public int[] AddUserGroupIds { get; set; }
    public int[] DeleteUserGroupIds { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
