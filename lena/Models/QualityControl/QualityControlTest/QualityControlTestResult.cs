using lena.Models.QualityControl.QualityControlTestCondition;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTest
{
  public class QualityControlTestResult
  {
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IQueryable<QualityControlTestConditionResult> QualityControlTestConditions { get; set; }
    public IQueryable<QualityControlTestEquipmentResult> QualityControlTestEquipments { get; set; }
    public IQueryable<QualityControlTestImportanceDegreeResult> QualityControlTestImportanceDegrees { get; set; }
    public IQueryable<QualityControlTestOperationResult> QualityControlTestOperations { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
