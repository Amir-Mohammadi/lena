using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPEventCategory
{
  public class ProjectERPEventCategoryComboInput
  {
    public short Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}