using lena.Domains.Enums;
namespace lena.Models.Supplies.Supplier
{
  public class SupplierResult
  {
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public string EmployeeCode { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
