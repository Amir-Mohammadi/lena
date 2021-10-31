using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlTestUnit : IEntity
  {
    protected internal QualityControlTestUnit()
    {
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<StuffQualityControlTestCondition> StuffQualityControlTestConditions { get; set; }
  }
}