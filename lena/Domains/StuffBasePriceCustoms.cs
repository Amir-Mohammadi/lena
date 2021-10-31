using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffBasePriceCustoms : IEntity
  {
    protected internal StuffBasePriceCustoms()
    {
      this.HowToBuyRatio = 0D;
    }
    public int Id { get; set; }
    public StuffBasePriceCustomsType Type { get; set; }
    public Nullable<double> Tariff { get; set; }
    public Nullable<double> Percent { get; set; }
    public Nullable<double> HowToBuyRatio { get; set; }
    public double Price { get; set; }
    public Nullable<short> HowToBuyId { get; set; }
    public Nullable<double> Weight { get; set; }
    public byte CurrencyId { get; set; }
    public int StuffBasePriceId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual HowToBuy HowToBuy { get; set; }
    public virtual Currency Currency { get; set; }
    public virtual StuffBasePrice StuffBasePrice { get; set; }
  }
}