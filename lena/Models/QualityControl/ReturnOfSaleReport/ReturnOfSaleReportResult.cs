using System.Dynamic;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.ReturnOfSaleReport
{
  public class ReturnOfSaleReportResult
  {
    public ExpandoObject[] Data { get; set; }
    public string[] DynamicColumnNames { get; set; }
  }
}
