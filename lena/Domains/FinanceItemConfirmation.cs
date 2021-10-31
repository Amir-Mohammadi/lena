using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinanceItemConfirmation : IEntity
  {
    protected internal FinanceItemConfirmation()
    { }
    public int Id { get; set; }
    public int FinanceItemId { get; set; }
    public int UserId { get; set; }
    public FinanceItemConfirmationStatus Status { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual FinanceItem FinanceItem { get; set; }
    public virtual FinanceItem LatestFinanceItem { get; set; }
  }
}
