using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPEvent
{
  public class DeleteProjectERPEventInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
