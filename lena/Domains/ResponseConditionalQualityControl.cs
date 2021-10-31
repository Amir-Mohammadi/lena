using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ResponseConditionalQualityControl : BaseEntity, IEntity
  {
    protected internal ResponseConditionalQualityControl()
    {
    }
    public ConditionalQualityControlStatus Status { get; set; }
    public int ConditionalQualityControlId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual ConditionalQualityControl ConditionalQualityControl { get; set; }
  }
}