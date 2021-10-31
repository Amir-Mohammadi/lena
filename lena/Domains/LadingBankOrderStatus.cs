using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LadingBankOrderStatus : IEntity
  {
    protected internal LadingBankOrderStatus()
    {
      this.LadingBankOrderLogs = new HashSet<LadingBankOrderLog>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<LadingBankOrderLog> LadingBankOrderLogs { get; set; }
  }
}
