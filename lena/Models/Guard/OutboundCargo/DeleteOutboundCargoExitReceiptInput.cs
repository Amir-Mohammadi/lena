using lena.Domains.Enums;
namespace lena.Models.Guard.OutboundCargo
{
  public class DeleteOutboundCargoExitReceiptInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
