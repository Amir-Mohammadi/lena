using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectHeader
{
  public class AddProjectHeaderInput
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public short DepartmentId { get; set; }
    public int OwnerCustomerId { get; set; }
    public int? StuffId { get; set; }
  }
}
