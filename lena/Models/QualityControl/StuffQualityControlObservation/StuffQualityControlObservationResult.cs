using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlObservation
{
  public class StuffQualityControlObservationResult
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public string RegisterarUserName { get; set; }
    public System.DateTime? RegisterDateTime { get; set; }
  }
}
