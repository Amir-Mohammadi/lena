using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumEntity
{
  public class ScrumEntityResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public int DepartmentId { get; set; }
    public bool IsDelete { get; set; }
    public string DepartmentName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
