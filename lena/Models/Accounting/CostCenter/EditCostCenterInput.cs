using lena.Domains.Enums;
namespace lena.Models.Accounting.CostCenter
{
  public class EditCostCenterInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
