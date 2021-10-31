using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WarehouseFiscalPeriod : IEntity
  {
    protected internal WarehouseFiscalPeriod()
    {
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool IsClosed { get; set; }
    public bool IsCurrent { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<BaseTransaction> BaseTransactions { get; set; }
  }
}