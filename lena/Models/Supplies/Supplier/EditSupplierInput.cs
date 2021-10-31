using lena.Domains.Enums;
namespace lena.Models.Supplies.Supplier
{
  public class EditSupplierInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int? EmployeeId { get; set; }
    public string Description { get; set; }
  }
}
