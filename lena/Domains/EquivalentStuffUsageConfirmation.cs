using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EquivalentStuffUsageConfirmation : BaseEntity, IEntity
  {
    protected internal EquivalentStuffUsageConfirmation()
    {
    }
    public int EquivalentStuffUsageId { get; set; }
    public EquivalentStuffUsageStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual EquivalentStuffUsage EquivalentStuffUsage { get; set; }
  }
}