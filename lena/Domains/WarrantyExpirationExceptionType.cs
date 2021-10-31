using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WarrantyExpirationExceptionType : IEntity
  {
    protected internal WarrantyExpirationExceptionType()
    {
      this.ConditionalQualityControls = new HashSet<ConditionalQualityControl>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string FreeOfCharge { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ConditionalQualityControl> ConditionalQualityControls { get; set; }
  }
}