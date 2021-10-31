using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffQualityControlTest : IEntity
  {
    protected internal StuffQualityControlTest()
    {
      this.QualityControlConfirmationTests = new HashSet<QualityControlConfirmationTest>();
    }
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public string MeasurementMethod { get; set; }
    public string Frequency { get; set; }
    public string CorrectiveReaction { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public byte[] RowVersion { get; set; }
    public long? QualityControlTestConditionQualityControlTestId { get; set; }
    public int? QualityControlTestConditionTestConditionId { get; set; }
    public long? QualityControlTestEquipmentQualityControlTestId { get; set; }
    public int? QualityControlTestEquipmentTestEquipmentId { get; set; }
    public long? QualityControlTestOperationQualityControlTestId { get; set; }
    public int? QualityControlTestOperationTestOperationId { get; set; }
    public long? QualityControlTestImportanceDegreeQualityControlTestId { get; set; }
    public int? QualityControlTestImportanceDegreeTestImportanceDegreeId { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual QualityControlTest QualityControlTest { get; set; }
    public virtual QualityControlTestCondition QualityControlTestCondition { get; set; }
    public virtual QualityControlTestEquipment QualityControlTestEquipment { get; set; }
    public virtual QualityControlTestOperation QualityControlTestOperation { get; set; }
    public virtual QualityControlTestImportanceDegree QualityControlTestImportanceDegree { get; set; }
    public virtual ICollection<QualityControlConfirmationTest> QualityControlConfirmationTests { get; set; }
    public virtual ICollection<StuffQualityControlTestCondition> StuffQualityControlTestConditions { get; set; }
    public virtual ICollection<StuffQualityControlTestEquipment> StuffQualityControlTestEquipments { get; set; }
    public virtual ICollection<StuffQualityControlTestOperation> StuffQualityControlTestOperations { get; set; }
    public virtual ICollection<StuffQualityControlTestImportanceDegree> StuffQualityControlTestImportanceDegrees { get; set; }
  }
}