using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlAccepter : IEntity
  {
    protected internal QualityControlAccepter()
    {
      this.ConditionalQualityControls = new HashSet<ConditionalQualityControl>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public byte[] RowVersion { get; set; }
    public int UserGroupId { get; set; }
    public virtual ICollection<ConditionalQualityControl> ConditionalQualityControls { get; set; }
    public virtual UserGroup UserGroup { get; set; }
  }
}