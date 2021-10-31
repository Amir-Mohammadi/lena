using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlObservation
{
  public class EditStuffQualityControlObservationInput
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}