using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationTestItem
{
  public class EditQualityControlConfirmationTestItemInput
  {

    public int Id { get; set; }
    public int AQLAmount { get; set; }
    public int TesterUserId { get; set; }
    public double? ObtainAmount { get; set; }
    public double? MinObtainAmount { get; set; }
    public double? MaxObtainAmount { get; set; }
    public int QualityControlConfirmationTestId { get; set; }

    public byte[] RowVersion { get; set; }

  }
}
