using lena.Models.QualityControl.StuffQualityControlTestCondition;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlTest
{
  public class EditStuffQualityControlTestDocument
  {
    public long QualityControlTestId { get; set; }
    public string FileKey { get; set; }

    public AddStuffQualityControlTestConditionInput[] AddStuffQualityControlTestConditionInputs { get; set; }
    public DeleteStuffQualityControlTestConditionInput[] DeleteStuffQualityControlTestConditionInputs { get; set; }
    public AddStuffQualityControlTestEquipmentInput[] AddStuffQualityControlTestEquipmentInputs { get; set; }
    public DeleteStuffQualityControlTestEquipmentInput[] DeleteStuffQualityControlTestEquipmentInputs { get; set; }
    public AddStuffQualityControlTestImportanceDegreeInput[] AddStuffQualityControlTestImportanceDegreeInputs { get; set; }
    public DeleteStuffQualityControlTestImportanceDegreeInput[] DeleteStuffQualityControlTestImportanceDegreeInputs { get; set; }
    public AddStuffQualityControlTestOperationInput[] AddStuffQualityControlTestOperationInputs { get; set; }
    public DeleteStuffQualityControlTestOperationInput[] DeleteStuffQualityControlTestOperationInputs { get; set; }
  }
}
