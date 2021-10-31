using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPLabel
{
  public class EditProjectERPLabelInput
  {

    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
