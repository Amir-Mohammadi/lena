using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffBasePriceTransport : IEntity
  {
    protected internal StuffBasePriceTransport()
    {
      this.Percent = 0D;
      this.Price = 0D;
    }
    public int Id { get; set; }
    public StuffBasePriceTransportType Type { get; set; }
    public Nullable<StuffBasePriceTransportComputeType> ComputeType { get; set; }
    public Nullable<double> Percent { get; set; }
    public Nullable<double> Price { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StuffBasePrice StuffBasePrice { get; set; }
    public int StuffBasePriceId { get; internal set; }
  }
}