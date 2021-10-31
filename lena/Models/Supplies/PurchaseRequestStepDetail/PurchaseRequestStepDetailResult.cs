using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequestStepDetail
{
  public class PurchaseRequestStepDetailResult
  {
    public int Id { get; set; }
    public Guid? DocumentId { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public int PurchaseRequestStepId { get; set; }
    public string PurchaseRequestStepName { get; set; }
    public bool AllowUploadDocument { get; set; }
    public string PurchaseRequestCode { get; set; }
    public int PurchaseRequestId { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
