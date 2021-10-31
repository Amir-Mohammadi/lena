using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class EditProductionPerformanceInput
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion;
  }
}
