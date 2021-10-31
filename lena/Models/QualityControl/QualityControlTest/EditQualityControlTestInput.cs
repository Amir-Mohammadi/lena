using lena.Models.QualityControl.TestCondition;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTest
{
  public class EditQualityControlTestInput
  {
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AddTestConditionInput[] AddTestConditions { get; set; }
    public DeleteTestConditionInput[] DeleteTestConditions { get; set; }
    public AddTestEquipmentInput[] AddTestEquipments { get; set; }
    public DeleteTestEquipmentInput[] DeleteTestEquipments { get; set; }
    public AddTestImportanceDegreeInput[] AddTestImportanceDegrees { get; set; }
    public DeleteTestImportanceDegreeInput[] DeleteTestImportanceDegrees { get; set; }
    public AddTestOperationInput[] AddTestOperations { get; set; }
    public DeleteTestOperationInput[] DeleteTestOperations { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
