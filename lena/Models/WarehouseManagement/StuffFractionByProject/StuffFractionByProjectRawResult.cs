using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffFractionByProject
{
  public class StuffFractionByProjectRawResult
  {
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string UnitName { get; set; }
    public string LadingAmountByCode { get; set; }
    public string LadingStatusByCode { get; set; }
    public double Request { get; set; }
    public double Order { get; set; }
    public double Cargo { get; set; }
    public double Lading { get; set; }
    public double QualityControl { get; set; }
    public double Inventory { get; set; }
    public double Needed { get; set; }
    public double TotalFraction { get; set; }

  }
}
