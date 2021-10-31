using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTaskCategory
{
  public class ProjectERPTaskCategoryComboInput
  {
    public short Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}