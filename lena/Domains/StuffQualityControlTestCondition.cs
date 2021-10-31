using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffQualityControlTestCondition : IEntity
  {
    protected internal StuffQualityControlTestCondition()
    {
    }
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public long QualityControlTestConditionQualityControlTestId { get; set; }
    public int QualityControlConditionTestConditionId { get; set; }
    public int QualityControlTestUnitId { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public string Description { get; set; }
    public ToleranceType ToleranceType { get; set; }
    public string AcceptanceLimit { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual QualityControlTestUnit QualityControlTestUnit { get; set; }
    public virtual StuffQualityControlTest StuffQualityControlTest { get; set; }
    public virtual QualityControlTestCondition QualityControlTestCondition { get; set; }
  }
}