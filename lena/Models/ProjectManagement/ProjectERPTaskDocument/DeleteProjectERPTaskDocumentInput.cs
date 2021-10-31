using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTask
{
  public class DeleteProjectERPTaskDocumentInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
