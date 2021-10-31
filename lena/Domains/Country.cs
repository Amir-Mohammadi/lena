using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Country : IEntity
  {
    protected internal Country()
    {
      this.BankOrders = new HashSet<BankOrder>();
      this.Cities = new HashSet<City>();
    }
    public byte Id { get; set; }
    public string Title { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<BankOrder> BankOrders { get; set; }
    public virtual ICollection<City> Cities { get; set; }
  }
}