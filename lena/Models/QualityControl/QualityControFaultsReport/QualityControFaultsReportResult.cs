﻿using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControFaultsReport
{
  public class QualityControFaultsReportResult
  {
    //public int Id { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int ProductionFaultTypeId { get; set; }
    public string ProductionFaultTypeTitle { get; set; }
    public int ProductionFaultTypeCount { get; set; }
  }
}
