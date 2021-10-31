using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialAccountDetail : IEntity
  {
    protected internal FinancialAccountDetail()
    {
      this.Finances = new HashSet<Finance>();
    }
    public int Id { get; set; }
    public string Account { get; set; }
    public FinancialAccountDetailType Type { get; set; }
    public string AccountOwner { get; set; }
    public byte BankId { get; set; }
    public int FinancialAccountId { get; set; }
    public bool IsArchive { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Bank Bank { get; set; }
    public virtual FinancialAccount FinancialAccount { get; set; }
    public virtual ICollection<Finance> Finances { get; set; }
  }
}
