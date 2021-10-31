using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlTestImportanceDegree : IEntity
  {
    protected internal QualityControlTestImportanceDegree()
    {
      this.StuffQualityControlTests = new HashSet<StuffQualityControlTest>();
    }
    public long QualityControlTestId { get; set; }
    public int TestImportanceDegreeId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual TestImportanceDegree TestImportanceDegree { get; set; }
    public virtual QualityControlTest QualityControlTest { get; set; }
    public ICollection<StuffQualityControlTestImportanceDegree> StuffQualityControlTestImportanceDegrees { get; set; }
    public virtual ICollection<StuffQualityControlTest> StuffQualityControlTests { get; set; }
  }
}