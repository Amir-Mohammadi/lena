using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialLimit : IEntity
  {
    protected internal FinancialLimit()
    {
    }
    public int Id { get; set; }
    public int Allowance { get; set; }
    public bool IsArchive { get; set; }
    public byte CurrencyId { get; set; }
    public int UserId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Currency Currency { get; set; }
    public virtual User User { get; set; }
  }
}
