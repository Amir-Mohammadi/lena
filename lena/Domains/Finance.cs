using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Finance : IEntity
  {
    protected internal Finance()
    {
      this.FinanceConfirmations = new HashSet<FinanceConfirmation>();
      this.FinanceItems = new HashSet<FinanceItem>();
      this.FinancialDocuments = new HashSet<FinancialDocument>();
      this.FinanceItemAllocationSummaries = new HashSet<FinanceItemAllocationSummary>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public int CooperatorId { get; set; }
    public Nullable<int> FinanacialAccountDetailId { get; set; }
    public DateTime DateTime { get; set; }
    public byte CurrencyId { get; set; }
    public int UserId { get; set; }
    public int? LastConfimationId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual FinancialAccountDetail FinancialAccountDetail { get; set; }
    public virtual Currency Currency { get; set; }
    public virtual Cooperator Cooperator { get; set; }
    public virtual User User { get; set; }
    public virtual FinanceConfirmation LatestFinanceConfirmation { get; set; }
    public virtual ICollection<FinanceConfirmation> FinanceConfirmations { get; set; }
    public virtual ICollection<FinanceItem> FinanceItems { get; set; }
    public virtual ICollection<FinancialDocument> FinancialDocuments { get; set; }
    public virtual ICollection<FinanceAllocation> FinanceAllocations { get; set; }
    public virtual FinanceAllocationSummary FinanceAllocationSummary { get; set; }
    public virtual ICollection<FinanceItemAllocationSummary> FinanceItemAllocationSummaries { get; set; }
  }
}