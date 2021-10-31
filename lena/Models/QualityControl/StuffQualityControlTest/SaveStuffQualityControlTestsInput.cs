using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlTest
{
  public class SaveStuffQualityControlTestsInput
  {
    public int StuffId { get; set; }
    public string MeasurementMethod { get; set; }
    public string Frequency { get; set; }
    public string CorrectiveReaction { get; set; }
    public AddStuffQualityControlTestDocument[] AddQualityControlTestInputs { get; set; }
    public EditStuffQualityControlTestDocument[] EditQualityControlTestInputs { get; set; }
    public long[] DeleteQualityControlTestIds { get; set; }
  }
}
