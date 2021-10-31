using lena.Domains.Enums;
namespace lena.Models.Planning.StuffPriceDiscrepancy
{
  public class AcceptOrRejectStuffPriceDiscrepancyInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public bool IsConfirmed { get; set; }
    public string ConfirmationDescription { get; set; }
  }
}
