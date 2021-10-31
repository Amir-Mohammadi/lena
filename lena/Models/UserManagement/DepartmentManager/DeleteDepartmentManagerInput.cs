using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlan
{
  public class DeleteDepartmentManagerInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
  }
}
