using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class InboundCargoCooperator : IEntity
  {
    protected internal InboundCargoCooperator()
    {
    }
    public int CooperatorId { get; set; }
    public int InboundCargoId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Cooperator Cooperator { get; set; }
    public virtual InboundCargo InboundCargo { get; set; }
  }
}
