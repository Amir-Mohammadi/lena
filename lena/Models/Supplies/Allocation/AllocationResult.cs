using System;
using System.Linq;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AllocationResult
  {
    public int Id { get; set; }
    public Guid? DocumentId { get; set; }
    public int BankOrderId { get; set; }
    public string BankOrderNumber { get; set; }
    public double Amount { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public int Duration { get; set; }
    public AllocationStatus Status { get; set; } //مرحله اقدام
    public DateTime ReceivedDateTime { get; set; } // تاریخ دریافت
    public DateTime BeginningDateTime { get; set; } // تاریخ اقدام
    public DateTime FinalizationDateTime { get; set; } // تاریخ اتمام
    public int StatisticalRegistrationCertificate { get; set; }//شماره گواهی ثبت آماری                  
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public bool HasBankOrderIssue { get; set; }
    public IQueryable<BankOrderIssueResult> BankOrderIssueResult { get; set; }
    public byte[] RowVersion { get; set; }

  }
}