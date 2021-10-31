using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffProductionFaultPercentegeReport
{
  public class GetProductionStuffDetailFaultPercentegeReportInput
  {
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }

  }
}
