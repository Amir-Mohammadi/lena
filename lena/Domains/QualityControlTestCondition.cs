using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlTestCondition : IEntity
  {
    protected internal QualityControlTestCondition()
    {
      this.StuffQualityControlTests = new HashSet<StuffQualityControlTest>();
    }
    public long QualityControlTestId { get; set; }
    public int TestConditionId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual TestCondition TestCondition { get; set; }
    public virtual QualityControlTest QualityControlTest { get; set; }
    public ICollection<StuffQualityControlTestCondition> StuffQualityControlTestConditions { get; set; }
    public virtual ICollection<StuffQualityControlTest> StuffQualityControlTests { get; set; }
  }
}