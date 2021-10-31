

using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionStuffDetailReport
{
  public class ProductionStuffDetailReportResult
  {
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffNoun { get; set; }
    public string StuffName { get; set; }
    public string StuffCategoryName { get; set; }
    public string UnitName { get; set; }
    public double UnitConverstionRatio { get; set; }
    public double? Amount { get; set; }
    public DateTime? LastStoreReceiptDate { get; set; }
    public byte[] RowVersion { get; set; }


  }
}
