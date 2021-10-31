using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffQualityControlTestOperation : IEntity
  {
    protected internal StuffQualityControlTestOperation()
    {
    }
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public long QualityControlTestOperationQualityControlTestId { get; set; }
    public int QualityControlOperationTestOperationId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StuffQualityControlTest StuffQualityControlTest { get; set; }
    public virtual QualityControlTestOperation QualityControlTestOperation { get; set; }
  }
}