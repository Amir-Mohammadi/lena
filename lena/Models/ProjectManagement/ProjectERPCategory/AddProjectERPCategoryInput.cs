using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPCategory
{
  public class AddProjectERPCategoryInput
  {
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
  }
}
