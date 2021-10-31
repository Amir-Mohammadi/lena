using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.DetailedCodeConfirmationRequest
{
  public class DetailedCodeConfirmationRequestResult
  {
    public int Id { get; set; }
    public int? CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public string CooperatorCode { get; set; }
    public string CooperatorDetailedCode { get; set; }

    public int? ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public string ProductionLineDetailedCode { get; set; }

    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime? ConfirmationDateTime { get; set; }
    public int? ConfirmationUserId { get; set; }
    public string ConfirmationEmployeeFullName { get; set; }
    public CooperatorType? CooperatorType { get; set; }
    public DetailedCodeEntityType DetailedCodeEntityType { get; set; }
    public DetailedCodeRequestType DetailedCodeRequestType { get; set; }
    public DetailCodeConfirmationStatus Status { get; set; }
    public string Description { get; set; }
    public string DetailedCode { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
