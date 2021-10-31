

using System;

using lena.Domains.Enums;
namespace lena.Models.Production.OperationTimeDiffReport
{
  public class OperationTimeDiffReportResult
  {

    public int Id { get; set; }
    public string UserCode { get; set; }
    public string UserFullName { get; set; }
    public DateTime FirstOperationTime { get; set; }
    public DateTime SecondOperationTime { get; set; }
    public int? OperationDifferenceTime { get; set; }
    public string FirstOperationCode { get; set; }
    public string SecondOperationCode { get; set; }
    public string FirstOperationName { get; set; }
    public string SecondOperationName { get; set; }
    public byte[] Rowversion { get; set; }
  }
}
