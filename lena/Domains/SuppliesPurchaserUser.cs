using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SuppliesPurchaserUser : IEntity
  {
    protected internal SuppliesPurchaserUser()
    {
    }
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsDefault { get; set; }
    public string Description { get; set; }
    public int PurchaserUserId { get; set; }
    public byte[] RowVersion { get; set; }
    public int StuffId { get; set; }
    public virtual User PurchaserUser { get; set; }
    public virtual Stuff Stuff { get; set; }
  }
}