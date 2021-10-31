using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffQualityControlTestImportanceDegree : IEntity
  {
    protected internal StuffQualityControlTestImportanceDegree()
    {
    }
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public long QualityControlTestImportanceDegreeQualityControlTestId { get; set; }
    public int QualityControlImportanceDegreeTestImportanceDegreeId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StuffQualityControlTest StuffQualityControlTest { get; set; }
    public virtual QualityControlTestImportanceDegree QualityControlTestImportanceDegree { get; set; }
  }
}