using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class HowToBuy : IEntity
  {
    protected internal HowToBuy()
    {
      this.HowToBuyDetails = new HashSet<HowToBuyDetail>();
      this.StuffBasePriceCustoms = new HashSet<StuffBasePriceCustoms>();
      this.ProviderHowToBuys = new HashSet<ProviderHowToBuy>();
      this.CargoItems = new HashSet<CargoItem>();
    }
    public short Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<HowToBuyDetail> HowToBuyDetails { get; set; }
    public virtual ICollection<StuffBasePriceCustoms> StuffBasePriceCustoms { get; set; }
    public virtual ICollection<ProviderHowToBuy> ProviderHowToBuys { get; set; }
    public virtual ICollection<CargoItem> CargoItems { get; set; }
  }
}
