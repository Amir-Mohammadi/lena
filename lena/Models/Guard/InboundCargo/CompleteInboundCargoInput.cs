using lena.Domains.Enums;
namespace lena.Models.Guard.InboundCargo
{
  public class CompleteInboundCargoInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
