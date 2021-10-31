using lena.Models.UserManagement.User;
using lena.Models.UserManagement.UserGroup;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDocumentType
{
  public class FullBillOfMaterialDocumentTypeResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public UserResult[] Users { get; set; }
    public UserGroupResult[] UserGroups { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
