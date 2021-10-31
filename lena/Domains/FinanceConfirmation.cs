using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinanceConfirmation : IEntity
  {
    protected internal FinanceConfirmation()
    { }
    public int Id { get; set; }
    public int FinanceId { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public FinanceConfirmationStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Finance Finance { get; set; }
    public virtual Finance LatestFinance { get; set; }
    public virtual User User { get; set; }
  }
}
