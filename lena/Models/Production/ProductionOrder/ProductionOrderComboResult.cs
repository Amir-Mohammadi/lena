using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class ProductionOrderComboResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
