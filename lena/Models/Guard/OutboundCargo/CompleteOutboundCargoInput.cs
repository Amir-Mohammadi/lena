using lena.Domains.Enums;
namespace lena.Models.Guard.OutboundCargo
{
  public class CompleteOutboundCargoInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
