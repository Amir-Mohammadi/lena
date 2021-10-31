using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffFractionByProject
{
  public class StuffFractionStuffInput
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string ProjectCode { get; set; }
    public double Qty { get; set; }

    public StuffFractionSemiProductInput[] SemiProductsInfo { get; set; }
  }
}
