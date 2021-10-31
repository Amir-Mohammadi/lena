using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProviderHowToBuy : IEntity
  {
    protected internal ProviderHowToBuy()
    {
    }
    public int ProviderId { get; set; }
    public short HowToBuyId { get; set; }
    public short LeadTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Cooperator Provider { get; set; }
    public virtual HowToBuy HowToBuy { get; set; }
  }
}
