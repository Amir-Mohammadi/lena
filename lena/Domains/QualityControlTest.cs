using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlTest : IEntity
  {
    protected internal QualityControlTest()
    {
      //this.QualityControlConfirmationTests = new HashSet<QualityControlConfirmationTest>();
      this.StuffQualityControlTests = new HashSet<StuffQualityControlTest>();
      this.QualityControlTestConditions = new HashSet<QualityControlTestCondition>();
      this.QualityControlTestEquipments = new HashSet<QualityControlTestEquipment>();
    }
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<StuffQualityControlTest> StuffQualityControlTests { get; set; }
    public virtual ICollection<QualityControlTestCondition> QualityControlTestConditions { get; set; }
    public virtual ICollection<QualityControlTestEquipment> QualityControlTestEquipments { get; set; }
    public virtual ICollection<QualityControlTestOperation> QualityControlTestOperations { get; set; }
    public virtual ICollection<QualityControlTestImportanceDegree> QualityControlTestImportanceDegrees { get; set; }
  }
}