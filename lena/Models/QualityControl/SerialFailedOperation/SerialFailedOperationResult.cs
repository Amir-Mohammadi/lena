using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.SerialFailedOperation
{
  public class SerialFailedOperationResult
  {
    public int Id { get; set; }
    public string Serial { get; set; }
    public int SuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string ReviewerUserFullName { get; set; }
    public DateTime? ReviewedDateTime { get; set; }
    public int FailedInOperationId { get; set; }
    public string FailedInOperationTitle { get; set; }
    public int? SerialFailedOperationFaultOperationId { get; set; }
    public int? FaultOperationId { get; set; }
    public string FaultOperationTitle { get; set; }
    public bool IsRepaired { get; set; }
    public string ProductionOrderCode { get; set; }
    public int ProductionLineId { get; set; }
    public int? ConfirmUserId { get; set; }
    public string ConfirmUserFullName { get; set; }
    public string Description { get; set; }
    public SerialFailedOperationStatus Status { get; set; }
    public byte[] RowVersion { get; set; }


  }
}
