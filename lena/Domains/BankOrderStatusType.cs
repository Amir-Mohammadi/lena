using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BankOrderStatusType : IEntity
  {
    protected internal BankOrderStatusType()
    {
      this.BankOrderLogs = new HashSet<BankOrderLog>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public string Code { get; set; }
    public virtual ICollection<BankOrderLog> BankOrderLogs { get; set; }
  }
}