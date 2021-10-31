using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OrderType : IEntity
  {
    protected internal OrderType()
    {
      this.Orders = new HashSet<Order>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
  }
}
