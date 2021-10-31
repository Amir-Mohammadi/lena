using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.ReturnOfSaleReport
{
  public class GetReturnOfSaleReportInput
  {
    public List<string> Years { get; set; }
    public List<string> YearMonths { get; set; }
    public int? StuffId { get; set; }
    public int CooperatorId { get; set; }
    public bool IsMonthly { get; set; }
  }
}
