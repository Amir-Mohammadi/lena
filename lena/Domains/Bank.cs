using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Bank : IEntity
  {
    protected internal Bank()
    {
      this.BankOrders = new HashSet<BankOrder>();
      this.FinancialAccountDetails = new HashSet<FinancialAccountDetail>();
    }
    public byte Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<BankOrder> BankOrders { get; set; }
    public virtual ICollection<FinancialAccountDetail> FinancialAccountDetails { get; set; }
  }
}