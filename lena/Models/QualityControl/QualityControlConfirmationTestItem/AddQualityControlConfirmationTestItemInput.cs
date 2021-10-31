using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationTestItem
{
  public class AddQualityControlConfirmationTestItemInput
  {

    public int TesterUserId { get; set; }
    public int QualityControlConfirmationTestId { get; set; }
    public double? ObtainAmount { get; set; }
    public double? MinObtainAmount { get; set; }
    public double? MaxObtainAmount { get; set; }
    public int StuffId { get; set; }
    public string SampleName { get; set; }
    public long QualityControlTestId { get; set; }


  }
}
