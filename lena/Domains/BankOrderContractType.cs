using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BankOrderContractType : IEntity
  {
    protected internal BankOrderContractType()
    {
      this.BankOrders = new HashSet<BankOrder>();
    }
    public short Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<BankOrder> BankOrders { get; set; }
  }
}