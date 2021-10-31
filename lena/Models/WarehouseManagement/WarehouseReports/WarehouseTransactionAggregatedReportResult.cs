using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseReports
{
  public class WarehouseTransactionAggregatedReportResult
  {

    public int StuffId { get; set; }
    public string StuffNoun { get; set; }
    public string StuffTitle { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double BeginTermQty { get; set; }
    public double BeginTermPrice { get; set; }
    public double IncomingQty { get; set; }
    public double IncomingPrice { get; set; }
    public double IssuedQty { get; set; }
    public double IssuedPrice { get; set; }
    public double RemainQty { get; set; }
    public double RemainPrice { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyName { get; set; }

  }
}
