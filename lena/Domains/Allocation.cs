using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Allocation : IEntity, IHasSaveLog
  {
    protected internal Allocation()
    {
      this.BankOrderIssues = new HashSet<BankOrderIssue>();
    }
    public int Id { get; set; }
    public int BankOrderId { get; set; }
    public double Amount { get; set; }
    public byte CurrencyId { get; set; }
    public int Duration { get; set; }
    public AllocationStatus Status { get; set; } //مرحله اقدام
    public DateTime ReceivedDateTime { get; set; } // تاریخ دریافت
    public DateTime BeginningDateTime { get; set; } // تاریخ اقدام
    public DateTime FinalizationDateTime { get; set; } // تاریخ اتمام
    public int StatisticalRegistrationCertificate { get; set; }//شماره گواهی ثبت آماری                  
    public int UserId { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Document Document { get; set; }
    public virtual User User { get; set; }
    public virtual Currency Currency { get; set; }
    public virtual BankOrder BankOrder { get; set; }
    public virtual ICollection<BankOrderIssue> BankOrderIssues { get; set; }
  }
}