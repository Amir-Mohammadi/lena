using lena.Domains.Enums;
namespace lena.Models.Guard.OutboundCargo
{
  public class AddOutboundCargoExitReceiptInput
  {
    public int Id { get; set; }
    public double? Price { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
