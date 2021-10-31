using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Customhouse : IEntity
  {
    protected internal Customhouse()
    {
      this.BankOrders = new HashSet<BankOrder>();
      this.Ladings = new HashSet<Lading>();
    }
    public short Id { get; set; }
    public string Title { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<BankOrder> BankOrders { get; set; }
    public virtual ICollection<Lading> Ladings { get; set; }
  }
}