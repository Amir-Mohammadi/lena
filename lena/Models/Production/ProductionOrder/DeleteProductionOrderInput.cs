using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class DeleteProductionOrderInput
  {
    public int Id { get; set; }
    public byte[] RowVersion;
  }
}
