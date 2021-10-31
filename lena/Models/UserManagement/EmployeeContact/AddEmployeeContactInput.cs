using lena.Domains.Enums;
namespace lena.Models
{
  public class AddEmployeeContactInput
  {
    public string Title { get; set; }
    public string ContactText { get; set; }
    public bool IsMain { get; set; }
    public int ContactTypeId { get; set; }
    public int EmployeeId { get; set; }
  }
}
