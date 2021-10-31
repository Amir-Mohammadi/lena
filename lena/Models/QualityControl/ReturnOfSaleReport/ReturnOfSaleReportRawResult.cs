using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.ReturnOfSaleReport
{
  public class ReturnOfSaleReportRawResult
  {
    public int StuffId { get; set; }
    public int StuffCode { get; set; }
    public int StuffName { get; set; }
    public int CooperatorCode { get; set; }
    public int CooperatorName { get; set; }
    public DateTime SendDateTime { get; set; }
    public int QtyHaveBeenSent { get; set; }
  }
}
