using lena.Domains.Enums;
namespace lena.Models
{
  public class EmployeeContactResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsMain { get; set; }
    public string ContactText { get; set; }
    public int ContactTypeId { get; set; }
    public string ContactTypeName { get; set; }
    public int? EmployeeId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}